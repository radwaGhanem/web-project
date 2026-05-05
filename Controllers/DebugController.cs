using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DebugController : ControllerBase
    {
        private readonly ApplicationDB _dbContext;

        public DebugController(ApplicationDB dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("db-connection")]
        [AllowAnonymous]
        public ActionResult<string> CheckDbConnection()
        {
            try
            {
                var canConnect = _dbContext.Database.CanConnect();
                var message = canConnect ? "Database connection is OK." : "Database connection failed.";
                return Ok(message);
            }
            catch (System.Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }
    }
}
