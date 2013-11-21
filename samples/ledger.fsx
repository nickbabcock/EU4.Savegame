#r @"..\EU4.Savegame\bin\Pdoxcl2Sharp.dll"
#r @"..\EU4.Savegame\bin\EU4.Savegame.dll"
open EU4.Savegame
let savefile = @"H:\Tuesday_Ottoman.eu4"
let save = new Savegame(savefile)

// Calculates and prints out the top 10 greatest increases in recorded history
// across all countries. This means that countries that can appear multiple times
// in the list. The algorithm works by only looking at successive years of data
// for each country where the y-values are not zero. This cuts down
// on the countries that had their increase skyrocket due to country formation
let greatest_yearly_increase (data:seq<LedgerData>) =
    data
    |> Seq.map (fun d ->
        seq { for (y1, y2) in Seq.zip d.XData d.YData |> Seq.pairwise do yield (d.Name, y1, y2) }
        |> Seq.filter (fun (_, (_,y1), (_,y2)) -> y1 <> 0 && y2 <> 0))
    |> Seq.map (fun s -> Seq.map(fun (name, (x1, y1), (x2, y2)) -> (name, x1, y2 - y1)) s)
    |> Seq.collect(fun s -> s)
    |> Seq.sortBy(fun (_,_,diff) -> (~-)(diff))
    |> Seq.take 10
    |> Seq.iter(fun (name, x1, diff) -> printfn "%s %d %d" name x1 diff)

printfn "Greatest Annual Income Increase"
printfn "--------------------"
greatest_yearly_increase save.IncomeStatistics

printfn ""
printfn "Greatest Annual Inflation Increase"
printfn "--------------------"
greatest_yearly_increase save.InflationStatistics

printfn ""
printfn "Greatest Annual Nation Size Increase"
printfn "--------------------"
greatest_yearly_increase save.NationSizeStatistics

printfn ""
printfn "Greatest Annual Score Increase"
printfn "--------------------"
greatest_yearly_increase save.ScoreStatistics