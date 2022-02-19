using Recipe.Models;
using System.Threading.Tasks;

namespace Recipe.Repositories.IRepositories
{
    public interface IEmailService
    {
        Task SendEmailForEmailConfirmation(UserEmailOptions userEmailOptions);

        Task SendEmailForForgotPassword(UserEmailOptions userEmailOptions);
    }
}
