using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient.AlwaysEncrypted.AzureKeyVaultProvider;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Client;
using netcore.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace netcore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private static string spClientId;
        private static string spClientSecret;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            spClientId = Configuration.GetValue<string>("spClientId");
            spClientSecret = Configuration.GetValue<string>("spClientSecret");

            SqlColumnEncryptionAzureKeyVaultProvider azureKeyVaultProvider = new SqlColumnEncryptionAzureKeyVaultProvider(AADAuthenticationCallback);
            SqlConnection.RegisterColumnEncryptionKeyStoreProviders(new Dictionary<string, SqlColumnEncryptionKeyStoreProvider>
            {
                { SqlColumnEncryptionAzureKeyVaultProvider.ProviderName, azureKeyVaultProvider }
            });

            services.AddDbContext<TodoContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("TodoDBConnection"))
            );

            services.AddControllersWithViews();
        }

        private static async Task<string> AADAuthenticationCallback(string authority, string resource, string scope)
        {
            var clientApp = ConfidentialClientApplicationBuilder
                .Create(spClientId)
                .WithClientSecret(spClientSecret)
                .WithAuthority(authority)
                .Build();
            var scopes = new[] { resource + "/.default" };
            var authResult = await clientApp.AcquireTokenForClient(scopes).ExecuteAsync();
            if (authResult == null)
            {
                throw new Exception("Failed to acquire the access token.");
            }

            return authResult.AccessToken;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
