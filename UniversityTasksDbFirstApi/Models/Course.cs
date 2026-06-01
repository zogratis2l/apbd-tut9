using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityTasksDbFirstApi.Models;

[Table("Courses")]
public partial class Course
{
    [Key]
    public int CourseId { get; set; }

    [Required]
    [MaxLength(20)]
    public string Code { get; set; } = null!;

    [Required]
    [MaxLength(160)]
    public string Name { get; set; } = null!;

    [Required]
    public int Credits { get; set; }

    [Required]
    public bool IsActive { get; set; } = true;

    public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
