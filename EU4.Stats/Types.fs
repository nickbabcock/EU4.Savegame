module EU4.Stats.Types

open System;
open MathNet.Numerics.Statistics;

type FiveNumberSummary = {
    min : double; q1 : double; median : double; q3 : double; max : double;
}

let ArrayToSummary (arr : double array) : FiveNumberSummary =
    { min = arr.[0]; q1 = arr.[1]; median = arr.[2]; q3 = arr.[3]; max = arr.[4] }

let toSummary (fn:'a -> double) = 
    Seq.map fn >> Statistics.FiveNumberSummary >> ArrayToSummary
let toSummary' (fn:'a -> Nullable<double>) = 
    Seq.map fn >> Seq.map (fun x -> x.GetValueOrDefault()) >>
    Statistics.FiveNumberSummary >> ArrayToSummary

let inline isNull o = Object.ReferenceEquals(o, null)
let inline notNull o = not (isNull o)

let printSummary (data:seq<string * FiveNumberSummary>) (header:string) =
    let max = data |> Seq.map (fun (x,_) -> x.Length) |> Seq.max
    let headerFormat = Printf.TextWriterFormat<string -> string -> string -> string -> string -> string -> unit>(
                        "%" + string max + "s %5s %5s %5s %5s %5s")
    let itemFormat = Printf.TextWriterFormat<string -> float -> float -> float -> float -> float -> unit>(
                        "%" + string max + "s %.02f %.02f %.02f %.02f %.02f")
    printfn headerFormat header "min" "q1" "median" "q3" "max"
    data |> Seq.iter (fun (x,a) -> printfn itemFormat x a.min a.q1 a.median a.q3 a.max)