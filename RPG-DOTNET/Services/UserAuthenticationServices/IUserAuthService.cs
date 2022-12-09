using RPG_DOTNET.Models;

namespace RPG_DOTNET.Services.UserAuthenticationServices
{
    public interface IUserAuthService
    {
        Task<ServiceResponce<int>> Register(User user, string password);
        Task<ServiceResponce<string>> Login(string email, string password);
        Task<ServiceResponce<string>> Verify(string token);
        Task<bool> UserExist(string username);
    }
}
