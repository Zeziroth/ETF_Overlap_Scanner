using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Service = ServiceStack.Text;

namespace ETF_Overlap_Scanner
{
    public class IShares : ETF
    {
        protected override string ISIN_FIELD
        {
            get;
        } = "ISIN";
        protected override string NAME_FIELD
        {
            get;
        } = "Name";

        protected override string WEIGHT_FIELD
        {
            get;
        } = "Gewichtung (%)";

        protected override void PrepareParams(string isin)
        {

            string preRes = "";
            dynamic preJson;

            again:
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(String.Format("https://www.ishares.com/de/privatanleger/de/search/Suchergebnisse?switchLocale=y&siteEntryPassthrough=true&searchText={0}&doTickerSearch=true", isin));

                request.CookieContainer = new CookieContainer();
                request.Timeout = 3000;
                using (WebResponse myHttpWebResponse = request.GetResponse())
                {
                }
                var request2 = (HttpWebRequest)WebRequest.Create(String.Format("https://www.ishares.com/de/privatanleger/de/autoComplete.search?type=autocomplete&term={0}", isin));
                request2.CookieContainer = request.CookieContainer;
                request2.Timeout = 3000;
                using (WebResponse response = request2.GetResponse())
                {
                    preRes = new StreamReader(response.GetResponseStream()).ReadToEnd();
                }
                preJson = JsonConvert.DeserializeObject<dynamic>(preRes);
            }
            catch
            {
                goto again;
            }
            
            string label = preJson[0].label;
            string id = preJson[0].id;

            string res = new WebClient().DownloadString(String.Format("{0}{1}/1478358465952.ajax?fileType=csv&fileName=test&dataType=fund", id, label)).Replace("\",\"", ";");
            res = res.Replace("\"", "");
            var csv = Service.CsvSerializer.DeserializeFromString<List<string>>(res);
            csv.RemoveAt(0);
            csv.RemoveAt(0);
            List<string> headlines = csv[0].Replace(",", ";").Split(';').ToList<string>();
            csv.RemoveAt(0);

            this.ParseParams(headlines, csv, ISIN_FIELD, NAME_FIELD, WEIGHT_FIELD);
        }

        public IShares(string isin)
        {
            this.PrepareParams(isin);
        }
    }
}
