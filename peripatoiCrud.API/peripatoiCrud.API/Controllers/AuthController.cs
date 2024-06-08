using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using peripatoiCrud.API.Models.DTOs;
using peripatoiCrud.API.Repositories;

namespace peripatoiCrud.API.Controllers
{
    [Route("api/Register")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepositroy tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepositroy tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        ///api/Auth/Register
        [HttpPost]
        [Route("Eggrafh")]
        public async Task<IActionResult> Register([FromBody] EggrafhRequestDto eggrafhRequestDto)
        {
            var identityUser = new IdentityUser // δημιουργουμε ενα identityUser και του περναμε τα στοιχεια που μας ηρθαν απο τον client
            {
                UserName = eggrafhRequestDto.Username,
                Email = eggrafhRequestDto.Username
            };

            var identityResult = await userManager.CreateAsync(identityUser, eggrafhRequestDto.Password); //εδω γινεται η δημιουργια του χρηστη βαση των στοιχειων

            //και στη περιπτωση που γινει η εγγραφη με επιτυχια (επιστραφει succeeded) τοτε πρεπει να προσδιορισουμε ρολους στον χρηστη (read, write κτλπ)
            if (identityResult.Succeeded)
            {
                if (eggrafhRequestDto.Roloi != null && eggrafhRequestDto.Roloi.Any()) //οποτε εδω τσεκαρουμε οτι υπαρχουν ρολοι στο request
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, eggrafhRequestDto.Roloi); // εαν ναι με την υπαρχουσα addtorolesasync τις περναμε στο identityResult

                    if (identityResult.Succeeded)
                    {
                        return Ok("Εγγραφή Επιτυχής");
                    }
                }
            }

            return BadRequest("Κάτι πήγε στραβά με την εγγραφή. Ξαναπροσπάθησε αργότερα.");
        }

        ///api/Auth/Login
        [HttpPost]
        [Route("Syndesh")]
        public async Task<IActionResult> Login([FromBody] SyndeshRequestDto syndeshRequestDto)
        {
            //ψαχνουμε τον χρηστη μεσω του email στη βαση
            var xrhsths = await userManager.FindByEmailAsync(syndeshRequestDto.Username);

            if (xrhsths != null) 
            {
                //Εαν ο χρήστης υπαρχει ελεγχουμε τον κωδικο του me thn checkpasswordasync καθως προφανως ειναι hashαρισμενος
                var checkKwdikosResult = await userManager.CheckPasswordAsync(xrhsths, syndeshRequestDto.Password);

                if (checkKwdikosResult)
                {
                    //εφοσον ο χρηστης υπαρχει και χρησιμοποιει σωστο κωδικο πρεπει να παραξουμε το JWT token
                    //πρωτα ψαχνουμε για τους ρολους
                    var roloi = await userManager.GetRolesAsync(xrhsths);

                    if (roloi != null)
                    {
                        var jwtToken = tokenRepository.DhmiourgiaJWTToken(xrhsths, roloi.ToList()); //δημιουργια του jwt token απο το token repository

                        //εδω δημιουργηθηκε response dto για μονο για το token γιατι μπορει στο μελλον το business να αποφασισει χρειαστει να επιστρεφουν κι αλλα δεδομενα κατα τη συνδεση
                        var response = new SyndeshResponseDto
                        {
                            JwtToken = jwtToken
                        };

                        return Ok(response);
                    }
                }
            }

            //διαφορετικα λαμβανει μηνυμα σφαλματος και 400
            return BadRequest("Το όνομα χρήστη ή ο κωδικός είναι λανθασμένα");
        }
    }
}
