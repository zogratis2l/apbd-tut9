using Microsoft.AspNetCore.Mvc;
using UniversityTasksDbFirstApi.Exceptions;
using UniversityTasksDbFirstApi.Services;

namespace UniversityTasksDbFirstApi.Controllers;

[ApiController]
[Route("api/courses")]
public class CoursesController : ControllerBase
{
    private readonly IDbService _dbService;

    public CoursesController(IDbService dbService)
    {
        _dbService = dbService;
    }

    
    [HttpGet]
    public async Task<IActionResult> GetCourses([FromQuery] bool activeOnly = false)
    {
        var courses = await _dbService.GetCoursesAsync(activeOnly);
        return Ok(courses);
    }

   
    [HttpGet("{idCourse}/assignments")]
    public async Task<IActionResult> GetCourseAssignments(int idCourse, [FromQuery] bool publishedOnly = false)
    {
        try
        {
            var assignments = await _dbService.GetCourseAssignmentsAsync(idCourse, publishedOnly);
            return Ok(assignments);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
