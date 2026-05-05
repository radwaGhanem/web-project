using WebApplication1.Data;

namespace WebApplication1.services
{
    public interface IAuthService
    {
        Task<AppUser?> ValidateUserCredentialsAsync(string email, string password);
        string CreateToken(AppUser user);
    }
}
