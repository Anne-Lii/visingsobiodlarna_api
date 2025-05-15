using System.ComponentModel.DataAnnotations;

namespace visingsobiodlarna_backend.DTOs;

public class ApiaryDto
{

    public int? Id { get; set; }

    [Required(ErrorMessage = "Namn är obligatoriskt")]   
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Plats är obligatoriskt")]
    public string Location { get; set; } = null!;
 
    public int HiveCount { get; set; }
}