using System;
using imageresizer.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace imageresizer.Configuration
{
    public static class StorageConfiguration
    {
        public static void ConfigureStorage(this IServiceCollection services, IConfiguration configuration, IHostingEnvironment env)
        {
            services.AddSingleton(factory => CreateStorageAccount(configuration, env));
            services.AddSingleton(CreateStorageClient);
            services.AddSingleton<StorageService>();
        }

        private static CloudStorageAccount CreateStorageAccount(IConfiguration configuration, IHostingEnvironment env)
        {
            return CloudStorageAccount.Parse(configuration["StorageAccount:ConnectionString"]);
            // var credentials = new StorageCredentials(
            //     configuration["StorageAccount:Account"],
            //     configuration["StorageAccount:Key"]
            // );

            // var useHttps = env.IsDevelopment() == false;
            // //var useHttps = configuration.GetValue<bool>("StorageAccount:UseHttps");

            // return new CloudStorageAccount(credentials, useHttps);
        }

        private static CloudBlobClient CreateStorageClient(IServiceProvider factory)
        {
            var account = factory.GetService<CloudStorageAccount>();
            return account.CreateCloudBlobClient(); 
        }
    }
}