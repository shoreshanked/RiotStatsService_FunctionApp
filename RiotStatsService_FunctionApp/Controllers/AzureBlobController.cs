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
        public static string path = "C:\\Storage\\vars.json";
        public static string storageConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

        public static BlobServiceClient blobServiceClient = new BlobServiceClient(storageConnectionString);
        public static Highscores LoadFromStorage(ILogger log)
        {
            log.LogInformation("Retreiving container from blob storage");
            var blobContainerClient = GetContainer(blobServiceClient, containerName);

            log.LogInformation("Calling GetBlobClient method and assigning to variable");
            var blobClient = blobContainerClient.GetBlobClient("C:/Storage/vars.json");

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
            var doesStorageExist = GetContainer(blobServiceClient, containerName);
            
            if (doesStorageExist.Name == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static async void UpdateCreateStorage(Highscores updatedHighscores, bool storageExists, ILogger log)
        {
            if (storageExists)
            {
                log.LogInformation("Storage has been found, updating storage via method override");
                StoreVars(updatedHighscores, log);
            }
            else
            {
                log.LogInformation("Storage has not been found, creating new storage");
                await CreateContainerAsync(blobServiceClient, containerName);
                StoreVars(log);
            }
        }


        public static void StoreVars(ILogger log)
        {
            var _path = path.Replace("\\vars.json", "");
            var files = Directory.GetFiles(_path, "*", SearchOption.AllDirectories);
            BlobContainerClient containerClient = new BlobContainerClient(storageConnectionString, containerName);

            foreach (var file in files)
            {
                var filePathOverCloud = file.Replace(path, "").Replace("\\", "");
                using (MemoryStream stream = new MemoryStream(File.ReadAllBytes(file)))
                {
                    log.LogInformation("Creating first blob");
                    containerClient.UploadBlob(file, stream);
                }
            }

        }

        public static void StoreVars(Highscores updatedHighscores, ILogger log)
        {
            var updatedJson = JsonSerializer.Serialize(updatedHighscores);
            var ts = new FileStream(path, FileMode.Truncate, FileAccess.Write);
            ts.Close();
            var fs = new FileStream(path, FileMode.Append, FileAccess.Write);
            var sw = new StreamWriter(fs);
            sw.WriteLine(updatedJson);
            sw.Flush();
            sw.Close();
            fs.Close();

            var _path = path.Replace("\\vars.json", "");
            var files = Directory.GetFiles(_path, "*", SearchOption.AllDirectories);
            BlobContainerClient containerClient = new BlobContainerClient(storageConnectionString, containerName);

            foreach (var file in files)
            {
                var filePathOverCloud = file.Replace(path, "").Replace("\\", "");
                using (MemoryStream stream = new MemoryStream(File.ReadAllBytes(file)))
                {
                    log.LogInformation("Deleting original blob");
                    containerClient.DeleteBlob(file);
                    log.LogInformation("Creating new blob with updates");
                    containerClient.UploadBlob(file, stream);
                }
            }

        }
        public static BlobContainerClient GetContainer(BlobServiceClient blobServiceClient, string containerName)
        {
            try
            {
                BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
                
                if (blobContainerClient.Exists())
                {
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
