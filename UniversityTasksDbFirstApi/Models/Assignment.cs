using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityTasksDbFirstApi.Models;

[Table("Assignments")]
public partial class Assignment
{
    [Key]
    public int AssignmentId { get; set; }

    [Required]
    public int CourseId { get; set; }

    [Required]
    [MaxLength(160)]
    public string Title { get; set; } = null!;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    [Required]
    public int MaxPoints { get; set; }

    [Required]
    public bool IsPublished { get; set; } = false;

    [ForeignKey(nameof(CourseId))]
    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}
