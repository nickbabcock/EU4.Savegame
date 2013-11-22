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
