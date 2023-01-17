using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Console;
using Microsoft.SharePoint.Client;
using System.Security;

namespace SharePointApp
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


            AzureAD azureAD = new AzureAD();

            configuration.GetSection("AzureAd").Bind(azureAD);


            Uri site = new Uri(azureAD.Site);
            string user = azureAD.Username;

            SecureString password = GetSecureString($"Password for {user}");
            Console.WriteLine(); // after password

            using (var authenticationManager = new AuthenticationManager(azureAD.TenantID, azureAD.ClientID))

            using (var context = authenticationManager.GetContext(site, user, password))
            {
                context.Load(context.Web, p => p.Title);
                await context.ExecuteQueryAsync();
                Console.WriteLine($"Title: {context.Web.Title}");
            }
            Console.WriteLine("Press the Any Key to End");
            Console.ReadLine();

        }

        private static SecureString GetSecureString(string promptUser)
        {
            Console.WriteLine(promptUser);
            SecureString securePassword = new SecureString();

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (securePassword.Length > 0)
                    {
                        securePassword.RemoveAt(securePassword.Length - 1);
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    securePassword.AppendChar(key.KeyChar);
                    Console.Write("*");
                }

            }
            
            securePassword.MakeReadOnly();
            return securePassword;
            
        }
    }
}