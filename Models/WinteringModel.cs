using System.ComponentModel.DataAnnotations;


namespace visingsobiodlarna_backend.Models
{
    public class WinteringModel
    {
        public int Id { get; set; }

        [Required]
        public string? UserId { get; set; } //Vilken användare som rapporterar

        [Required(ErrorMessage = "Vinter är obligatoriskt ex: vinter 2023/2024")]
        public string? Season { get; set; } //vilken vinter ex "2024/2025"

        [Required(ErrorMessage = "Antal invintrade kupor är obligatoriskt")]
        public int HiveCount { get; set; } //Antal invintrade kupor
    }
}
