
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

        public static string InitChartData(Dictionary<string, List<double>> chartData, Dictionary<string, KdaTotalsModel> kdaResultsDictionary)
        {
            foreach (var t in kdaResultsDictionary)
            {
                //if the key doesnt exist then add the key and value to the chartData dictionary
                switch (t.Key)
                {
                    case "Rick n Two Crows":
                        kdaChartList1.Add(t.Value.averageKDA);
                        if (!chartData.ContainsKey(t.Key)) { chartData.Add(t.Key, kdaChartList1); }
                        else { chartData[t.Key] = kdaChartList1; }
                        break;
                    case "The Master Queef":
                        kdaChartList2.Add(t.Value.averageKDA);
                        if (!chartData.ContainsKey(t.Key)) { chartData.Add(t.Key, kdaChartList2); }
                        else { chartData[t.Key] = kdaChartList2; }
                        break;
                    case "Up the Ashe":
                        kdaChartList3.Add(t.Value.averageKDA);
                        if (!chartData.ContainsKey(t.Key)) { chartData.Add(t.Key, kdaChartList3); }
                        else { chartData[t.Key] = kdaChartList3; }
                        break;
                    case "The Meshsiah":
                        kdaChartList4.Add(t.Value.averageKDA);
                        if (!chartData.ContainsKey(t.Key)) { chartData.Add(t.Key, kdaChartList4); }
                        else { chartData[t.Key] = kdaChartList4; }
                        break;
                    case "The Rum Ham":
                        kdaChartList5.Add(t.Value.averageKDA);
                        if (!chartData.ContainsKey(t.Key)) { chartData.Add(t.Key, kdaChartList5); }
                        else { chartData[t.Key] = kdaChartList5; }
                        break;
                    case "Ninjahobo":
                        kdaChartList6.Add(t.Value.averageKDA);
                        if (!chartData.ContainsKey(t.Key)) { chartData.Add(t.Key, kdaChartList6); }
                        else { chartData[t.Key] = kdaChartList6; }
                        break;
                }
            }
            BuildChart(chartData);
            return URL;
        }

        public static void BuildChart(Dictionary<string, List<double>> chartData)
        {
            Chart qc = new Chart();

            qc.Width = 500;
            qc.Height = 300;

            string[] labels = { "1", "2" };

            Class_qc_config qc_config = new Class_qc_config() { type = "line" };
            Data qc_data = new Data() { };
            List<Dataset> qc_datasets = new List<Dataset>(){ };

            foreach (var c in chartData)
            {
                Dataset qc_dataset = new Dataset() { };
                qc_dataset.label = c.Key;
                qc_dataset.data = c.Value.ToArray();
                qc_datasets.Add(qc_dataset);
            }

            qc_data.labels = labels;
            
            qc_data.datasets = qc_datasets.ToArray();
            qc_config.data = qc_data;

            qc.Config = JsonSerializer.Serialize(qc_config);

            // Get the URL
            URL = (qc.GetUrl());
        }
    }
}
