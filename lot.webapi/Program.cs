using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Text;

namespace LottoLion.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var provider = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(provider);

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://*:5127")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}