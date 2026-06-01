namespace UniversityTasksDbFirstApi.DTOs;

public class SubmissionDto
{
    public int SubmissionId { get; set; }
    public StudentInfoDto Student { get; set; } = null!;
    public AssignmentInfoDto Assignment { get; set; } = null!;
    public string RepositoryUrl { get; set; } = null!;
    public DateTime SubmittedAt { get; set; }
    public string Status { get; set; } = null!;
    public int? Score { get; set; }
    public string? Feedback { get; set; }
}
