using Microsoft.AspNetCore.Mvc;
using UniversityTasksDbFirstApi.Exceptions;
using UniversityTasksDbFirstApi.Services;

namespace UniversityTasksDbFirstApi.Controllers;

[ApiController]
[Route("api/students")]
public class StudentsController : ControllerBase
{
    private readonly IDbService _dbService;

    public StudentsController(IDbService dbService)
    {
        _dbService = dbService;
    }

  
    [HttpGet("{idStudent}/dashboard")]
    public async Task<IActionResult> GetStudentDashboard(int idStudent)
    {
        try
        {
            var dashboard = await _dbService.GetStudentDashboardAsync(idStudent);
            return Ok(dashboard);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
