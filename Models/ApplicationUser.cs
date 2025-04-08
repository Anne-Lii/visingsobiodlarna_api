using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace visingsobiodlarna_backend.Models;

public class ApplicationUser : IdentityUser
{
    [Required(ErrorMessage = "Förnamn är obligatoriskt")]
    public string? FirstName {get; set;}//Förnamn på medlem

    [Required(ErrorMessage = "Efternamn är obligatoriskt")]
    public string? LastName {get; set;}//Efternamn på medlem

    public string? FullName => $"{FirstName} {LastName}";//visning av hela namnet 
    
    public bool IsApprovedByAdmin { get; set; } =false;//registrering godkänd av admin false som default
}