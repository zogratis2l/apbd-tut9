using UniversityTasksDbFirstApi.DTOs;

namespace UniversityTasksDbFirstApi.Services;

public interface IDbService
{
    Task<List<CourseDto>> GetCoursesAsync(bool activeOnly);
    Task<List<AssignmentDto>> GetCourseAssignmentsAsync(int courseId, bool publishedOnly);
    Task<StudentDashboardDto> GetStudentDashboardAsync(int studentId);
    Task<SubmissionDto> CreateSubmissionAsync(CreateSubmissionDto request);
    Task<SubmissionDto> GradeSubmissionAsync(int submissionId, GradeSubmissionDto request);
    Task DeleteSubmissionAsync(int submissionId);
}
