using Bibliothicc.Models;
using Bibliothicc.Services;
using System.Net.Http;
using System.Windows;

namespace Bibliothicc
{
    public partial class App : Application
    {
        public static ILibraryService Service { get; private set; }
        public static User? CurrentUser { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var client = new HttpClient { BaseAddress = new Uri("http://bibliothicc.duckdns.org:8000/") };
            Service = new LibraryServiceRest(client);
        }
    }
}
