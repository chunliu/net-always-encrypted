# net-always-encrypted
The code in this repo demonstrates how to use SQL Always Encrypted and Azure Key Vault with ASP.NET MVC and Entity Framework. 

The code in the following folders:

- netcore - .NET 5 + EF Core + Microsoft.Data.SqlClient.AlwaysEncrypted.AzureKeyVaultProvider
- netfx - .NET Framework 4.8 + EF6 + Microsoft.SqlServer.Management.AlwaysEncrypted.AzureKeyVaultProvider

Microsoft.Data.SqlClient.AlwaysEncrypted.AzureKeyVaultProvider cannot be used for EF6 since Microsoft.Data.SqlClient is not compatible with EF6. dotnet/SqlClient#725
