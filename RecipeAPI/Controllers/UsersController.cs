using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Recipe.Models;
using Recipe.Repositories.IRepositories;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Recipe.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;

        public UsersController(IUserRepository userRepository, UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] AuthenticationModel model)
        {
            var user = await _userRepository.LoginAsync(model.Username, model.Password);

            if (user == null)
                return Unauthorized(new User { Response = new UserManagerResponse { IsSuccess = false, Message = "Error while login" } });
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRegistrationModel model)
        {
            bool ifUserNameUnique = _userRepository.IsUniqueUser(model.Username);
            if (!ifUserNameUnique)
                return BadRequest(new User { Response = new UserManagerResponse { IsSuccess = false, Message = "Username already exist" } });
            var user = await _userRepository.RegisterAsync(model);

            if (user == null)
                return BadRequest(new User { Response = new UserManagerResponse { IsSuccess = false, Message = "Error while registering" } });

            return Ok(user);
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
            var result = await _userRepository.ConfirmEmailAsync(user.Id, token);
            var body = System.IO.File.ReadAllText(string.Format("Templates/EmailConfirmed.html"));
            if (result.IsSuccess)
            {
                return new ContentResult
                {
                    ContentType = "text/html",
                    StatusCode = (int)HttpStatusCode.OK,
                    Content = body
                };
            }
            return BadRequest(result);
        }
    }
}
