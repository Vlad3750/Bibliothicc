using Bibliothicc.Services;
using System.Configuration;
using System.Data;
using System.Net.Http;
using System.Windows;

namespace Bibliothicc
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ILibraryService Service { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var client = new HttpClient { BaseAddress = new Uri("http://bibliothicc.duckdns.org:8000/") };
            Service = new LibraryServiceRest(client);
        }
    }

}
