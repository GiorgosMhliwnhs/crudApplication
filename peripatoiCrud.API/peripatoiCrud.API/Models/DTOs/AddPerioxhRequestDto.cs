namespace peripatoiCrud.API.Models.DTOs
{
    public class AddPerioxhRequestDto
    {
        public Guid Id { get; set; }
        public String Kwdikos { get; set; }
        public String Onoma { get; set; }
        public String? EikonaUrl { get; set; }
    }
}
