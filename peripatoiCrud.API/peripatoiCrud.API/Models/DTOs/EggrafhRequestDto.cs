using System.ComponentModel.DataAnnotations;

namespace peripatoiCrud.API.Models.DTOs
{
    public class EggrafhRequestDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string[] Roloi { get; set; }
    }
}
