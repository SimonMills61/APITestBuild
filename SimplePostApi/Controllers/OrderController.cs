using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace SimplePostApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IConfiguration _config;

        public OrderController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("current")]public async Task<IActionResult> GetCurrentCounter()
{
    var connString = _config.GetConnectionString("AzureSql");

    using (var conn = new SqlConnection(connString))
    {
        await conn.OpenAsync();

        using (var cmd = new SqlCommand("SELECT CounterValue FROM MMOrderCounter", conn))
        {
            var result = await cmd.ExecuteScalarAsync();
            return Ok(new { currentValue = result });
        }
    }
}



        [HttpPost("next")]
        public async Task<IActionResult> GetNextOrderNumber()
        {
            var connectionString = _config.GetConnectionString("AzureSql");

            string nextOrderNumber;

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("dbo.GetNextMMOrderNumber", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // If your SP uses an OUTPUT parameter:
                // var outputParam = new SqlParameter("@NextOrderNumber", SqlDbType.VarChar, 50)
                // {
                //     Direction = ParameterDirection.Output
                // };
                // cmd.Parameters.Add(outputParam);

                await conn.OpenAsync();

                // If your SP returns a single value via SELECT:
                var result = await cmd.ExecuteScalarAsync();
                nextOrderNumber = result?.ToString() ?? string.Empty;

                // If using OUTPUT parameter instead:
                // nextOrderNumber = outputParam.Value?.ToString() ?? string.Empty;
            }

            var response = new OrderNumberResponse
            {
                OrderNumber = nextOrderNumber
            };

            return Ok(response);
        }
    }
}
