namespace Tours.Service.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Tours.Models;

    public interface IUserService
    {
        Task<List<User>> GetAllUsers();

        Task<User> GetUserById(string id);

        Task<User> GetUserByEmail(string email);

        Task<List<User>> GetAllUsersByUsername(string username);

        Task<bool> UpdatePassword(string newPassword, string password);

        Task<bool> UpdateUser(UserModel model);

        Task DeleteUser(string id);

        Task<string> GetUsernameByEmail(string email);
    }
}
