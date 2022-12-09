using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RPG_DOTNET.Data;
using RPG_DOTNET.Dtos.UserDto;
using RPG_DOTNET.Models;
using RPG_DOTNET.Repository;
using RPG_DOTNET.Services.EmailServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web.Helpers;

namespace RPG_DOTNET.Services.UserAuthenticationServices
{
    public class UserAuthService : IUserAuthService
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public UserAuthService(DataContext dataContext, IConfiguration configuration, IEmailService emailService)
        {
            _dataContext = dataContext;
            _configuration = configuration;
            _emailService = emailService;
        }
        public async Task<ServiceResponce<string>> Login(string email, string password)
        {
            ServiceResponce<string> response = new ServiceResponce<string>();
            try
            {
                User user = await _dataContext.User.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(email.ToLower()));
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "User not found.";
                    return response;
                }
                if (!VerifyUserPassord(password, user.PasswordHash, user.PasswordSalt))
                {
                    response.Success = false;
                    response.Message = "Wrong Password.";
                    return response;
                }
                if (user.VerifiedAt == null)
                {
                    response.Success = false;
                    response.Message = "Not Verified!";
                    return response;
                }
                response.Data = CreateToken(user);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }

        public async Task<ServiceResponce<int>> Register(User user, string password)
        {
            ServiceResponce<int> response = new ServiceResponce<int>();
            try
            {
                if (await UserExist(user.Email))
                {
                    response.Success = false;
                    response.Message = "User already exists.";
                    return response;
                }
                CreateHashPassword(password, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.VerificationToken = CreateVerifiedToken();
                await _dataContext.User.AddAsync(user);
                await _dataContext.SaveChangesAsync();
                Email email = new Email();
                email.To = user.Email;
                email.Body = _configuration.GetSection("BaseUrl").Value + "user/verify/" + user.VerificationToken;
                email.Subject = "Verify User";
                _emailService.SendEmail(email);
                response.Data = user.Id;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
            return response;

        }

        public async Task<ServiceResponce<string>> Verify(string token)
        {
            ServiceResponce<string> response = new ServiceResponce<string>();
            try
            {
                User user = await _dataContext.User.FirstOrDefaultAsync(x => x.VerificationToken == token);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "Invalid User";
                    return response;
                }
                user.VerifiedAt = DateTime.Now;
                await _dataContext.SaveChangesAsync();
                response.Message = "Verified User!";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }

        public async Task<bool> UserExist(string email)
        {
            if (await _dataContext.User.AnyAsync(x => x.Email.ToLower() == email.ToLower()))
            {
                return true;
            }
            return false;

        }

        private void CreateHashPassword(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                passwordSalt = hmac.Key;
            }
        }

        private bool VerifyUserPassord(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computeHash.Length; i++)
                {
                    if (passwordHash[i] != computeHash[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        private string CreateVerifiedToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name)
            };

            string tokenSymmetric = _configuration.GetSection("AppSettings:Token").Value;

            SymmetricSecurityKey key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(tokenSymmetric)
                );

            SigningCredentials cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = cred
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);

            return jwtSecurityTokenHandler.WriteToken(token);
        }
    }
}
