namespace peripatoiCrud.API.Models.Domain
{
    // οι περιπατοι/δρομακια/πεζοδρομοι
    public class Peripatos
    {
        public Guid Id { get; set; }
        public String Onoma { get; set; }
        public String Perigrafh { get; set; }
        public double Mhkos { get; set; }
        public string? EikonaUrl { get; set; }

        public Guid DyskoliaId { get; set; }
        public Guid PerioxhId { get; set; }

        public Dyskolia Dyskolia { get; set; }
        public Perioxh Perioxh { get; set; }
    }
}
