
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using QuickChart;
using RiotStatsService_FunctionApp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static RiotStatsService_FunctionApp.AzureBlobController;

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
        public static List<List<double>> kdaMasterList = new List<List<double>>();

        public static void InitChartData(Dictionary<string, List<double>> chartData, Dictionary<string, KdaTotalsModel> kdaResultsDictionary, ILogger log)
        {
            //check storage exists already
            var containerStorageExists = AzureBlobController.CheckContainerStorageExists(log);
            var blobChartListExists = CheckBlobExists(log, "chartListData.json");
            
            if (containerStorageExists && blobChartListExists)
            {
                // if storage exists then deseralize the data and add to the player lists
                var _chartDataLists = LoadFromStorage(log, "chartListData.json");
                var chartDataLists = JsonSerializer.Deserialize<List<List<double>>>(_chartDataLists);
                kdaChartList1 = chartDataLists[0];
                kdaChartList2 = chartDataLists[1];
                kdaChartList3 = chartDataLists[2];
                kdaChartList4 = chartDataLists[3];
                kdaChartList5 = chartDataLists[4];
                kdaChartList6 = chartDataLists[5];
            }

            #region Test Data
            // TEST DATA TO TEST SAVING TO BLOB
            //kdaResultsDictionary.Clear();
            //KdaTotalsModel kdaTotalsModel1 = new KdaTotalsModel();
            //kdaTotalsModel1.averageKDA = 2.39;
            //kdaResultsDictionary.Add("Rick n Two Crows", kdaTotalsModel1);

            //KdaTotalsModel kdaTotalsModel2 = new KdaTotalsModel();
            //kdaTotalsModel2.averageKDA = 3.0;
            //kdaResultsDictionary.Add("The Master Queef", kdaTotalsModel2);

            //KdaTotalsModel kdaTotalsModel3 = new KdaTotalsModel();
            //kdaTotalsModel3.averageKDA = 3.36;
            //kdaResultsDictionary.Add("Up the Ashe", kdaTotalsModel3);

            //KdaTotalsModel kdaTotalsModel4 = new KdaTotalsModel();
            //kdaTotalsModel4.averageKDA = 3.52;
            //kdaResultsDictionary.Add("The Meshsiah", kdaTotalsModel4);

            //KdaTotalsModel kdaTotalsModel5 = new KdaTotalsModel();
            //kdaTotalsModel5.averageKDA = 3.68;
            //kdaResultsDictionary.Add("The Rum Ham", kdaTotalsModel5);

            //KdaTotalsModel kdaTotalsModel6 = new KdaTotalsModel();
            //kdaTotalsModel6.averageKDA = 3.9;
            //kdaResultsDictionary.Add("Ninjahobo", kdaTotalsModel6);
            //// TEST DATA TO TEST SAVING TO BLOB 
            #endregion

            foreach (var t in kdaResultsDictionary)
            {
                //if the key doesnt exist then add the key and value to the chartData dictionary
                switch (t.Key)
                {
                    case "Rick n Two Crows":
                        log.LogInformation("Adding Rick n Two Crows to chartData");
                        // appends the next iteration of data to the playerlist
                        kdaChartList1.Add(t.Value.averageKDA);
                        if (!chartData.ContainsKey(t.Key)) { chartData.Add(t.Key, kdaChartList1); }
                        //updates the chartData with the latest info
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
            // update the master list to be saved back into azure blob storage
            kdaMasterList.Add(kdaChartList1);
            kdaMasterList.Add(kdaChartList2);
            kdaMasterList.Add(kdaChartList3);
            kdaMasterList.Add(kdaChartList4);
            kdaMasterList.Add(kdaChartList5);
            kdaMasterList.Add(kdaChartList6);

            // upload to blob storage
            StoreChartLists(kdaMasterList, log);

            if(containerStorageExists)
            {
                //upload chartData to blob storage
                StoreChartData(chartData, log);
            }  
        }

        public static string BuildChart(Dictionary<string, List<double>> chartData, int labelCount, ILogger log)
        {
            Chart qc = new Chart();

            qc.Width = 1920;
            qc.Height = 1080;
            
            List<string> labelList = new List<string>();
            for (int count = 0; count < labelCount; count++)
            {
                string label = string.Format("Day {0}", (count + 1).ToString());
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
                qc_dataset.borderWidth = 4;
                qc_dataset.backgroundColor = "rgba(0,0,0,0)";
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
            qc_title.text = "10 Match rolling average KDA over time";

            qc_ticks.suggestedMin = 3;
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


            log.LogInformation("Calling tinyurl API");
            Uri address = new Uri("http://tinyurl.com/api-create.php?url=" + URL);
            WebClient client = new WebClient();
            string tinyUrl = client.DownloadString(address);

            return tinyUrl;
        }
    }
}
