namespace UniversityTasksDbFirstApi.DTOs;

public class StudentInfoDto
{
    public int StudentId { get; set; }
    public string IndexNumber { get; set; } = null!;
    public string FullName { get; set; } = null!;
}
