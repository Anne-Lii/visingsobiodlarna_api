using System.ComponentModel.DataAnnotations;

namespace visingsobiodlarna_backend.Models;

public class CalenderModel {

    public int Id { get; set; }

    [Required(ErrorMessage = "Titel är obligatoriskt")]
    public string? Title { get; set; }

    public string? Content { get; set; }

    [Required(ErrorMessage = "Datum/startdatum är obligatoriskt")]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

}