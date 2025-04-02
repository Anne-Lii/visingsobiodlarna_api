using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace visingsobiodlarna_backend.Models;
public class Apiary
{
    public int Id { get; set; }

    [Required]
    public string? Name { get; set; }

    public string? Location { get; set; }

    //Foreign key till användaren
    [Required]
    public string? UserId { get; set; }

    [ForeignKey("UserId")]
    public ApplicationUser? User { get; set; }

   
}