namespace EU4.Stats.Test

open System.Collections.Generic
open EU4.Stats
open EU4.Savegame
open NUnit.Framework
open FsUnit

[<TestFixture>]
type CountryBuildingTests () =
    [<Test>]
    member x.CountryBuildingBaseCase () =
        let country = new Country("MEE")
        country.NumOfBuildings <- new List<int>([1;2])

        let prov1 = new Province(1)
        prov1.Owner <- "MEE"
        prov1.Buildings <- new List<string>(["building1"])

        let prov2 = new Province(2)
        prov2.Owner <- "MEE"
        prov2.Buildings <-
            new List<string>(["building2";"building1"])

        let countryCollection = new CountryCollection()
        countryCollection.Add(country)

        let provinceCollection = new ProvinceCollection()
        provinceCollection.Add(prov1)
        provinceCollection.Add(prov2)

        let save = new Save()
        save.Provinces <- provinceCollection
        save.Countries <- countryCollection

        let stats = SaveStats save
        let actual = stats.CountryBuildings ()
        let expected = [("building2", 1);("building1", 2)]

        Seq.length actual |> should equal 1
        CollectionAssert.AreEqual(expected, snd (Seq.head actual))

    [<Test>]
    member x.DoubleCountryBuilding () =
        let country = new Country("MEE")
        country.NumOfBuildings <- new List<int>([1;2])

        let country2 = new Country("YOU")
        country2.NumOfBuildings <- new List<int>([1;0])

        let prov1 = new Province(1)
        prov1.Owner <- "MEE"
        prov1.Buildings <- new List<string>(["armory"])

        let prov2 = new Province(2)
        prov2.Owner <- "YOU"
        prov2.Buildings <- new List<string>(["teepee"])

        let prov3 = new Province(3)
        prov3.Owner <- "MEE"
        prov3.Buildings <- new List<string>(["teepee";"armory"])

        let countryCollection = new CountryCollection()
        countryCollection.Add(country)
        countryCollection.Add(country2)

        let provinceCollection = new ProvinceCollection()
        provinceCollection.Add(prov1)
        provinceCollection.Add(prov2)
        provinceCollection.Add(prov3)

        let save = new Save()
        save.Provinces <- provinceCollection
        save.Countries <- countryCollection

        let stats = SaveStats save
        let actual = stats.CountryBuildings ()
        let meExpected = [("teepee", 1);("armory", 2)]
        let youExpected = [("teepee", 1);("armory", 0)]

        Seq.length actual |> should equal 2
        let meBuildings = snd (Seq.head actual)
        let youBuildings = snd (Seq.nth 1 actual)
        CollectionAssert.AreEqual(meExpected, meBuildings)
        CollectionAssert.AreEqual(youExpected, youBuildings)