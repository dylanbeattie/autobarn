using Autobarn.Data;
using Autobarn.Website.GraphQL.Schemas;
using Autobarn.Website.Hubs;
using EasyNetQ;
using GraphiQl;
using GraphQL.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Autobarn.Website {
	public class Startup {

		const string AMQP = "amqps://uzvpuvak:2S_t9clRGI16R8Kx6MbaYdUKc0XXGq7p@rattlesnake.rmq.cloudamqp.com/uzvpuvak";

		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services) {
			services.AddRouting(options => options.LowercaseUrls = true);
			services.AddControllersWithViews();
			var db = new InMemoryCarDatabase("JsonData");
			services.AddSingleton<ICarDatabase>(db);

			var bus = RabbitHutch.CreateBus(AMQP);
			services.AddSingleton<IBus>(bus);

			services.AddSingleton<AutobarnSchema>();
			services
				.AddGraphQL(options => options.EnableMetrics = false)
				.AddSystemTextJson();

			services.AddSignalR();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			} else {
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			// app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.UseGraphQL<AutobarnSchema>();
			app.UseGraphiQl("/graphiql");

			app.UseEndpoints(endpoints => {
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
					endpoints.MapHub<NewCarHub>("/newcarhub");
			});

		}
	}
}
