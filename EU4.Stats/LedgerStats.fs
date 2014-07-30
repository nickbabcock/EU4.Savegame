module EU4.Stats.LedgerStats

open EU4.Stats.Types

type LedgerStat = {
    Country : string;
    NationSize : seq<int>;
    Income : seq<int>;
    Score : seq<int>;
    Inflation : seq<int>;
}

type Correlations = {
    SizeAndIncome : FiveNumberSummary;
    SizeAndScore : FiveNumberSummary;
    SizeAndInflation : FiveNumberSummary;
    IncomeAndScore : FiveNumberSummary;
    IncomeAndInflation : FiveNumberSummary;
    ScoreAndInflation : FiveNumberSummary;
}
