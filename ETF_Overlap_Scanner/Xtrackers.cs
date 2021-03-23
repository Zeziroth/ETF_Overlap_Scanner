using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Service = ServiceStack.Text;

namespace ETF_Overlap_Scanner
{
    public class Xtrackers : ETF
    {
        protected override string ISIN_FIELD
        {
            get;
        } = "Constituent ISIN";
        protected override string NAME_FIELD
        {
            get;
        } = "Constituent Name";
        protected override string WEIGHT_FIELD
        {
            get;
        } = "Constituent Weighting";
        protected override void PrepareParams(string isin)
        {
            string res = new WebClient().DownloadString(String.Format("https://etf.dws.com/etfdata/export/DEU/DEU/csv/product/constituent/{0}", isin));
            var csv = Service.CsvSerializer.DeserializeFromString<List<string>>(res);
            List<string> headlines = csv[0].Replace(",", ";").Split(';').ToList<string>();
            csv.RemoveAt(0);

            this.ParseParams(headlines, csv, ISIN_FIELD, NAME_FIELD, WEIGHT_FIELD);
            this.SetHoldings(true);
        }


        public Xtrackers(string isin)
        {
            this.PrepareParams(isin);
        }
    }
}
