using ApimTestApi.Entities;
using ApimTestApi.Interfaces;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace ApimTestApi.ServiceValidators
{
    public class StorageValidator : IStorageValidatorRepository
    {
        private readonly IConfiguration _config;
        public StorageValidator(IConfiguration config)
        {
            _config = config;
        }

        public async Task<ResponseEntity> ValidateAsync()
        {
            try
            {
                var connString = _config.GetValue<string>("StorageConnectionString");

                CloudStorageAccount storageAccount = null;
                CloudBlobContainer cloudBlobContainer = null;

                CloudStorageAccount.TryParse(connString, out storageAccount);
                CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
                cloudBlobContainer = cloudBlobClient.GetContainerReference("networktestblobbs" + Guid.NewGuid().ToString());
                await cloudBlobContainer.CreateIfNotExistsAsync();
                await cloudBlobContainer.DeleteIfExistsAsync();

                return new ResponseEntity { Status = nameof(ResponseStatus.OK), Message = $"Successful Response From Storage {DateTime.UtcNow.ToLongTimeString()}" };
            }
            catch (Exception e)
            {
                return new ResponseEntity { Status = nameof(ResponseStatus.FAIL), Message = e.Message };
            }
        }
    }
}