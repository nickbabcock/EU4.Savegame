module Types

open MathNet.Numerics.Statistics;

type FiveNumberSummary = {
    min : double; q1 : double; median : double; q3 : double; max : double;
}

let ArrayToSummary (arr : double array) : FiveNumberSummary =
    { min = arr.[0]; q1 = arr.[1]; median = arr.[2]; q3 = arr.[3]; max = arr.[4] }

let summarize = (Statistics.FiveNumberSummary : seq<float> -> float array) 
                >> ArrayToSummary