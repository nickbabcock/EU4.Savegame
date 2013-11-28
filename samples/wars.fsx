#r @"..\EU4.Savegame\bin\Pdoxcl2Sharp.dll"
#r @"..\EU4.Savegame\bin\EU4.Savegame.dll"
open EU4.Savegame
let savefile = Seq.last fsi.CommandLineArgs
let save = new Savegame(savefile)

// Analyze all wars (those that ocurred in the past and the ones ocurring now).
// Since some wars have long names, shorten them.
let wars : seq<War> = 
    Seq.append (Seq.cast save.ActiveWars) (Seq.cast save.PreviousWars)
    |> Seq.map(fun (x:War) -> 
               if x.Name.Length > 40 then x.Name <- x.Name.[0..39] + "..."
               x)

let ConcatWithReverse data =
    data
    |> Seq.toList
    |> List.rev
    |> Seq.zip data

let TopLevelAttribute attr (wars:seq<War>) =
    wars
    |> Seq.groupBy (fun war -> attr war)
    |> Seq.map (fun (name, group) -> (name, Seq.length group))
    |> Seq.sortBy (fun x -> (~-) (snd x))

printfn "Country   Wars Started  |  Country   Wars Started Against"
Seq.zip (TopLevelAttribute (fun x -> x.OriginalAttacker) wars)
        (TopLevelAttribute (fun x -> x.OriginalDefender) wars) 
|> Seq.take 10
|> Seq.iter (fun ((n1, s1), (n2, s2)) -> printfn "%-9s %-12d  |  %-9s %d" n1 s1 n2 s2)

printfn ""
printfn ""

printfn "---- Wars with the Most Stalled Years ----"
wars
|> Seq.sortBy (fun x -> (~-) x.StalledYears)
|> Seq.take 10
|> Seq.iter (fun x -> printfn "%-43s %d" x.Name x.StalledYears)

// To calculate the losses in a war, add defender and attacker losses for all
// battles
let warLosses (w:War) = 
    w.GetBattles()
    |> Seq.fold (fun acc elem ->
                 acc + elem.Attacker.Losses + elem.Defender.Losses) 0

let startDate (w:War) =
    w.History.Events
    |> Seq.map (fun (d, es) -> d)
    |> Seq.min

printfn ""
printfn ""
printfn "---- Wars with the Most Losses ----"
printfn "%-43s %11s %12s %s" "War Name" "Losses" "Start Date" "Opponents"
wars
|> Seq.sortBy (fun w -> warLosses w |> (~-))
|> Seq.take 10
|> Seq.iter (fun war ->
             printfn "%-43s %11s %s %s vs %s"
                      war.Name ((warLosses war).ToString("n0"))
                      ((startDate war).ToString("MMM dd, yyyy"))
                      war.OriginalAttacker war.OriginalDefender)
