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

namespace RiotStatsService_FunctionApp
{
    class AzureBlobController
    {

        public static string containerName = Environment.GetEnvironmentVariable("ContainerName");
        public static string path = "\\Storage\\vars.json";
        public static string storageConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

        public static BlobServiceClient blobServiceClient = new BlobServiceClient(storageConnectionString);
        public static BlobContainerClient doesStorageExist = new BlobContainerClient(storageConnectionString, containerName);

        public static Highscores LoadFromStorage(ILogger log)
        {
            log.LogInformation("Retreiving container from blob storage");
            var blobContainerClient = GetContainer(blobServiceClient, containerName, log);

            log.LogInformation("Calling GetBlobClient method and assigning to variable");
            var blobClient = blobContainerClient.GetBlobClient("/vars.json");

            log.LogInformation("Downloading blob content to a stream");
            var blobContent = blobClient.DownloadContent().Value.Content;
            
            Highscores highscores = new Highscores();
            log.LogInformation("Deserializing blob content to Highscores object");
            highscores = JsonSerializer.Deserialize<Highscores>(blobContent);

            log.LogInformation("Returning Highscores object");
            return highscores;
        }
        
        public static bool CheckStorageExists(ILogger log)
        {
            //Does a previous container exist
            log.LogInformation("Retreiving container from blob storage to see if it exists");

            try
            {
                doesStorageExist = GetContainer(blobServiceClient, containerName, log);
            }
            catch(Exception e)
            {
                log.LogError("Exception thrown: " + e.Message);
            }
            
            if (doesStorageExist == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static void UpdateCreateStorage(Highscores updatedHighscores, bool storageExists, ILogger log)
        {
            if (storageExists)
            {
                log.LogInformation("Storage has been found, updating storage via method override");
                StoreVars(updatedHighscores, log, true);
                log.LogError("updatecreate storage WOOOOOP");
            }
            else
            {
                log.LogInformation("Storage has not been found, creating new storage");
                CreateContainerAsync(blobServiceClient, containerName);
                StoreVars(updatedHighscores, log, false);
            }
        }

        public static void StoreVars(Highscores updatedHighscores, ILogger log, bool deleteOldBlob)
        {
            log.LogInformation("Creating container");
            var blobContainerClient = GetContainer(blobServiceClient, containerName, log);

            log.LogInformation("Creating blob client");
            var blobClient = blobContainerClient.GetBlobClient("/vars.json");

            log.LogInformation("Creating blob content");
            var blobContent = JsonSerializer.Serialize(updatedHighscores);

            log.LogInformation("Uploading blob content");
            if (deleteOldBlob)
            {
                blobClient.Delete();
            }
            blobClient.Upload(new MemoryStream(Encoding.UTF8.GetBytes(blobContent)));
        }

        public static BlobContainerClient GetContainer(BlobServiceClient blobServiceClient, string containerName, ILogger log)
        {
            try
            {
                log.LogInformation("Calling GetBlobContainerClient method and assigning to variable");
                BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);

                if (blobContainerClient.Exists())
                {
                    log.LogInformation("blob exists, return blob: " + blobContainerClient.Name);
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
