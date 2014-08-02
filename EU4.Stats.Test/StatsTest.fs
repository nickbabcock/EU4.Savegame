namespace EU4.Stats.Test

open EU4.Stats
open EU4.Savegame
open NUnit.Framework
open FsUnit

[<TestFixture>]
type ``Given a Save`` ()  as x =
    [<DefaultValue>] val mutable stats : SaveStats
    do
        let mutable save = new Save()
        let mutable c1 = new Country("MEE")
        c1.Manpower <- 1.2
        c1.MaxManpower <- 1.2
        c1.NumOfCities <- 1

        let mutable c2 = new Country("YOU")
        c2.Manpower <- 1.3
        c2.MaxManpower <- 2.2
        c2.NumOfCities <- 1
        save.Countries <- new CountryCollection()
        save.Countries.Add(c1)
        save.Countries.Add(c2)

        x.stats <- SaveStats save

    [<Test>] member x.
        ``World Manpower is Summed Correctly`` () =
            x.stats.WorldManpower() |> should equal 2500

    [<Test>] member x.
        ``Max World Manpower is Summed Correctly`` () =
            x.stats.MaxWorldManpower() |> should equal 3400
