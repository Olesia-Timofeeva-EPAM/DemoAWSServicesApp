using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.S3;
using Amazon.SQS;
using DAL;
using DAL.Contracts;
using Infrastructure;
using Infrastructure.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DI
{
	public static class DemoConfiguration
	{

		public static void ConfigureDependencies(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDefaultAWSOptions(configuration.GetAWSOptions());
			services.AddAWSService<IAmazonDynamoDB>();
			services.AddAWSService<IAmazonS3>();
			services.AddAWSService<IAmazonSQS>();
			services.AddSingleton<IDynamoDBContext, DynamoDBContext>();

			services.AddScoped<IDataAccessProvider, DataAccessDynamoDBProvider>();
			services.AddSingleton<ISQSProvider, SQSProvider>();
		}
	}
}
