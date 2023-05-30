using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiotStatsService_FunctionApp
{
    public partial class MatchOverviewModel
    {
        public string matchID { get; set; }
        public int teamID { get; set; }
        public bool winGame { get; set; }
        public int totalGoldEarned { get; set; }
        public int totalGoldSpent { get; set; }
        public int totalKills { get; set; }
        public int totalDeaths { get; set; }
        public int totalAssists { get; set; }
        public int totalHealing { get; set; }
        public bool myTeam { get; set; }

    }
}
