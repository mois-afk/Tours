namespace Tours.Service.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAuthenticationService
    {
        public Task<Token> AuthenticateAsync(string username, string password);

        public Task<Token> RegistrationAsync(string username, string password, string email);

        public bool IsEmailCorrect(string email);

        public Task<string> SendVerifyCodeToEmailAsync(string email);

        public Task<bool> IsEmailAvailableAsync(string email);

        public bool IsPasswordCorrect(string password);

        public bool VerifyPassword(string password, string storedPassword);
    }
}
