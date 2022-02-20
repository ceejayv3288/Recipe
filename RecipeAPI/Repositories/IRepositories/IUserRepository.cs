using RecipeAPI.Models;
using System.Threading.Tasks;

namespace RecipeAPI.Repositories.IRepositories
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        Task<UserModel> LoginAsync(string username, string password);
        Task<UserModel> RegisterAsync(UserRegistrationModel user);
        Task<UserManagerResponseModel> ConfirmEmailAsync(string uid, string token);
    }
}
