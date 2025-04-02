using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace visingsobiodlarna_backend.Models;
public class HiveModel
{
     public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public int ApiaryId { get; set; }

    [ForeignKey("ApiaryId")]
    public ApiaryModel? Apiary { get; set; }

}