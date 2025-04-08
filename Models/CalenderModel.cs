using System.ComponentModel.DataAnnotations;

namespace visingsobiodlarna_backend.Models;

public class CalenderModel {
    public int Id { get; set; }
    [Required]
    public string? Title { get; set; }
    public string? Content { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

}