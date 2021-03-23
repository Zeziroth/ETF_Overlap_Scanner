using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace ETF_Overlap_Scanner
{
    public abstract class ETF
    {
        protected abstract string ISIN_FIELD
        {
            get;
        }

        protected abstract string NAME_FIELD
        {
            get;
        }

        protected abstract string WEIGHT_FIELD
        {
            get;
        }

        private List<string> Headlines { get; set; }
        private List<string> Body { get; set; }
        private Dictionary<string, List<string>> FieldLists { get; set; } = new Dictionary<string, List<string>>();

        public List<Holding> Holdings { get; private set; } = new List<Holding>();

        protected virtual void PrepareParams(string isin)
        {

        }

        protected void ParseParams(List<string> headlines, List<string> body, params string[] fields)
        {
            this.Headlines = headlines;
            this.Body = body;

            foreach (string field in fields)
            {
                if (!this.FieldLists.ContainsKey(field))
                {
                    this.FieldLists.Add(field, GetValuesOfColumn(field));
                }
            }

            this.SetHoldings();
        }

        private List<string> GetValuesOfColumn(string column)
        {
            List<string> ret = new List<string>();

            if (this.Headlines.Contains(column))
            {
                int col = this.Headlines.IndexOf(column);
                foreach (string values in this.Body)
                {
                    if (values.Trim() != "")
                    {
                        ret.Add(values.Split(';')[col]);
                    }
                }
            }

            return ret;
        }

        public List<string> GetIsinList()
        {
            return this.FieldLists[ISIN_FIELD];
        }

        public List<string> GetNameList()
        {
            return this.FieldLists[NAME_FIELD];
        }

        public List<string> GetWeightList()
        {
            return this.FieldLists[WEIGHT_FIELD];
        }

        public void SetHoldings(bool weightPercentage = false)
        {
            this.Holdings.Clear();

            List<string> isins = this.GetIsinList();
            List<string> names = this.GetNameList();
            List<string> weights = this.GetWeightList();

            for (int i = 0; i < isins.Count; i++)
            {
                string isin = isins[i];
                Regex r = new Regex(@"\b([A-Z]{2})((?![A-Z]{10}\b)[A-Z0-9]{10})\b");
                if (r.IsMatch(isin))
                {
                    this.Holdings.Add(new Holding(isins[i], names[i], weights[i], weightPercentage));
                }
            }
        }
    }
}
