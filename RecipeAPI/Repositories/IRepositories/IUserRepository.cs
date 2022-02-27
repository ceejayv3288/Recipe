using RecipeAPI.Models;
using System.Threading.Tasks;

namespace RecipeAPI.Repositories.IRepositories
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        Task<ResponseModel> AuthenticateAsync(string username, string password);
        Task<UserModel> LoginAsync(string username, string password);
        Task<ResponseModel> RegisterAsync(UserRegistrationModel user);
        Task<ResponseModel> ConfirmEmailAsync(string uid, string token);
    }
}
