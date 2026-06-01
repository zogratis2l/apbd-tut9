using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityTasksDbFirstApi.Models;

[Table("Submissions")]
public partial class Submission
{
    [Key]
    public int SubmissionId { get; set; }

    [Required]
    public int AssignmentId { get; set; }

    [Required]
    public int StudentId { get; set; }

    [Required]
    [MaxLength(300)]
    public string RepositoryUrl { get; set; } = null!;

    [Required]
    public DateTime SubmittedAt { get; set; }

    public int? Score { get; set; }

    [MaxLength(1000)]
    public string? Feedback { get; set; }

    [Required]
    [MaxLength(30)]
    public string Status { get; set; } = null!;

    [ForeignKey(nameof(AssignmentId))]
    public virtual Assignment Assignment { get; set; } = null!;

    [ForeignKey(nameof(StudentId))]
    public virtual Student Student { get; set; } = null!;
}
