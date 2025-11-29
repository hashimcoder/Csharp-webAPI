using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        // This is a placeholder for student-related actions.
        // You can add methods here to handle student data.
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            string [] students =  new string[] { "Alice", "Bob", "Charlie" };
            return Ok(students);
        }
    }
}