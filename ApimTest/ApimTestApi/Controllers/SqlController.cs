using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ApimTestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SqlController : ControllerBase
    {
        private readonly IConfiguration _config;
        public SqlController(IConfiguration config)
        {
            _config = config;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<string> Get()
        {
            try
            {
                var connString = _config.GetValue<string>("DatabaseConnectionString");
                using (var connection = new SqlConnection(connString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SELECT * FROM [dbo].[Test]", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            var response = "";
                            while (reader.Read())
                            {
                                response += $"{reader.GetString(0)}";
                            }
                            return response;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest($"{e.ToString()}");
            }
        }
    }
}
