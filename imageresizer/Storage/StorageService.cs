using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;

namespace imageresizer.Storage
{
    public class StorageService
    {
        private readonly CloudBlobClient _client;
        private readonly IConfiguration _configuration;
        private readonly ILogger<StorageService> _logger;
        public StorageService(ILogger<StorageService> logger, CloudBlobClient client, IConfiguration configuration)
        {
            _logger = logger;
            _client = client;
            _configuration = configuration;
        }
        public async Task<(bool, CloudBlockBlob)> TryGetFile(string name)
        {
            return await TryGetFile(_configuration["StorageAccount:Container"], name);
        }

        public async Task<(bool, CloudBlockBlob)> TryGetFileCached(string name)
        {
            return await TryGetFile(_configuration["StorageAccount:ContainerResized"], name);
        }

        private async Task<(bool, CloudBlockBlob)> TryGetFile(string containerName, string name)
        {
            try
            {
                var blob = GetFile(containerName, name);

                if(await blob.ExistsAsync())
                    return (true, blob);
            }
            catch(Exception error)
            {
                _logger.LogError(error, $"Não foi possível fazer o download do arquivo {name}");
            }
            
            return (false, null);
        }

        public async Task<(bool, CloudBlockBlob)> TryUploadToCache(string name, byte[] imageContent, string mimetype)
        {
            try
            {
                var cacheBlob = GetFile(_configuration["StorageAccount:ContainerResized"], name);
                await cacheBlob.UploadFromByteArrayAsync(imageContent, 0, imageContent.Length);

                cacheBlob.Properties.ContentType = mimetype;
                await cacheBlob.SetPropertiesAsync();

                return (true, cacheBlob);
            }
            catch(Exception error)
            {
                _logger.LogError(error, $"Não foi possível fazer o uoload do arquivo {name}");
            }

            return (false, null);
        }
        private CloudBlockBlob GetFile(string containerName, string blobName)
        {
            var container = _client.GetContainerReference(containerName);
            return container.GetBlockBlobReference(blobName);
        }

        public async Task<byte[]> GetBlobBytes(CloudBlockBlob blob)
        {
            using(var memory = new MemoryStream())
            {
                await blob.DownloadToStreamAsync(memory);
                return memory.ToArray();
            }
        }
    }
}