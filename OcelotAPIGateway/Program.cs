using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;

namespace OcelotAPIGateway
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateWebHostBuilder(args).Build().Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
			.ConfigureAppConfiguration((host, config) =>
			{
				config
				.SetBasePath(host.HostingEnvironment.ContentRootPath)
				.AddJsonFile("appsettings.json", true, true)
				.AddJsonFile($"appsettings.{host.HostingEnvironment.EnvironmentName}.json", true, true)
				.AddOcelot("Configuration", host.HostingEnvironment)
				.AddEnvironmentVariables();
			})
			.UseStartup<Startup>();
	}
}
