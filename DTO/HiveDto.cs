using System.ComponentModel.DataAnnotations;

namespace visingsobiodlarna_backend.DTOs
{
    public class HiveDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Namn är obligatoriskt")]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public int StartYear { get; set; }

        [Required]
        public int ApiaryId { get; set; }
    }
}
