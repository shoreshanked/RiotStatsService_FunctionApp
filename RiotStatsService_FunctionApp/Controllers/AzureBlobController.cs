using Azure.Storage.Blobs;
using Azure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using System.ComponentModel;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using System.Reflection.Metadata;
using Azure.Storage.Blobs.Specialized;
using static System.Net.WebRequestMethods;
using File = System.IO.File;
using Azure.Storage.Blobs.Models;

namespace RiotStatsService_FunctionApp
{
    class AzureBlobController
    {

        public static string containerName = Environment.GetEnvironmentVariable("ContainerName");
        public static string path = "\\Storage\\vars.json";
        public static string storageConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

        public static BlobServiceClient blobServiceClient = new BlobServiceClient(storageConnectionString);
        public static BlobContainerClient doesContainerStorageExist = new BlobContainerClient(storageConnectionString, containerName);
        public static Pageable<BlobItem> blobs;

        public static BinaryData LoadFromStorage(ILogger log, string blobName)
        {
            log.LogInformation("Retreiving container from blob storage");
            var blobContainerClient = GetContainer(blobServiceClient, containerName, log);

            log.LogInformation("Calling GetBlobClient method and assigning to variable");
            var blobClient = blobContainerClient.GetBlobClient(blobName);

            log.LogInformation("Downloading blob content to a stream");
            var blobContent = blobClient.DownloadContent().Value.Content;
 
            log.LogInformation("Returning blobContent object");
            return blobContent;
        }
        
        public static bool CheckContainerStorageExists(ILogger log)
        {
            //Does a previous container exist
            log.LogInformation("Retreiving container from blob storage to see if it exists");

            try
            {
                doesContainerStorageExist = GetContainer(blobServiceClient, containerName, log);
            }
            catch(Exception e)
            {
                log.LogError("Exception thrown: " + e.Message);
            }

            if (doesContainerStorageExist == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool CheckBlobExists(ILogger log, string blobName)
        {
            //Does a previous container exist
            log.LogInformation("Retreiving blob from container to see if it exists");
            bool doesBlobExist = false;
            try
            {
                blobs = doesContainerStorageExist.GetBlobs();
            }
            catch (Exception e)
            {
                log.LogError("Exception thrown: " + e.Message);
            }

            if (blobs == null)
            {
                doesBlobExist = false;
            }
            else
            {
                foreach (var blob in blobs)
                {
                    if (blob.Name == blobName)
                    {
                        doesBlobExist = true;
                        log.LogInformation("blob exists, return blob: " + blob.Name);
                    }
                    else
                    {
                        doesBlobExist = false;
                    }
                    if (doesBlobExist == true)
                    {
                        break;
                    }
                }
            }
            return doesBlobExist;
        }

        public static void UpdateCreateStorage(Highscores updatedHighscores, bool containerExists, bool blobExists, ILogger log)
        {
            if (containerExists)
            {
                log.LogInformation("Storage has been found, updating storage via method override");
                StoreVars(updatedHighscores, log);
                log.LogError("updatecreate storage WOOOOOP");
            }
            else
            {
                log.LogInformation("Container Storage has not been found, creating new container");
                CreateContainerAsync(blobServiceClient, containerName);
                StoreVars(updatedHighscores, log);
            }
        }

        public static void StoreVars(Highscores updatedHighscores, ILogger log)
        {
            log.LogInformation("Getting container");
            var blobContainerClient = GetContainer(blobServiceClient, containerName, log);

            log.LogInformation("Creating blob client");
            var blobClient = blobContainerClient.GetBlobClient("/vars.json");

            log.LogInformation("Creating blob content");
            var blobContent = JsonSerializer.Serialize(updatedHighscores);

            log.LogInformation("Uploading blob content");

            blobClient.Upload(new MemoryStream(Encoding.UTF8.GetBytes(blobContent)), overwrite: true);
        }

        public static void StoreChart(string chartURL, ILogger log)
        {
            log.LogInformation("Getting container");
            var blobContainerClient = GetContainer(blobServiceClient, containerName, log);

            log.LogInformation("Creating blob client");
            var blobClient = blobContainerClient.GetBlobClient("/chartURL.txt");

            log.LogInformation("Creating blob content (chartURL)");
            var blobContent = chartURL;

            log.LogInformation("Uploading blob content (chartURL)");

            blobClient.Upload(new MemoryStream(Encoding.UTF8.GetBytes(blobContent)), overwrite: true);
        }

        public static void StoreChartLists(List<List<double>> kdaMasterList, ILogger log)
        {
            log.LogInformation("Getting container");
            var blobContainerClient = GetContainer(blobServiceClient, containerName, log);

            log.LogInformation("Creating blob client");
            var blobClient = blobContainerClient.GetBlobClient("/chartListData.json");

            log.LogInformation("Creating blob content (chartListData)");
            var blobContent = JsonSerializer.Serialize(kdaMasterList);

            log.LogInformation("Uploading blob content (chartListData)");

            blobClient.Upload(new MemoryStream(Encoding.UTF8.GetBytes(blobContent)), overwrite: true);
        }
   
        public static void StoreChartData(Dictionary<string, List<double>> chartData, ILogger log)
        {
            log.LogInformation("Getting container");
            var blobContainerClient = GetContainer(blobServiceClient, containerName, log);

            log.LogInformation("Creating blob client");
            var blobClient = blobContainerClient.GetBlobClient("/chartData.json");

            log.LogInformation("Creating blob content (chartData)");
            var blobContent = JsonSerializer.Serialize(chartData);

            log.LogInformation("Uploading blob content (chartData)");

            blobClient.Upload(new MemoryStream(Encoding.UTF8.GetBytes(blobContent)), overwrite: true);
        }

        public static BlobContainerClient GetContainer(BlobServiceClient blobServiceClient, string containerName, ILogger log)
        {
            try
            {
                log.LogInformation("Calling GetBlobContainerClient method and assigning to variable");
                BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);

                if (blobContainerClient.Exists())
                {
                    log.LogInformation("container exists, return container: " + blobContainerClient.Name);
                    return blobContainerClient;
                }
            }
            catch (RequestFailedException e)
            {
                Console.WriteLine("HTTP error code {0}: {1}",
                                    e.Status, e.ErrorCode);
                Console.WriteLine(e.Message);
            }
            return null;
        }

        public static async Task<BlobContainerClient> CreateContainerAsync(BlobServiceClient blobServiceClient, string containerName)
        {
            // Name the sample container based on new GUID to ensure uniqueness.
            // The container name must be lowercase.
            try
            {
                // Create the container
                BlobContainerClient container = await blobServiceClient.CreateBlobContainerAsync(containerName);
                
                if (await container.ExistsAsync())
                {
                    Console.WriteLine("Created container {0}", container.Name);
                    return container;
                }
                else
                {
                    Console.WriteLine("Container {0} already exists", container.Name);
                    return container;
                }
            }
            catch (RequestFailedException e)
            {
                Console.WriteLine("HTTP error code {0}: {1}",
                                    e.Status, e.ErrorCode);
                Console.WriteLine(e.Message);
            }
            
            return null;
        }

    }
}
