using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace visingsobiodlarna_backend.Models;
public class ApiaryModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Namn på bigården är obligatoriskt")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Vart är bigården placerad? Ex hemma, nere vid ån, bakom ladan")]
    public string Location { get; set; } = null!;

    [Required]
    public string UserId { get; set; } = null!;

    [ForeignKey("UserId")]
    public ApplicationUser? User { get; set; }

    public ICollection<HiveModel> Hives { get; set; } = new List<HiveModel>();
}