namespace visingsobiodlarna_backend.DTOs;

public class HoneyHarvestDto
{
    public int Id { get; set; }
    public int Year { get; set; }
    public DateTime? HarvestDate { get; set; }
    public decimal AmountKg { get; set; }
    public string? BatchId { get; set; }
    public bool IsTotalForYear { get; set; }
}