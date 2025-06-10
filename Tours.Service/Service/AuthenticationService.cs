namespace Tours.Service
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Tours.Interface;
    using Tours.Service.Interface;

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;

        private readonly IEmailService _emailService;

        private readonly ITokenService _tokenService;

        private readonly Regex _emailRegex = new Regex(@"^(\w)+@(gmail\.com|mail\.ru|yandex\.ru)$");

        private readonly Regex _passwordRegex = new Regex(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[^\w\s]).{8,}$");

        public AuthenticationService(IUserRepository userRepository, IEmailService emailService, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _tokenService = tokenService;
        }

        public async Task<Token> AuthenticateAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == null || !VerifyPassword(password, user.Password))
            {
                throw new InvalidCredentialsException();
            }

            var token = _tokenService.GetToken(user.Email, user.IsAdmin ? "Admin" : "User");

            return token;
        }

        public async Task<Token> RegistrationAsync(string username, string password, string email)
        {

            if (!await IsEmailAvailableAsync(email))
            {
                throw new EmailAlreadyExistsException();
            }

            await _userRepository.AddUserAsync(username, email, password);

            var token = _tokenService.GetToken(email, "User");

            return token;
        }

        public bool IsEmailCorrect(string email)
        {
            return _emailRegex.IsMatch(email);
        }

        public async Task<bool> IsEmailAvailableAsync(string email)
        {
            return await _userRepository.UserEmailNotExistAsync(email);
        }

        public async Task<string> SendVerifyCodeToEmailAsync(string email)
        {
            string code = Guid.NewGuid().ToString();

            if (IsEmailCorrect(email) && await IsEmailAvailableAsync(email) && _emailService.SendEmail(email, "Подтверждение почты", code))
            {
                return code;
            }

            return null;
        }

        public bool IsPasswordCorrect(string password)
        {
            return _passwordRegex.IsMatch(password);
        }

        public bool VerifyPassword(string password, string storedPassword)
        {
            return password == storedPassword;
        }

        private string HashPassword(string password)
        {
            return password;
        }
    }
}
