using Microsoft.AspNetCore.Mvc;

namespace imageresizer.Controllers
{
    public class StatusController : ControllerBase
    {
        [HttpGet("api/status")]   
        public IActionResult Get()
        {
            return Ok("On");
        }
    }
}