using Recipe.Models;
using System.Threading.Tasks;

namespace Recipe.Repositories.IRepositories
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        Task<User> LoginAsync(string username, string password);
        Task<User> RegisterAsync(UserRegistrationModel user);
        Task<UserManagerResponse> ConfirmEmailAsync(string uid, string token);
    }
}
