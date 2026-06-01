namespace UniversityTasksDbFirstApi.DTOs;

public class StudentDashboardDto
{
    public int StudentId { get; set; }
    public string IndexNumber { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public bool IsActive { get; set; }
    public List<EnrollmentSummaryDto> Enrollments { get; set; } = new();
    public List<SubmissionSummaryDto> Submissions { get; set; } = new();
}
