using System.Collections.Generic;

namespace ETF_Overlap_Scanner
{
    public class ISharesResults
    {
        public List<ISharesResult> results { get; set; }
    }
    public class ISharesResult
    {
        public string label { get; set; }
        public string id { get; set; }
        public string category { get; set; }
    }
}
