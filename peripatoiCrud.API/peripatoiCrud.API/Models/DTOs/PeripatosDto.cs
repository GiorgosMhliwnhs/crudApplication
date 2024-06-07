namespace peripatoiCrud.API.Models.DTOs
{
    public class PeripatosDto
    {
        public Guid Id { get; set; }
        public String Onoma { get; set; }
        public String Perigrafh { get; set; }
        public double Mhkos { get; set; }
        public string? EikonaUrl { get; set; }

        public Guid DyskoliaId { get; set; }
        public Guid PerioxhId { get; set; }

        public PerioxhDto Perioxh { get; set; }
        public DyskoliaDto Dyskolia { get; set; }
    }
}
