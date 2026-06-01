using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityTasksDbFirstApi.Models;

[Table("Enrollments")]
public partial class Enrollment
{
    [Key]
    public int EnrollmentId { get; set; }

    [Required]
    public int StudentId { get; set; }

    [Required]
    public int CourseId { get; set; }

    [Required]
    public DateOnly EnrolledAt { get; set; }

    [Required]
    [MaxLength(30)]
    public string Status { get; set; } = null!;

    [ForeignKey(nameof(StudentId))]
    public virtual Student Student { get; set; } = null!;

    [ForeignKey(nameof(CourseId))]
    public virtual Course Course { get; set; } = null!;
}
