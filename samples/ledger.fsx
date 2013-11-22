#r @"..\EU4.Savegame\bin\Pdoxcl2Sharp.dll"
#r @"..\EU4.Savegame\bin\EU4.Savegame.dll"
open EU4.Savegame
let savefile = Seq.last fsi.CommandLineArgs
let save = new Savegame(savefile)

// Calculates and prints out the top 10 greatest increases in recorded history
// across all countries. This means that countries that can appear multiple times
// in the list. The algorithm works by only looking at successive years of data
// for each country where the y-values are not zero. This cuts down
// on the countries that had their increase skyrocket due to country formation
let GreatestYearlyIncrease (data:seq<LedgerData>) =
    data
    |> Seq.map (fun d ->
        seq { for (y1, y2) in Seq.zip d.XData d.YData |> Seq.pairwise do yield (d.Name, y1, y2) }
        |> Seq.filter (fun (_, (_,y1), (_,y2)) -> y1 <> 0 && y2 <> 0))
    |> Seq.map (fun s -> Seq.map(fun (name, (x1, y1), (x2, y2)) -> (name, x1, y2 - y1)) s)
    |> Seq.collect(fun s -> s)
    |> Seq.sortBy(fun (_,_,diff) -> (~-)(diff))

let ConcatWithReverse data =
    data
    |> Seq.toList
    |> List.rev
    |> Seq.zip data

let PrintLedger title data = 
    printfn "----- Greatest Change in %s in a Year -----" title
    printfn "Greatest Increase   | Greatest Decrease"
    data
    |> Seq.take 10
    |> Seq.iter(fun ((name1, x1, diff1),(name2, x2, diff2)) ->
                printfn "%-5s %6d %6d | %-5s %6d %6d" name1 x1 diff1 name2 x2 diff2)
    printfn ""
    printfn ""

GreatestYearlyIncrease save.IncomeStatistics
|> ConcatWithReverse
|> PrintLedger "Income"

GreatestYearlyIncrease save.InflationStatistics
|> ConcatWithReverse
|> PrintLedger "Inflation"

GreatestYearlyIncrease save.NationSizeStatistics
|> ConcatWithReverse
|> PrintLedger "Nation Size"

GreatestYearlyIncrease save.ScoreStatistics
|> ConcatWithReverse
|> PrintLedger "Score"

// Function calculates the relative rank that a country stands in a certain year.
// It looks intimidating but really is a series of simple steps.
// Step 1: Create tuple that that is (country, year, value)
// Step 2: Group the set by year
// Step 3: Each group is then grouped again by y-value (countries can tie)
// Step 4: Find out how many countries are in front of each group and then
//         add one to get the rank of all the countries in current group
// Step 5: Throw away all information except for sequence of (name, date, rank)
let RelativeRank (data:seq<LedgerData>) =
    data
    |> Seq.map (fun d ->
        seq { for (x, y) in Seq.zip d.XData d.YData do yield (d.Name, x, y) }
        |> Seq.filter (fun (_, _, y) -> y <> 0))
    |> Seq.collect(fun s -> s)
    |> Seq.groupBy (fun (_, date, _) -> date)
    |> Seq.sortBy (fun (date, _) -> date)
    |> Seq.map
       (fun (date, col) ->
           (date, col
                  |> Seq.sortBy (fun (_,_,y) -> (~-) y)
                  |> Seq.groupBy (fun (_, _, y) -> y)))
    |> Seq.map
       (fun (date, col) ->
           (date, col
                  |> Seq.zip (col
                      |> Seq.map(fun (_, group) -> group |> Seq.length)
                      |> Seq.scan (+) 0)
                  |> Seq.collect(fun (i, (y, gr)) ->
                      seq { for (n, x, y) in gr do yield (n, x, i+1) })))
    |> Seq.collect snd

// Return a sequence of tuples of (name, x) where x is the number of
// years that the name was number 1 in the rankings
let YearsOfFirst data =
    data
    |> Seq.groupBy(fun (name, _, _) -> name)
    |> Seq.map (fun (name, group) ->
               (name, group
                      |> Seq.filter (fun (_,_,rank) -> rank = 1)
                      |> Seq.length))
    |> Seq.sortBy (fun (_, years) -> (~-) years)

// Creates a sequence of (name, date, rank difference) where
// the date is the year of occurrence. A negative rank difference
// is a good thing, depicting moving "up" in the ranks.
let GreatestChangeInRelativity (data:seq<string * int * int>) =
    data
    |> Seq.groupBy (fun (name, _, _) -> name)
    |> Seq.collect(fun (name, group) ->
        group
        |> Seq.sortBy (fun (_, date, _) -> date)
        |> Seq.pairwise
        |> Seq.map(fun ((name, x1, y1), (_, _, y2)) -> (name, x1, y2 - y1)))
    |> Seq.sortBy (fun (_, _, diff) -> diff)

save.IncomeStatistics
|> RelativeRank
|> GreatestChangeInRelativity
|> ConcatWithReverse
|> PrintLedger "Income Rank"

save.InflationStatistics
|> RelativeRank
|> GreatestChangeInRelativity
|> ConcatWithReverse
|> PrintLedger "Inflation Rank"

save.NationSizeStatistics
|> RelativeRank
|> GreatestChangeInRelativity
|> ConcatWithReverse
|> PrintLedger "Size Rank"

save.ScoreStatistics
|> RelativeRank
|> GreatestChangeInRelativity
|> ConcatWithReverse
|> PrintLedger "Score Rank"

let firstIncome = YearsOfFirst (RelativeRank save.IncomeStatistics)
let firstInflation = YearsOfFirst (RelativeRank save.InflationStatistics)
let firstSize = YearsOfFirst (RelativeRank save.NationSizeStatistics)
let firstScore = YearsOfFirst (RelativeRank save.ScoreStatistics)

let zip4 s1 s2 s3 s4 =
  Seq.map2 (fun (a,b)(c,d) ->a,b,c,d) (Seq.zip s1 s2)(Seq.zip s3 s4)

printfn "----- Number of Years Being Ranked First -----"
printfn "%-9s %-9s %-9s %-9s" "Income" "Inflation" "Size" "Score"
zip4 firstIncome firstInflation firstSize firstScore
|> Seq.take 10
|> Seq.iter (fun ((n1, y1), (n2, y2), (n3, y3), (n4, y4)) ->
    printfn "%-5s %3d %-5s %3d %-5s %3d %-5s %3d" n1 y1 n2 y2 n3 y3 n4 y4)
