using System.ComponentModel.DataAnnotations;


namespace visingsobiodlarna_backend.Models
{
    public class WinteringModel
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty; //Vilken användare som rapporterar

        [Required]
        public string Season { get; set; } = string.Empty; //vilken vinter ex "2024/2025"

        [Required]
        public int HiveCount { get; set; } //Antal invintrade kupor
    }
}
