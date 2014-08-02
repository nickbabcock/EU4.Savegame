namespace EU4.Stats.Test

open System.Collections.Generic
open EU4.Stats
open EU4.Savegame
open NUnit.Framework
open FsUnit

[<TestFixture>]
type ``Given LedgerData`` () as x =
    let xdata = seq { 1 .. 5 }
    [<DefaultValue>] val mutable data : seq<LedgerData>

    do
        let mutable l1 = new LedgerData()
        l1.Name <- "MEE"
        l1.XData <- new List<int>(xdata)
        l1.YData <- new List<int>([5;6;3;10;11])

        let mutable l2 = new LedgerData()
        l2.Name <- "YOU"
        l2.XData <- new List<int>(xdata)
        l2.YData <- new List<int>(seq { for i in 1 .. 5 -> 2 * i })

        let mutable l3 = new LedgerData()
        l3.Name <- "THM"
        l3.XData <- new List<int>(xdata)
        l3.YData <- new List<int>([1;6;5;6;7])

        x.data <- [l1;l2;l3]

    [<Test>] 
    member x.``Rankings are calculated correctly`` () =
        let actual = LedgerStats.LedgerRankings x.data
        let expected =
            [("MEE", 1, 1);("MEE", 2, 1);("MEE", 3, 3);
            ("MEE", 4, 1);("MEE", 5, 1);
            ("YOU", 1, 2);("YOU", 2, 3);("YOU", 3, 1);
            ("YOU", 4, 2);("YOU", 5, 2);
            ("THM", 1, 3);("THM", 2, 1);("THM", 3, 2);
            ("THM", 4, 3);("THM", 5, 3)]

        CollectionAssert.AreEquivalent(expected, actual)

    [<Test>]
    member x.``Years leading ledger are calculated correctly`` () =
        let actual = LedgerStats.YearsLeadingLedger x.data
        let expected = [("MEE", 4); ("YOU", 1); ("THM", 1)]
        CollectionAssert.AreEquivalent(expected, actual)

    [<Test>]
    member x.``Relative rank change is calcualted correctly`` () =
        let actual = LedgerStats.RelativeRankChange x.data None
        let expected = 
            [("MEE", 1, 0);("MEE", 2, 2);("MEE", 3, -2);("MEE", 4, 0);
            ("YOU", 1, 1);("YOU", 2, -2);("YOU", 3, 1);("YOU", 4, 0);
            ("THM", 1, -2);("THM", 2, 1);("THM", 3, 1);("THM", 4, 0)]

        CollectionAssert.AreEquivalent(expected, actual)