using System.ComponentModel.DataAnnotations;

namespace visingsobiodlarna_backend.Models
{
    public class NewsModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Titel är obligatoriskt")]
        public string? Title { get; set; }

        public string? Content { get; set; }
        
        public DateTime PublishDate { get; set; }
    }
}
