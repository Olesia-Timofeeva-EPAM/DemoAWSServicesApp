using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using DAL.Contracts;
using Infrastructure;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace LibraryApp
{
	public class Startup
	{
		private const string ApiVersion = "v1";
		private const string ApiName = "Bidding API";
		private const string SwaggerSectionKey = "Swagger";

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();
			services.AddSingleton<IDataAccessProvider, DataAccessDynamoDBProvider>();
			services.AddSingleton<ISQSProvider, SQSProvider>();

			services.AddSwaggerGen(x =>
			{
				x.SwaggerDoc(ApiVersion, new OpenApiInfo { Title = ApiName, Version = ApiVersion });

				
			});
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
			app.UseRouting();

			app.UseSwagger();

			app.UseSwaggerUI(x =>
			{
				x.RoutePrefix = string.Empty;

				x.SwaggerEndpoint($"/swagger/{ApiVersion}/swagger.json", $"{ApiName} {ApiVersion}");

			});
		}
	}
}
