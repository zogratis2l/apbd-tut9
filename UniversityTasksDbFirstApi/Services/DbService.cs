using Microsoft.EntityFrameworkCore;
using UniversityTasksDbFirstApi.Data;
using UniversityTasksDbFirstApi.DTOs;
using UniversityTasksDbFirstApi.Exceptions;
using UniversityTasksDbFirstApi.Models;

namespace UniversityTasksDbFirstApi.Services;

public class DbService : IDbService
{
    private readonly DatabaseContext _context;

    public DbService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<CourseDto>> GetCoursesAsync(bool activeOnly)
    {
        var query = _context.Courses.AsNoTracking();

        if (activeOnly)
            query = query.Where(c => c.IsActive);

        return await query
            .Select(c => new CourseDto
            {
                CourseId = c.CourseId,
                Code = c.Code,
                Name = c.Name,
                Credits = c.Credits,
                IsActive = c.IsActive,
                AssignmentCount = c.Assignments.Count
            })
            .ToListAsync();
    }

    public async Task<List<AssignmentDto>> GetCourseAssignmentsAsync(int courseId, bool publishedOnly)
    {
        var courseExists = await _context.Courses
            .AsNoTracking()
            .AnyAsync(c => c.CourseId == courseId);

        if (!courseExists)
            throw new NotFoundException();

        var query = _context.Assignments
            .AsNoTracking()
            .Where(a => a.CourseId == courseId);

        if (publishedOnly)
            query = query.Where(a => a.IsPublished);

        return await query
            .Select(a => new AssignmentDto
            {
                AssignmentId = a.AssignmentId,
                Title = a.Title,
                Description = a.Description,
                DueDate = a.DueDate,
                MaxPoints = a.MaxPoints,
                IsPublished = a.IsPublished,
                SubmissionCount = a.Submissions.Count
            })
            .ToListAsync();
    }

    public async Task<StudentDashboardDto> GetStudentDashboardAsync(int studentId)
    {
        var dashboard = await _context.Students
            .AsNoTracking()
            .Where(s => s.StudentId == studentId)
            .Select(s => new StudentDashboardDto
            {
                StudentId = s.StudentId,
                IndexNumber = s.IndexNumber,
                FullName = s.FirstName + " " + s.LastName,
                IsActive = s.IsActive,
                Enrollments = s.Enrollments.Select(e => new EnrollmentSummaryDto
                {
                    EnrollmentId = e.EnrollmentId,
                    CourseCode = e.Course.Code,
                    CourseName = e.Course.Name,
                    EnrolledAt = e.EnrolledAt,
                    Status = e.Status
                }).ToList(),
                Submissions = s.Submissions.Select(sub => new SubmissionSummaryDto
                {
                    SubmissionId = sub.SubmissionId,
                    AssignmentTitle = sub.Assignment.Title,
                    RepositoryUrl = sub.RepositoryUrl,
                    SubmittedAt = sub.SubmittedAt,
                    Status = sub.Status,
                    Score = sub.Score
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (dashboard == null)
            throw new NotFoundException();

        return dashboard;
    }

    public async Task<SubmissionDto> CreateSubmissionAsync(CreateSubmissionDto request)
    {
        if (string.IsNullOrWhiteSpace(request.RepositoryUrl) || !request.RepositoryUrl.StartsWith("https://"))
            throw new BadRequestException();

        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.StudentId == request.StudentId);

        if (student == null)
            throw new NotFoundException();

        if (!student.IsActive)
            throw new BadRequestException();

        var assignment = await _context.Assignments
            .Include(a => a.Course)
            .FirstOrDefaultAsync(a => a.AssignmentId == request.AssignmentId);

        if (assignment == null)
            throw new NotFoundException();

        if (!assignment.IsPublished)
            throw new BadRequestException();

        var isEnrolled = await _context.Enrollments.AnyAsync(e =>
            e.StudentId == request.StudentId &&
            e.CourseId == assignment.CourseId &&
            (e.Status == "Active" || e.Status == "Completed"));

        if (!isEnrolled)
            throw new BadRequestException();

        var alreadySubmitted = await _context.Submissions.AnyAsync(s =>
            s.AssignmentId == request.AssignmentId &&
            s.StudentId == request.StudentId);

        if (alreadySubmitted)
            throw new ConflictException();

        var now = DateTime.UtcNow;
        var status = assignment.IsOverdue(now) ? "Late" : "Submitted";

        var submission = new Submission
        {
            AssignmentId = request.AssignmentId,
            StudentId = request.StudentId,
            RepositoryUrl = request.RepositoryUrl,
            SubmittedAt = now,
            Status = status
        };

        _context.Submissions.Add(submission);
        await _context.SaveChangesAsync();

        var created = await _context.Submissions
            .AsNoTracking()
            .Include(s => s.Student)
            .Include(s => s.Assignment)
            .FirstAsync(s => s.SubmissionId == submission.SubmissionId);

        return MapToDto(created);
    }

    public async Task<SubmissionDto> GradeSubmissionAsync(int submissionId, GradeSubmissionDto request)
    {
        
        var submission = await _context.Submissions
            .Include(s => s.Student)
            .Include(s => s.Assignment)
            .FirstOrDefaultAsync(s => s.SubmissionId == submissionId);

        if (submission == null)
            throw new NotFoundException();

        if (request.Score < 0)
            throw new BadRequestException();

        if (request.Score > submission.Assignment.MaxPoints)
            throw new BadRequestException();

        
        submission.Score = request.Score;
        submission.Feedback = request.Feedback;
        submission.Status = "Graded";

        await _context.SaveChangesAsync();

        return MapToDto(submission);
    }

    public async Task DeleteSubmissionAsync(int submissionId)
    {
        var submission = await _context.Submissions
            .FirstOrDefaultAsync(s => s.SubmissionId == submissionId);

        if (submission == null)
            throw new NotFoundException();

        if (submission.Status == "Graded")
            throw new BadRequestException();

        _context.Submissions.Remove(submission);
        await _context.SaveChangesAsync();
    }

    private static SubmissionDto MapToDto(Submission s) => new()
    {
        SubmissionId = s.SubmissionId,
        RepositoryUrl = s.RepositoryUrl,
        SubmittedAt = s.SubmittedAt,
        Status = s.Status,
        Score = s.Score,
        Feedback = s.Feedback,
        Student = new StudentInfoDto
        {
            StudentId = s.Student.StudentId,
            IndexNumber = s.Student.IndexNumber,
            FullName = s.Student.FullName
        },
        Assignment = new AssignmentInfoDto
        {
            AssignmentId = s.Assignment.AssignmentId,
            Title = s.Assignment.Title,
            MaxPoints = s.Assignment.MaxPoints
        }
    };
}
