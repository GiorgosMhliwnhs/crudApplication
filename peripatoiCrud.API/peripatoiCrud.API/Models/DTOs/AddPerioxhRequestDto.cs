using System.ComponentModel.DataAnnotations;

namespace peripatoiCrud.API.Models.DTOs
{
    public class AddPerioxhRequestDto
    {
        //Δηλωνουμε τα πεδια κωδικος και ονομα ως απαιτουμενα και εμφανιζουμε μηνυμα σφαλματος στην περιπτωση που ο χρηστης δεν στειλει ακριβως 3 χαρακτηρες για τον κωδικο
        [Required]
        [MinLength(3, ErrorMessage = "Ο κωδικός περιοχής πρέπει να είναι αυστηρά 3 χαρακτήρες")]
        [MaxLength(3, ErrorMessage = "Ο κωδικός περιοχής πρέπει να είναι αυστηρά 3 χαρακτήρες")]
        public String Kwdikos { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "Το όνομα της περιοχής είναι πολύ μεγάλο")]

        public String Onoma { get; set; }

        public String? EikonaUrl { get; set; }
    }
}
