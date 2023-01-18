using ConsoleAuthenticationManager;
using Microsoft.Extensions.Configuration;
using Microsoft.SharePoint.Client;
using System.Linq;
using System.Security;

namespace GetSharePointList
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()

               .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
               .AddJsonFile("appsettings.json", optional: true)
               .AddUserSecrets<Program>();

            var configuration = builder.Build();

            // Read options from appsettings.json or usersecrets.json if defined
            AzureAD azureAD = new AzureAD();
            configuration.GetSection("AzureAd").Bind(azureAD);

            // SharePoint Site
            Uri site = new Uri(azureAD.Site);

            // User Credential Flow - This is not the only way to do this, but great for interractive console apps
            string user = azureAD.Username;
            SecureString password = AuthenticationManager.GetSecureString($"Password for {user}");
            Console.WriteLine(); // Blank Line after password ****

            using (var authenticationManager = new AuthenticationManager(azureAD.TenantID, azureAD.ClientID))
            using (var context = authenticationManager.GetContext(site, user, password))
            {
                await DoWork(azureAD, context);
            }

            Console.WriteLine("Press the Any Key to End");
            Console.ReadLine();

        }

        private static async Task DoWork(AzureAD azureAD, ClientContext context)
        {
            await DisplayWebsiteTitleAsync(context);
            await DisplayListItemsAsync(azureAD, context);
        }

        private static async Task DisplayWebsiteTitleAsync(ClientContext context)
        {
            context.Load(context.Web, p => p.Title);
            await context.ExecuteQueryAsync();

            Console.WriteLine($"Title: {context.Web.Title}");
        }

        private static async Task DisplayListItemsAsync(AzureAD azureAD, ClientContext context)
        {

            // Find List and Pattern match to strong type
            if  (context.Web.Lists.GetByTitle(azureAD.List) is List list)
            {
                // Load List
                context.Load(list, l => l.RootFolder.ServerRelativeUrl);
                await context.ExecuteQueryAsync();

                // Load Items
                CamlQuery camlQuery = CamlQuery.CreateAllItemsQuery();
                var items = list.GetItems(camlQuery);
                context.Load(items, l => l.IncludeWithDefaultProperties(i => i.Folder, i => i.File, i => i.DisplayName));
                await context.ExecuteQueryAsync();


                // Display List Summary
                var url = azureAD.List;
                Console.WriteLine($"List URL : {azureAD.Site}/Lists/{Uri.EscapeDataString(url)}");

                // Display List Items
                items
                    .Select(item => $" ID:{item["ID"]}\t {item["Title"]} ")     // Format output string
                    .ToList()                                                   // Convert to List
                    .ForEach(Console.WriteLine);                                // Apply WriteLine to each
            }
        }
    }
}