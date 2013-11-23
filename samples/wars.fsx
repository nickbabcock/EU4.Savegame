#r @"..\EU4.Savegame\bin\Pdoxcl2Sharp.dll"
#r @"..\EU4.Savegame\bin\EU4.Savegame.dll"
open EU4.Savegame
let savefile = Seq.last fsi.CommandLineArgs
let save = new Savegame(savefile)

let wars : seq<War> = 
    Seq.append (Seq.cast save.ActiveWars) (Seq.cast save.PreviousWars)

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
