#r @"C:\Projects\EUIVSavegame\EU4.Stats\bin\Pdoxcl2Sharp.dll"
#r @"C:\Projects\EUIVSavegame\EU4.Stats\bin\EU4.Savegame.dll"
#r @"C:\Projects\EUIVSavegame\EU4.Stats\bin\MathNet.Numerics.dll"
#r @"C:\Projects\EUIVSavegame\EU4.Stats\bin\EU4.Stats.dll"

#load "LedgerStats.fs"
#load "CountryStats.fs"
#load "Types.fs"
open EU4.Savegame
open EU4.Stats
open EU4.Stats.Types

let savefile = Seq.last fsi.CommandLineArgs
let save = new Save(savefile)
let correlations = LedgerStats.correlations(save)

printfn "Correlate %f" correlations.IncomeAndInflation.median