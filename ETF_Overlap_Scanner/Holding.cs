using System;

namespace ETF_Overlap_Scanner
{
    public class Holding
    {
        public string ISIN { get; private set; }
        public string Name { get; private set; }
        public decimal Weight { get; private set; }

        public Holding(string isin, string name, string weight, bool weightPercent = false)
        {
            this.ISIN = isin;
            this.Name = name;
            this.Weight = decimal.Parse(weight.Replace(".", ","));

            if (weightPercent)
            {
                this.Weight *= 100;
            }

            this.Weight = Math.Round(this.Weight, 2);
        }
    }
}
