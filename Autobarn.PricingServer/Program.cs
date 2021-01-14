using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;

using System.Net;


namespace Autobarn.PricingServer {
	public class Program {
		public static void Main(string[] args) {
			CreateHostBuilder(args).Build().Run();
		}

		// Additional configuration is required to successfully run gRPC on macOS.
		// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder => {
					webBuilder.ConfigureKestrel(options => {
						var pfxPassword = Environment.GetEnvironmentVariable("UrsatilePfxPassword");
						var https = UseCertIfAvailable(@"d:\workshop.ursatile.com\certificate.pfx", pfxPassword);
						options.ListenAnyIP(5002, listenOptions => listenOptions.Protocols = HttpProtocols.Http1AndHttp2);
						options.Listen(IPAddress.Any, 5003, https);
					});
					webBuilder.UseStartup<Startup>();
				});


		private static Action<ListenOptions> UseCertIfAvailable(string pfxFilePath, string pfxPassword) {
			if (File.Exists(pfxFilePath)) return listen => listen.UseHttps(pfxFilePath, pfxPassword);
			return listen => listen.UseHttps();
		}
	}
}



