using ApimTestApi.Entities;
using ApimTestApi.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ApimTestApi.ServiceValidators
{
    public class SqlValidator : ISqlValidatorRepository
    {
        private readonly IConfiguration _config;

        public SqlValidator(IConfiguration config)
        {
            _config = config;
        }

        public async Task<ResponseEntity> ValidateAsync()
        {
            try
            {
                var connString = _config.GetValue<string>("DatabaseConnectionString");
                using (var connection = new SqlConnection(connString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SELECT @@VERSION AS 'SQL Server Version'", connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            var response = "";
                            while (reader.Read())
                            {
                                response += $"{reader.GetString(0)}";
                            }
                            return new ResponseEntity { Status = nameof(ResponseStatus.OK), Message = $"Successful Response From SQL {DateTime.UtcNow.ToLongTimeString()}" };
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return new ResponseEntity { Status = nameof(ResponseStatus.FAIL), Message = e.Message };
            }
        }
    }
}