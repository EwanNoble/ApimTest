using ApimTestApi.Entities;
using ApimTestApi.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;

namespace ApimTestApi.ServiceValidators
{
    public class CosmosValidator : ICosmosValidatorRepository
    {
        private readonly IConfiguration _config;

        public CosmosValidator(IConfiguration config)
        {
            _config = config;
        }

        public async Task<ResponseEntity> ValidateAsync()
        {
            try
            {
                var EndpointUri = _config.GetValue<string>("CosmosDbEndpointUri");
                var PrimaryKey = _config.GetValue<string>("CosmosDbPrimaryKey");
                using(var client = new DocumentClient(new Uri(EndpointUri), PrimaryKey))
                {
                    var account = await client.GetDatabaseAccountAsync();
                    var resourceId = account.ResourceId;
                } 

                return new ResponseEntity { Status = nameof(ResponseStatus.OK), Message = $"Successful Response From CosmosDb {DateTime.UtcNow.ToLongTimeString()}" };
            }
            catch (Exception e)
            {

                return new ResponseEntity { Status = nameof(ResponseStatus.FAIL), Message = e.Message };
            }
        }
    }
}