namespace EU4.Stats

open System
open System.Collections.Generic
open EU4.Stats.CountryExtensions
open EU4.Stats.Types
open EU4.Stats.WarStats
open EU4.Stats.TradeStats
open EU4.Stats.LedgerStats
open EU4.Stats.CountryStats
open EU4.Savegame
open MathNet.Numerics.Statistics

type SaveStats (save : Save) =
    let wars = Seq.append (nullToEmpty save.ActiveWars) (nullToEmpty save.PreviousWars)

    // Creates a sequence of wars with associated battles
    let battles =
        wars
        |> Seq.map (fun war ->
            let warBattles =
                if isNull war.History then Seq.empty else seq {
                    for event in war.History do
                        match event with
                        | :? BattleResult as b -> yield b
                        | _ -> ()
                }
            (war, warBattles))

    // Creates a sequence of countries with associated leaders
    let leaders =
        nullToEmpty save.Countries
        |> Seq.map (fun country ->
            let leads = 
                if isNull country.History then Seq.empty else seq {
                    for event in country.History do
                        match event with
                        | :? Leader as l -> yield l
                        | :? Monarch as m when notNull m.Leader -> yield m.Leader
                        | :? Heir as h when notNull h.Leader -> yield h.Leader
                        | _ -> ()
                }
            
            (country, leads))

    // Creates map of leader names to leaders
    let leaderMap =
        leaders
        |> Seq.collect (fun (country, leads) -> 
            leads |> Seq.map (fun x -> (x.Name, (x, country))))
        |> Map.ofSeq

    let countryMap = 
        nullToEmpty save.Countries
        |> Seq.map (fun x -> (x.Abbreviation, x))
        |> Map.ofSeq

    /// Computes the number of battles, forces, and losses in a type of battle (land
    /// or sea) in a war.
    let warReport fn =
        battles
        |> Seq.map (fun (war, allBattles) ->
            let battles = allBattles |> Seq.where (fun x -> (fn x.Attacker) <> 0)
            let attackers = battles |> Seq.map (fun x -> x.Attacker)
            let defenders = battles |> Seq.map (fun x -> x.Defender)
            let attackingWins = battles |> Seq.map (fun x -> x.Result)
                                |> Seq.where id |> Seq.length

            { name = war.Name; 
              attacker = countryMap.[war.OriginalAttacker].DisplayName;
              defender = countryMap.[war.OriginalDefender].DisplayName;
              attackingForces = attackers |> Seq.sumBy fn;
              defendingForces = defenders |> Seq.sumBy fn;
              attackingLosses = attackers |> Seq.sumBy (fun x -> x.Losses);
              defendingLosses = defenders |> Seq.sumBy (fun x -> x.Losses);
              battles = Seq.length battles;
              attackingWins = attackingWins;
              defendingWins = Seq.length battles - attackingWins
            })
        |> Seq.sortBy (fun x -> (~-) x.battles)

    let existantCountries = nullToEmpty save.Countries 
                            |> Seq.where(fun x -> x.NumOfCities <> 0)

    /// Summation of the available manpower for existing country
    member x.WorldManpower () = 
        int ((existantCountries |> Seq.sumBy(fun x -> x.Manpower)) * 1000.0)

    /// Summation of the max manpower for existing country
    member x.MaxWorldManpower () = 
        int ((existantCountries |> Seq.sumBy(fun x -> x.MaxManpower)) * 1000.0)

    member x.LandWarReport () = warReport landDeployments
    member x.NavalWarReport () = warReport navalDeployments

    /// For all the leaders that fought in a battle compile statistics based on
    /// their wins, losses, kills, etc.
    member x.LeaderReport () =
        battles
        |> Seq.collect snd
        |> Seq.collect (fun x -> seq {
                yield (x.Attacker, x.Defender, x.Result, x.Location)
                yield (x.Defender, x.Attacker, not x.Result, x.Location)
            })
        |> Seq.where (fun (side,_,_,_) -> side.Commander <> "")
        |> Seq.groupBy (fun (side,_,_,_) -> side.Commander)
        |> Seq.choose (fun (name, battles) -> 
            match (leaderMap.TryFind name) with
            | Some (leader, country) -> Some (leader, country, battles)
            | None -> None)
        |> Seq.map (fun (leader, country, battles) ->
            let forces, losses, faces, kills, wins =
                battles
                |> Seq.fold (fun (f,l,f2,k,w) (me,you,win,_) ->
                    (f + forces me,
                     l + me.Losses,
                     f2 + forces you,
                     k + you.Losses,
                     w + (if win then 1 else 0))) (0,0,0,0,0)
            { leader = leader; forces = forces; losses = losses; faced = faces;
              kills = kills; battles = Seq.length battles; wins = wins })



    /// Rivalries are calculated based on the number of times two commanders faced
    /// off on the battlefield. This statistics is a culmination of of all wars that
    /// the two commanders fought against each other in.
    member x.CommanderRivalries () =
        battles
        |> Seq.collect snd

        // We actually need to check to make sure that a country has the
        // specified commander as there has been commanders that are listed in
        // battles that do not show up under any country. It's very subtle but
        // the country that is listed in the fighting side may not be the same
        // country as the commander. I'm not sure in what circumstances this
        // occurs, but I know "He Li" of MNG has fought battles that have a
        // country of "DAI". Return the commanders in alphabetical order to
        // combine battles where the attacker and defender flipped.
        |> Seq.choose (fun battle ->
            let commander1 = leaderMap.TryFind battle.Attacker.Commander
            let commander2 = leaderMap.TryFind battle.Defender.Commander
            match (commander1, commander2) with
            | (Some (x, country1), Some (y, country2)) -> 
                if x.Name < y.Name then
                     Some { commander1 = x; country1 = country1;
                            commander2 = y; country2 = country2;
                            side1 = battle.Attacker; side2 = battle.Defender;
                            won = battle.Result }
                else Some { commander1 = y; country1 = country2;
                            commander2 = x; country2 = country1;
                            side1 = battle.Defender; side2 = battle.Attacker;
                            won = not battle.Result }
            | _ -> None)
        |> Seq.groupBy (fun x -> (x.commander1.Name, x.commander2.Name))
        |> Seq.map (fun ((c1name, c2name), battles) -> 
            let com1Wins =  battles |> Seq.where (fun x -> x.won) |> Seq.length
            let com1bs = battles |> Seq.map (fun x -> x.side1)
            let com2bs = battles |> Seq.map (fun x -> x.side2)
            let c1, country1 = Seq.head battles |> fun x -> (x.commander1, x.country1)
            let c2, country2 = Seq.head battles |> fun x -> (x.commander2, x.country2)

            { description = sprintf "%s(%d/%d/%d/%d) and %s(%d/%d/%d/%d) of %s and %s"
                                    c1.Name c1.Fire c1.Shock c1.Manuever c1.Siege
                                    c2.Name c2.Fire c2.Shock c2.Manuever c2.Siege
                                    country1.DisplayName country2.DisplayName
              battles = Seq.length battles;
              forces1 = com1bs |> Seq.sumBy forces;
              losses1 = com1bs |> Seq.sumBy (fun x -> x.Losses);
              wins1 = com1Wins; wins2 = Seq.length battles - com1Wins;
              forces2 = com2bs |> Seq.sumBy forces; 
              losses2 = com2bs |> Seq.sumBy (fun x -> x.Losses); })
        |> Seq.sortBy (fun rivalry -> (~-) rivalry.battles)

    member x.CountryTradeReport () =
        nullToEmpty save.Trade
        |> Seq.collect (fun x -> x.Powers)
        |> Seq.groupBy (fun x -> x.Country)
        |> Seq.map (fun (abbr, col) ->
            { country = countryMap.[abbr].DisplayName
              lightShips = col |> Seq.sumBy (fun x -> x.LightShip)
              shipPower = col |> Seq.sumBy (fun x -> x.ShipPower)
              power = col |> Seq.sumBy (fun x -> x.Current)
              provincePower = col |> Seq.sumBy (fun x -> x.ProvincePower)
              money = col |> Seq.sumBy (fun x -> x.Money)
              total = col |> Seq.sumBy (fun x -> x.Total)
            })
        |> Seq.sortBy (fun x -> (~-) x.power)

    /// Returns the five number summaries of the correlations between
    /// the ledger information stored in the save file
    member x.LedgerCorrelations () =
        let q = query {
            for nationSize in nullToEmpty save.NationSizeStatistics do
            join income in nullToEmpty save.IncomeStatistics on
                (nationSize.Name = income.Name)
            join score in nullToEmpty save.ScoreStatistics on
                (nationSize.Name = score.Name)
            join inflation in nullToEmpty save.InflationStatistics on
                (nationSize.Name = inflation.Name)
            where (Seq.last nationSize.YData > 0)

            select { Country = nationSize.Name
                     NationSize = nationSize.YData
                     Income = income.YData
                     Score = score.YData
                     Inflation = inflation.YData }
        }

        let cast (lis:seq<int>) : seq<float> = lis |> Seq.map(fun x -> float x) 

        let correlate f1 f2 =
            // We must make sure that the difference between the max value and
            // the min value in a list is non-zero else the variance will be
            // zero and there will be dividing by zero error. So filter it to
            // only non-zero difference lists
            q
            |> Seq.where(fun x -> Seq.min (f1(x)) <> Seq.max (f1(x)))
            |> Seq.where(fun x -> Seq.min (f2(x)) <> Seq.max (f2(x)))
            |> toSummary (fun x -> Correlation.Pearson(cast(f1(x)), cast(f2(x))))

        { 
            SizeAndIncome = correlate (fun x -> x.NationSize) (fun x -> x.Income);
            SizeAndScore = correlate (fun x -> x.NationSize) (fun x -> x.Score);
            SizeAndInflation = correlate (fun x -> x.NationSize) (fun x -> x.Inflation);
            IncomeAndScore = correlate (fun x -> x.Income) (fun x -> x.Score);
            IncomeAndInflation = correlate (fun x -> x.Income) (fun x -> x.Inflation);
            ScoreAndInflation = correlate (fun x -> x.Score) (fun x -> x.Inflation);
        }

    /// Calculates the desire for various AI countries to want something
    member x.HighestAiPriorities (fn:Ai -> seq<AiProvincePriority>) =
        save.Countries
        |> Seq.where (fun x -> x.NumOfCities > 0)
        |> Seq.collect (fun x -> fn x.Ai)
        |> Seq.groupBy (fun x -> x.Id)
        |> Seq.map (fun (key, grp) ->
            let summation = 
                grp
                |> Seq.map (fun x -> x.Value)
                |> Seq.fold (+) 0
            (key, summation))

    /// Creates a five number summary on countries in a tech group
    member x.TechSummary (fn:Technology -> byte) =
        nullToEmpty save.Countries
        |> Seq.where (fun x -> x.NumOfCities > 0)
        |> Seq.groupBy (fun x -> x.TechnologyGroup)
        |> Seq.map (fun (key, grp) ->
            let summary =
                grp
                |> toSummary (fun x -> float (fn x.Technology))
            (key, summary))

    member x.ScoreRankings () =
        nullToEmpty save.Countries
        |> Seq.where (fun x -> notNull x.ScoreRating && x.ScoreRating.Count = 3)
        |> Seq.sortBy (fun x -> (~-) x.Score)
        |> Seq.mapi (fun ind x ->
            { rank = ind + 1; name = countryMap.[x.Abbreviation].DisplayName;
              score = x.Score;
              adm = x.ScoreRating.[0]; dip = x.ScoreRating.[1];
              mil = x.ScoreRating.[2] })

    member x.CountryDebts () =
        nullToEmpty save.Countries
        |> Seq.map (fun x ->
            (countryMap.[x.Abbreviation].DisplayName, x.Loans.Count,
             x.Loans |> Seq.sumBy (fun y -> y.Amount)))
        |> Seq.sortBy (fun (_,_,amount) -> (~-) amount)

    member x.IsPlayer country = 
        match countryMap.TryFind country with
        | Some(x) -> x.WasPlayer.GetValueOrDefault()
        | None -> false

    member x.PlayerStats () =
        if isNull save.Countries then Seq.empty else

        let buildings = x.CountryBuildings ()
        let playerCountries =
            save.Countries
            |> Seq.where (fun x -> x.WasPlayer.GetValueOrDefault())

        let names =
            match isNull save.PlayersCountries with
            | true -> [((Seq.exactlyOne playerCountries).DisplayName, "human")] 
                      |> List.toSeq
            | false -> 
                save.PlayersCountries
                |> Seq.pairwise
                |> Seq.mapi (fun i x -> i % 2 = 0, x)
                |> Seq.where fst
                |> Seq.map snd
                |> Seq.map (fun (name,country) -> (country,name))

        seq {
            for country in playerCountries do
                let builds =
                    buildings 
                    |> Seq.find (fun (c:Country,_) -> 
                        c.Abbreviation = country.Abbreviation)
                    |> snd

                let player = 
                    Seq.find (fun (c,_) -> c = country.Abbreviation) names |> snd

                yield { name = country.DisplayName;
                        player = player;
                        treasury = country.Treasury;
                        stability = country.Stability;
                        inflation = country.Inflation;
                        prestige = country.Prestige;
                        cities = country.NumOfCities;
                        colonies = country.NumOfColonies;
                        armyTradition = country.ArmyTradition;
                        navyTradition = country.NavyTradition;
                        mercantilism = country.Mercantilism;
                        cultures = Seq.append [country.PrimaryCulture]
                            country.AcceptedCultures;
                        warExhaustion = country.WarExhaustion;
                        manpower = country.Manpower;
                        maxManpower = country.MaxManpower;
                        government = country.Government;
                        religion = country.Religion;
                        colonists = Seq.length country.Colonists;
                        missionaries = Seq.length country.Missionaries;
                        merchants = Seq.length country.Merchants;
                        diplomats = Seq.length country.Diplomats;
                        buildings = builds }
        }

    member x.CountryBuildings () =
        let deps = seq {
            for prov in save.Provinces do
                let buildingHistory = seq {
                    for evt in nullToEmpty prov.History do
                        match evt with 
                        | :? BuildingChange as b -> yield b
                        | _ -> ()                
                }

                let beginningBuildings =
                    buildingHistory
                    |> Seq.where (fun x -> x.EventDate = DateTime.MinValue)
                    |> Seq.map (fun x -> x.Building)
                    |> List.ofSeq

                yield! buildingHistory
                |> Seq.where (fun x -> x.EventDate <> DateTime.MinValue)
                |> Seq.map (fun x -> x.Building)
                |> Seq.fold (fun (s, deps) elem -> 
                    ((elem, deps)::s, elem::deps)) ([], beginningBuildings)
                |> fun (s, deps) ->
                    s |> Seq.map fst |> Set.ofSeq
                      |> Set.difference (Set.ofSeq prov.Buildings)
                      |> Seq.map (fun x -> (x, deps |> Seq.where ((<>) x)
                                                    |> List.ofSeq))
                      |> Seq.append s
        }

        let depMap = 
            deps
            |> Seq.groupBy fst
            |> Seq.map (fun (building, deps) ->
                (building, deps |> Seq.map (snd >> Set.ofSeq) |> Set.intersectMany))
            |> Map.ofSeq

        let allBuildings =
            deps
            |> Seq.collect(fun (building, d) -> seq { yield building; yield! d })
            |> Set.ofSeq

        // Group provinces by owner and further group by building and count
        // Buildings not found in the country but found in other countries
        // have a count of zero
        let countryBuildings = 
            nullToEmpty save.Provinces
            |> Seq.groupBy (fun x -> x.Owner)
            |> Seq.map (fun (country, provs) ->
                let hasBuildings =
                    provs
                    |> Seq.collect (fun x -> Seq.distinct (seq {
                            for b in x.Buildings do
                                yield b
                                yield! depMap.[b]
                        }))
                    |> Seq.groupBy id
                    |> Seq.map (fun (b, g) -> (b, Seq.length g))
                let buildings = 
                    hasBuildings |> Seq.map fst |> Set.ofSeq
                    |> Set.difference allBuildings
                    |> Set.map (fun x -> (x, 0))
                (country, buildings |> Seq.append hasBuildings))

        // Merge abbreviated country name with the actual country
        let merge = 
            seq {
                for (country, buildings) in countryBuildings do
                    match countryMap.TryFind country with
                    | Some x -> yield (x, buildings)
                    | None -> ()
            } |> Array.ofSeq

        let buildingCount = (Seq.head save.Countries).NumOfBuildings.Count

        // returns a sequence of buildings in the order that they are suppose to
        // appear. If a building appears nowhere in the game, it is omitted from
        // the list. There may be instances where buildings can't be reduced and
        // this is somewhat of a common occurrence (think of buildings where
        // there can only be one of them (glorious_monument, tax_assessor,
        // etc)). In this instance the ambiguous buildings will be returned in
        // an arbitrary sequence
        let bindicies =
            seq {
                for i in 0 .. buildingCount - 1 do
                    yield! merge
                    |> Seq.map (fun (country, buildings) ->
                        buildings
                        |> Seq.where (fun (_, count) -> 
                            count = country.NumOfBuildings.[i])
                        |> Seq.map fst
                        |> Set.ofSeq
                        |> fun x -> if Set.isEmpty x then allBuildings else x)
                    |> Set.intersectMany
            } |> Array.ofSeq

        merge
        |> Seq.map (fun (country, buildings) ->
            (country, buildings
                      |> Seq.where (fst >> notNull)
                      |> Seq.sortBy (fun (b, _) -> 
                          Seq.findIndex ((=) b) bindicies)))
