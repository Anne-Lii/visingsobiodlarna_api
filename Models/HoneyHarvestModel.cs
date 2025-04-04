using System.ComponentModel.DataAnnotations;

namespace visingsobiodlarna_backend.Models;

public class HoneyHarvestModel
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty; //Vem som raporterar

        [Required]
        public int Year { get; set; } //Vilket år skörden gäller

        public DateTime? HarvestDate { get; set; } //Datum - om det är en enskild slungning (kan vara null om det är total)

        [Required]
        public int AmountKg { get; set; } //Antal kilo honung

        [Required]
        public bool IsTotalForYear { get; set; } = false; //true = Totalskörd för året, false = Enskild slungning
    }