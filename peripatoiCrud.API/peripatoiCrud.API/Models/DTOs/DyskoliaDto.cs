namespace peripatoiCrud.API.Models.DTOs
{
    public class DyskoliaDto //δημιουργηθηκε για να μπορεσουμε να το δηλωσουμε στο dto του περιπατου για επιστροφη στον client
    {
        public Guid Id { get; set; }
        public String Onoma { get; set; }
    }
}
