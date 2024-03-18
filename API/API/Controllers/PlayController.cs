using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PlayController : ControllerBase
    {
        [HttpGet("get-player")]
        public IActionResult Players() 
        {
            return Ok(new JsonResult(new { message = "Only authorized users can see this message!" }));
        }
    }
}
