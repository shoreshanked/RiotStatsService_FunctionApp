
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using QuickChart;
using RiotStatsService_FunctionApp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RiotStatsService_FunctionApp
{
    public class ChartBuilder
    {

        public static List<double> kdaChartList1 = new List<double>();
        public static List<double> kdaChartList2 = new List<double>();
        public static List<double> kdaChartList3 = new List<double>();
        public static List<double> kdaChartList4 = new List<double>();
        public static List<double> kdaChartList5 = new List<double>();
        public static List<double> kdaChartList6 = new List<double>();
        public static string URL = "";

        public static string InitChartData(Dictionary<string, List<double>> chartData, Dictionary<string, KdaTotalsModel> kdaResultsDictionary, ILogger log)
        {
            foreach (var t in kdaResultsDictionary)
            {
                //if the key doesnt exist then add the key and value to the chartData dictionary
                switch (t.Key)
                {
                    case "Rick n Two Crows":
                        log.LogInformation("Adding Rick n Two Crows to chartData");
                        kdaChartList1.Add(t.Value.averageKDA);
                        if (!chartData.ContainsKey(t.Key)) { chartData.Add(t.Key, kdaChartList1); }
                        else { chartData[t.Key] = kdaChartList1; }
                        break;
                    case "The Master Queef":
                        log.LogInformation("Adding The Master Queef to chartData");
                        kdaChartList2.Add(t.Value.averageKDA);
                        if (!chartData.ContainsKey(t.Key)) { chartData.Add(t.Key, kdaChartList2); }
                        else { chartData[t.Key] = kdaChartList2; }
                        break;
                    case "Up the Ashe":
                        log.LogInformation("Adding Up the Ashe to chartData");
                        kdaChartList3.Add(t.Value.averageKDA);
                        if (!chartData.ContainsKey(t.Key)) { chartData.Add(t.Key, kdaChartList3); }
                        else { chartData[t.Key] = kdaChartList3; }
                        break;
                    case "The Meshsiah":
                        log.LogInformation("Adding The Meshsiah to chartData");
                        kdaChartList4.Add(t.Value.averageKDA);
                        if (!chartData.ContainsKey(t.Key)) { chartData.Add(t.Key, kdaChartList4); }
                        else { chartData[t.Key] = kdaChartList4; }
                        break;
                    case "The Rum Ham":
                        log.LogInformation("Adding The Rum Ham to chartData");
                        kdaChartList5.Add(t.Value.averageKDA);
                        if (!chartData.ContainsKey(t.Key)) { chartData.Add(t.Key, kdaChartList5); }
                        else { chartData[t.Key] = kdaChartList5; }
                        break;
                    case "Ninjahobo":
                        log.LogInformation("Adding Ninjahobo to chartData");
                        kdaChartList6.Add(t.Value.averageKDA);
                        if (!chartData.ContainsKey(t.Key)) { chartData.Add(t.Key, kdaChartList6); }
                        else { chartData[t.Key] = kdaChartList6; }
                        break;
                }
            }
            log.LogInformation("Calling BuildChart");
            BuildChart(chartData, log);

            log.LogInformation("Returning URL");
            return URL;
        }

        public static void BuildChart(Dictionary<string, List<double>> chartData, ILogger log)
        {
            Chart qc = new Chart();

            qc.Width = 1920;
            qc.Height = 1080;

            List<string> labelList = new List<string>();
            for (int count = 0; count < kdaChartList1.Count(); count++)
            {
                string label = string.Format("Week {0}", (count + 1).ToString());
                labelList.Add(label);
            }

            Class_qc_config qc_config = new Class_qc_config() { type = "line" };
            Data qc_data = new Data() { };
            List<Dataset> qc_datasets = new List<Dataset>(){ };

            log.LogInformation("Looping through chartData");
            foreach (var c in chartData)
            {
                Dataset qc_dataset = new Dataset() { };
                qc_dataset.label = c.Key;
                qc_dataset.data = c.Value.ToArray();
                qc_dataset.lineTension = 0.2;
                qc_dataset.borderWidth = 2;
                qc_datasets.Add(qc_dataset);
            }

            qc_data.labels = labelList.ToArray();
            
            qc_data.datasets = qc_datasets.ToArray();
            qc_config.data = qc_data;

            Options qc_options = new Options();
            Ticks qc_ticks = new Ticks();
            Title qc_title = new Title();
            Scales qc_scales = new Scales();
            YAxis qc_yaxis = new YAxis();

            qc_options.responsive = true;

            qc_title.display = true;
            qc_title.text = "Test Title - does this work?";

            qc_ticks.suggestedMin = 5;
            qc_ticks.suggestedMax = 3;
            //qc_ticks.beginAtZero = false;
            
            qc_yaxis.ticks = qc_ticks;

            List<YAxis> qc_YAxisList = new List<YAxis>() { };
            qc_YAxisList.Add(qc_yaxis);
            qc_scales.yAxes = qc_YAxisList;

            qc_options.title = qc_title;
            qc_options.scales = qc_scales;

            qc_config.options = qc_options;

            log.LogInformation("Serializing qc_config");

            qc.Config = JsonSerializer.Serialize(qc_config);
            
            // Get the URL
            log.LogInformation("Calling GetURL Method");
            URL = (qc.GetUrl());
        }
    }
}
