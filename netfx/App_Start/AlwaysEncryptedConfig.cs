using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.SqlServer.Management.AlwaysEncrypted.AzureKeyVaultProvider;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace netfx
{
    public class AlwaysEncryptedConfig
    {
        public static void InitializeAzureKeyVaultProvider()
        {
            var azKeyVaultProvider = new SqlColumnEncryptionAzureKeyVaultProvider(GetToken);
            Dictionary<string, SqlColumnEncryptionKeyStoreProvider> providers = new Dictionary<string, SqlColumnEncryptionKeyStoreProvider>
            {
                { SqlColumnEncryptionAzureKeyVaultProvider.ProviderName, azKeyVaultProvider }
            };
            SqlConnection.RegisterColumnEncryptionKeyStoreProviders(providers);
        }

        private static async Task<string> GetToken(string authority, string resource, string scope)
        {
            string clientId = ConfigurationManager.AppSettings["spClientId"];
            string clientSecret = ConfigurationManager.AppSettings["spClientSecret"];

            // Option 1: Use Microsoft.IdentityModel.Clients.ActiveDirectory
            //var authContext = new AuthenticationContext(authority);
            //ClientCredential clientCred = new ClientCredential(clientId, clientSecret);
            //AuthenticationResult result = await authContext.AcquireTokenAsync(resource, clientCred);

            // Option 2 (recommended): Use Microsoft.Identity.Client
            var clientApp = ConfidentialClientApplicationBuilder
                .Create(clientId)
                .WithClientSecret(clientSecret)
                .WithAuthority(authority)
                .Build();
            var scopes = new[] { resource + "/.default" };
            var result = await clientApp.AcquireTokenForClient(scopes).ExecuteAsync();

            if (result == null)
                throw new Exception("GetToken failed");

            return result.AccessToken;
        }
    }
}