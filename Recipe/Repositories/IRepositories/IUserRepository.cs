using Recipe.Models;

namespace Recipe.Repositories.IRepositories
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        User Authenticate(string username, string passwork);
        User Register(User user);
    }
}
