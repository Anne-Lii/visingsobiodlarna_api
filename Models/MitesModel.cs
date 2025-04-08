using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace visingsobiodlarna_backend.Models;

public class MitesModel
    {
        public int Id { get; set; }//id på rapporten

        [Required]
        public int HiveId { get; set; }//id på kupan det gäller

        [ForeignKey("HiveId")]
        public HiveModel? Hive { get; set; }

        [Required(ErrorMessage = "År är obligatoriskt")]
        public int Year { get; set; }//året

        [Required(ErrorMessage = "Vecka är obligatoriskt")]
        public int Week { get; set; }//vecka som rapporteras

        [Required(ErrorMessage = "Antal kvalster är obligatoriskt")]
        public int MiteCount { get; set; } //Antal kvalster
    }