#r @"C:\Projects\EUIVSavegame\EU4.Stats\bin\Debug\Pdoxcl2Sharp.dll"
#r @"C:\Projects\EUIVSavegame\EU4.Stats\bin\Debug\EU4.Savegame.dll"
#r @"C:\Projects\EUIVSavegame\EU4.Stats\bin\Debug\MathNet.Numerics.dll"
#r @"C:\Projects\EUIVSavegame\EU4.Stats\bin\Debug\EU4.Stats.dll"

#load "LedgerStats.fs"
open EU4.Savegame

let savefile = Seq.last fsi.CommandLineArgs
let save = new Savegame(savefile)
let correlations = LedgerStats.correlations(save)

printfn "Correlate %f" correlations.IncomeAndInflation.median