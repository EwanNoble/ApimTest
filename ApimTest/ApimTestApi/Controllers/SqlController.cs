using Microsoft.AspNetCore.Mvc;
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
        // GET api/values
        [HttpGet]
        public ActionResult<string> Get()
        {
            try
            {
                var connString = ConfigurationManager.AppSettings["DatabaseConnectionString"];
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
                                response += $"{reader.GetString(0)} {reader.GetString(1)}";
                            }
                            return response;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return $"{e.ToString()}";
            }
        }
    }
}
