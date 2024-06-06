namespace peripatoiCrud.API.Models.Domain
{
    // οι περιοχες οπου εχουν περιπατους, δρομακια, πεζοδρομους κτλπ
    public class Perioxh
    {
        public Guid Id { get; set; }
        public String Kwdikos { get; set; }
        public String Onoma { get; set; }
        public String? EikonaUrl { get; set; }
    }
}
