namespace UniversityTasksDbFirstApi.DTOs;

public class AssignmentInfoDto
{
    public int AssignmentId { get; set; }
    public string Title { get; set; } = null!;
    public int MaxPoints { get; set; }
}
