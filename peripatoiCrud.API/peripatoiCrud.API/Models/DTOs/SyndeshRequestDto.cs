using System.ComponentModel.DataAnnotations;

namespace peripatoiCrud.API.Models.DTOs
{
    public class SyndeshRequestDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
