using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiotStatsService_FunctionApp
{
    public partial class KdaModel
    {
        public string puuid { get; set; }
        public string name { get; set; }
        public double kills { get; set; }
        public double deaths { get; set; }
        public double assists { get; set; }
    }

    public partial class KdaTotalsModel
    {
        public double totalKills { get; set; }
        public double totalDeaths { get; set; }
        public double totalAssists { get; set; }
        public double averageKDA { get; set; }
    }
}
