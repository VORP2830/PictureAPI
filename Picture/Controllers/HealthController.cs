using Microsoft.AspNetCore.Mvc;

namespace Picture.Controllers;

[ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
       [HttpGet]
        public ActionResult<string> Get()
        {
            try
            {
                return Ok("Acessado em :" + DateTime.Now.ToLongDateString());
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        } 
    }