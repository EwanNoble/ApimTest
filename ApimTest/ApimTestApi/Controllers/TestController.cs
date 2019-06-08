using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ApimTestApi.Interfaces;

namespace ApimTestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ISqlValidatorRepository sqlValidator;
        private readonly IStorageValidatorRepository storageValidator;
        private readonly ICosmosValidatorRepository cosmosValidator;
        public TestController(ISqlValidatorRepository _sqlValidator, IStorageValidatorRepository _storageValidator, ICosmosValidatorRepository _cosmosValidator)
        {
            sqlValidator = _sqlValidator;
            storageValidator = _storageValidator;
            cosmosValidator = _cosmosValidator;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {

            try
            {
                var sqlResponse = await sqlValidator.ValidateAsync();
                var storageResponse = await storageValidator.ValidateAsync();
                var cosmosResponse = await cosmosValidator.ValidateAsync();

                var response = new JObject(
                                new JProperty("Sql", new JObject(
                                    new JProperty("status", sqlResponse.Status),
                                    new JProperty("message", sqlResponse.Message)
                                )),
                                new JProperty("Storage", new JObject(
                                    new JProperty("status", storageResponse.Status),
                                    new JProperty("message", storageResponse.Message)
                                )),
                                new JProperty("CosmosDb", new JObject(
                                    new JProperty("status", cosmosResponse.Status),
                                    new JProperty("message", cosmosResponse.Message)
                                ))
                            );

                return response.ToString(Formatting.Indented);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
