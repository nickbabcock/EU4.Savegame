module EU4.Stats.CountryExtensions

open EU4.Savegame
open EU4.Stats.Types

type Country with
   member x.DisplayName =
      if isNull x.Name then x.Abbreviation else x.Name