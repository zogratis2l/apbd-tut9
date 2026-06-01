using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityTasksDbFirstApi.Models;

[Table("Students")]
public partial class Student
{
    [Key]
    public int StudentId { get; set; }

    [Required]
    [MaxLength(20)]
    public string IndexNumber { get; set; } = null!;

    [Required]
    [MaxLength(80)]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(80)]
    public string LastName { get; set; } = null!;

    [Required]
    [MaxLength(160)]
    public string Email { get; set; } = null!;

    [Required]
    public DateOnly EnrollmentDate { get; set; }

    [Required]
    public bool IsActive { get; set; } = true;

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}
