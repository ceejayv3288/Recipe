using Recipe.Models;
using Recipe.Repositories.IRepositories;

namespace Recipe.Repositories
{
    public class UserRepository : IUserRepository
    {
        public User Authenticate(string username, string passwork)
        {
            throw new System.NotImplementedException();
        }

        public bool IsUniqueUser(string username)
        {
            throw new System.NotImplementedException();
        }

        public User Register(string username, string password)
        {
            throw new System.NotImplementedException();
        }
    }
}
