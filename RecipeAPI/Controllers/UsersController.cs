using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RecipeAPI.Models;
using RecipeAPI.Repositories.IRepositories;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace RecipeAPI.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<UserModel> _userManager;

        public UsersController(IUserRepository userRepository, UserManager<UserModel> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] AuthenticationModel model)
        {
            var authResponse = await _userRepository.AuthenticateAsync(model.Username, model.Password);
            if (authResponse.StatusCode != (int)HttpStatusCode.OK)
                return Unauthorized(authResponse);

            var response = await _userRepository.LoginAsync(model.Username, model.Password);
            if (response == null)
                return Unauthorized(new ResponseModel { StatusCode = (int)HttpStatusCode.BadRequest, Message = "Error while login" });

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRegistrationModel model)
        {
            bool ifUserNameUnique = _userRepository.IsUniqueUser(model.Username);
            if (!ifUserNameUnique)
                return BadRequest(new ResponseModel { StatusCode = (int)HttpStatusCode.BadRequest, Message = "Username already exist" });

            var response = await _userRepository.RegisterAsync(model);
            if (response == null)
                return BadRequest(new ResponseModel { StatusCode = (int)HttpStatusCode.BadRequest, Message = "Error while registering" });

            if (response.StatusCode == (int)HttpStatusCode.OK)
                return Ok(response);
            else
                return BadRequest(new ResponseModel { StatusCode = (int)HttpStatusCode.BadRequest, Message = response.Message});
        }

        [AllowAnonymous]
        [HttpPost("confirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync(string uid, string token)
        {
            if (string.IsNullOrWhiteSpace(uid) || string.IsNullOrWhiteSpace(token))
                return NotFound();

            var user = await _userManager.FindByIdAsync(uid);
            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            if (isEmailConfirmed)
                return Ok();

            HttpUtility.UrlDecode(token);
            var response = await _userRepository.ConfirmEmailAsync(user.Id, token);
            var body = System.IO.File.ReadAllText(string.Format("Templates/EmailConfirmed.html"));
            if (response.StatusCode == (int)HttpStatusCode.OK)
            {
                return new ContentResult
                {
                    ContentType = "text/html",
                    StatusCode = (int)HttpStatusCode.OK,
                    Content = body
                };
            }
            return BadRequest(response);
        }
    }
}
