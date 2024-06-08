using System.ComponentModel.DataAnnotations;

namespace peripatoiCrud.API.Models.DTOs
{
    public class UpdatePeripatosRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Το όνομα δεν μπορεί να είναι τόσο μικρό")]
        [MaxLength(30, ErrorMessage = "Το όνομα είναι πολύ μεγάλο")]
        public String Onoma { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = "Η περιγραφή είναι πολύ μικρή")]
        [MaxLength(1000, ErrorMessage = "Η περιγραφή είναι πολύ μεγάλη")]
        public String Perigrafh { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "Το μήκος του περίπατου είναι εκτός ορίων")]
        public double Mhkos { get; set; }

        public string? EikonaUrl { get; set; }

        [Required]
        public Guid DyskoliaId { get; set; }
        [Required]
        public Guid PerioxhId { get; set; }
    }
}
