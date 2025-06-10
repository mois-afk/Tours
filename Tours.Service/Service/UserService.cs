namespace Tours.Service
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using Tours.Service.Interface;
    using Tours.Models;
    using Tours.Interface;

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        private readonly IRedisService _redisService;

        private readonly IAuthenticationService _authenticationService;

        public UserService(IUserRepository userRepository, IRedisService redisService, IAuthenticationService authenticationService)
        {
            _userRepository = userRepository;
            _redisService = redisService;
            _authenticationService = authenticationService;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<User> GetUserById(string id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }

        public async Task<List<User>> GetAllUsersByUsername(string username)
        {
            return await _userRepository.GetUsersByPartUsernameAsync(username);
        }

        public async Task<bool> UpdatePassword(string newPassword, string password)
        {
            string email = _redisService.Get<string>("Email");
            var user = await _userRepository.GetUserByEmailAsync(email);

            return await _userRepository.UpdatePassword(newPassword, email);
        }

        public async Task<bool> UpdateUser(UserModel model)
        {
            var user = await _userRepository.GetUserByIdAsync(model.UserId);

            if (user.Email != model.Email)
            {
                if (!_authenticationService.IsEmailCorrect(model.Email))
                {
                    throw new EmailAlreadyExistsException();
                }

                if (!await _authenticationService.IsEmailAvailableAsync(model.Email))
                {
                    throw new EmailAlreadyExistsException();
                }
            }

            return await _userRepository.UpdateUser(model.UserId, model.Email, model.Username);
        }

        public async Task DeleteUser(string id)
        {
            await _userRepository.DeleteUserAsync(id);
        }

        public async Task<string> GetUsernameByEmail(string email)
        {
            return await _userRepository.GetUsernameByEmailAsync(email);
        }
    }
}
