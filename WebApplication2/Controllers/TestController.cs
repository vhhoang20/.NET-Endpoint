using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("call-project1-api")]
        public IActionResult CallProject1Api()
        {
            // Replace with the actual URL of Project 1's API endpoint
            string project1ApiUrl = "https://project1api.com/api/endpoint";

            WebRequest request = WebRequest.Create(project1ApiUrl);
            WebResponse response = request.GetResponse();

            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string responseBody = reader.ReadToEnd();
                return Ok(responseBody);
            }
        }
    }
}
