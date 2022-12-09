using RPG_DOTNET.Dtos.EmailDto;
using RPG_DOTNET.Models;

namespace RPG_DOTNET.Services.EmailServices
{
    public interface IEmailService
    {
        void SendEmail(Email emailRequest);
    }
}
