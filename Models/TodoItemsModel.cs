using System.ComponentModel.DataAnnotations;

namespace TrovTver.Models;

public class TodoItems
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "The Todo text is required.")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Todo text must be between 2 and 200 characters.")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Completed")]
    public bool IsDone { get; set; }

    [Display(Name = "Due Date")]
    [DataType(DataType.Date)]
    public DateTime? DueDate { get; set; }

    [Display(Name = "Created Date")]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
