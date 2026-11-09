using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers
{
    // https://localhost:portnumber/api/students
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            var studentNames = new string[]
            {
                "John",
                "Jane",
                "Mark",
                "Emily",
                "David"
            };
            
            return Ok(studentNames);
        }
    }
}