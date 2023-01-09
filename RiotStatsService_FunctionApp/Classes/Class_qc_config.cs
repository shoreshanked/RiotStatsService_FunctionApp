using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiotStatsService_FunctionApp.Classes
{
    public class Class_qc_config
    {
        public string type { get; set; }
        public Data data { get; set; }
        public Options options { get; set; }
    }

    public class Data
    {
        public string[] labels { get; set; }
        public Dataset[] datasets { get; set; }
    }

    public class Dataset
    {
        public string label { get; set; }
        public double[] data { get; set; }
        public int borderWidth { get; set; }
        public double lineTension { get; set; }
    }

    public class Options
    {
        public bool responsive { get; set; }
        public Title title { get; set; }
        public Scales scales { get; set; }
    }

    public class Scales
    {
        public List<YAxis> yAxes { get; set; }
    }

    public class Ticks
    {
        public int suggestedMin { get; set; }
        public int suggestedMax { get; set; }
        //public bool beginAtZero { get; set; }
    }

    public class Title
    {
        public bool display { get; set; }
        public string text { get; set; }
    }

    public class YAxis
    {
        public Ticks ticks { get; set; }
    }
}
