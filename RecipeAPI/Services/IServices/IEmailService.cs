using RecipeAPI.Models;
using System.Threading.Tasks;

namespace RecipeAPI.Services.IServices
{
    public interface IEmailService
    {
        Task SendEmailForEmailConfirmation(UserEmailOptionsModel userEmailOptions);

        Task SendEmailForForgotPassword(UserEmailOptionsModel userEmailOptions);
    }
}
