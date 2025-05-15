using System.ComponentModel.DataAnnotations;

namespace visingsobiodlarna_backend.DTOs;

public class MiteDto
{

    public int? Id { get; set; }
    
    [Required]
    public int HiveId { get; set; }

    [Required]
    [Range(1900, 3000, ErrorMessage = "Ogiltigt år XXXX")]
    public int Year { get; set; }

    [Required]
    [Range(1, 53, ErrorMessage = "Vecka måste vara mellan 1 och 53")]
    public int Week { get; set; }

    [Required]
    [Range(0, 10000, ErrorMessage = "Kvalsterantal måste vara mellan 0 och 1000")]
    public int MiteCount { get; set; }
}
