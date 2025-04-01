using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public string FirstName {get; set;}//Förnamn på medlem
    public string LastName {get; set;}//Efternamn på medlem
    public string FullName => $"{FirstName} {LastName}";//visning av hela namnet 
    public bool IsApprovedByAdmin { get; set; } =false;//registrering godkänd av admin false som default
}