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
        public TestController(ISqlValidatorRepository _sqlValidator, IStorageValidatorRepository _storageValidator)
        {
            sqlValidator = _sqlValidator;
            storageValidator = _storageValidator;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {

            try
            {                
                var sqlResponse = await sqlValidator.ValidateAsync();
                var storageResponse = await storageValidator.ValidateAsync();

                var response = new JObject(
                                new JProperty("Sql", new JObject(
                                    new JProperty("status", sqlResponse.Status),
                                    new JProperty("message", sqlResponse.Message)
                                )),
                                                                new JProperty("Storage", new JObject(
                                    new JProperty("status", storageResponse.Status),
                                    new JProperty("message", storageResponse.Message)
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
