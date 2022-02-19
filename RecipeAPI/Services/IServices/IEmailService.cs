using Recipe.Models;
using System.Threading.Tasks;

namespace Recipe.Services.IServices
{
    public interface IEmailService
    {
        Task SendEmailForEmailConfirmation(UserEmailOptions userEmailOptions);

        Task SendEmailForForgotPassword(UserEmailOptions userEmailOptions);
    }
}
