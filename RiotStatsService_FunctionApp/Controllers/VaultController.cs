using System;
using System.Threading.Tasks;
using VaultSharp.V1.AuthMethods.UserPass;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.Commons;
using VaultSharp;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs;

namespace RiotStatsService_FunctionApp
{
    class VaultController
    {   
        public static async Task<string> ReturnVaultSecret(ILogger log)
        {
            var user = Environment.GetEnvironmentVariable("VaultUser");
            var pass = Environment.GetEnvironmentVariable("VaultPass");

            IAuthMethodInfo authMethod = new UserPassAuthMethodInfo(user, pass);
            VaultClientSettings vaultClientSettings = new VaultClientSettings("https://vault-public-vault-6d778307.f278edbc.z1.hashicorp.cloud:8200", authMethod);
            vaultClientSettings.Namespace = "admin";

      
            IVaultClient vaultClient2 = new VaultClient(vaultClientSettings);
            
            
            log.LogInformation("Retrieving Riot API Key from vault");
            try
            {
                Secret<SecretData> kv2Secret = await vaultClient2.V1.Secrets.KeyValue.V2.ReadSecretAsync(path: "riotAPI", mountPoint: "kv");
                return kv2Secret.Data.Data.First().Value.ToString();
            }
            catch (Exception e)
            {
                log.LogError("Error retrieving API key: {0}", e);
                return null;
            }  
        }
    }
}
