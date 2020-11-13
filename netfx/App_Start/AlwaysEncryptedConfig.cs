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
        private static string clientId = ConfigurationManager.AppSettings["spClientId"];
        private static string clientSecret = ConfigurationManager.AppSettings["spClientSecret"];

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
            var authContext = new AuthenticationContext(authority);
            ClientCredential clientCred = new ClientCredential(clientId, clientSecret);
            AuthenticationResult result = await authContext.AcquireTokenAsync(resource, clientCred);
            if (result == null)
                throw new Exception("GetToken failed");

            return result.AccessToken;
        }
    }
}