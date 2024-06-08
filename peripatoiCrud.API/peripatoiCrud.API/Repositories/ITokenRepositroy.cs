using Microsoft.AspNetCore.Identity;

namespace peripatoiCrud.API.Repositories
{
    public interface ITokenRepositroy
    {
        string DhmiourgiaJWTToken(IdentityUser user, List<string> roloi);
    }
}
