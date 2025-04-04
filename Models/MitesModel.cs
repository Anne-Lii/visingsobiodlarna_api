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

        [Required]
        public int Year { get; set; }//året

        [Required]
        public int Week { get; set; }//vecka som rapporteras

        [Required]
        public int MiteCount { get; set; } //Antal kvalster
    }