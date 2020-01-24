using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace OcelotAPIGateway
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services
				.AddMvc()
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

			services
				.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
				.AddIdentityServerAuthentication("TestKey", opt =>
				{
					opt.Authority = "http://localhost:6000";
					opt.ApiName = "SampleService";
					opt.SupportedTokens = SupportedTokens.Both;
					opt.RequireHttpsMetadata = false;
				});


			services
				.AddOcelot()
				.AddCacheManager(x => x.WithDictionaryHandle());

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseMvc();
			app.UseOcelot().Wait();


		}
	}
}
