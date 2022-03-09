using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RecipeAPI.Data;
using RecipeAPI.Models;
using RecipeAPI.Repositories.IRepositories;
using RecipeAPI.Services.IServices;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RecipeAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _db;
        private readonly AppSettings _appSettings;
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public UserRepository(ApplicationDbContext db, IOptions<AppSettings> appsettings, UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, IMapper mapper, IEmailService emailService, IConfiguration configuration)
        {
            _db = db;
            _appSettings = appsettings.Value;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<ResponseModel> AuthenticateAsync(string username, string password)
        {
            var userResult = await _userManager.FindByNameAsync(username);
            if (userResult == null)
                return new ResponseModel { StatusCode = (int)HttpStatusCode.Unauthorized, Message = "There is no user with that username." };
            var checkPasswordResult = await _userManager.CheckPasswordAsync(userResult, password);
            if (!checkPasswordResult)
                return new ResponseModel { StatusCode = (int)HttpStatusCode.Unauthorized, Message = "Invalid password." };
            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(userResult);
            if (!isEmailConfirmed)
                return new ResponseModel { StatusCode = (int)HttpStatusCode.Unauthorized, Message = "Email is not confirmed." };

            return new ResponseModel { StatusCode = (int)HttpStatusCode.OK };
        }

        public async Task<UserModel> LoginAsync(string username, string password)
        {
            var userResponse = await _userManager.FindByNameAsync(username);

            await _signInManager.PasswordSignInAsync(userResponse.UserName, password, true, false);
            //if user was found, generate a jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, userResponse.Id.ToString()),
                    new Claim(ClaimTypes.Role, userResponse.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration.GetSection("AppSettings:Issuer").Value,
                Audience = _configuration.GetSection("AppSettings:Audience").Value
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            userResponse.Token = tokenHandler.WriteToken(token);
            userResponse.PasswordHash = string.Empty;

            return userResponse;
        }

        public bool IsUniqueUser(string username)
        {
            var user = _db.Users.SingleOrDefault(x => x.UserName == username);

            if (user == null)
                return true;

            return false;
        }

        public async Task<ResponseModel> RegisterAsync(UserRegistrationModel user)
        {
            var userObj = _mapper.Map<UserModel>(user);
            userObj.Role = "Member";
            var result = await _userManager.CreateAsync(userObj, user.Password);
            if (!result.Succeeded)
                return new ResponseModel { StatusCode = (int)HttpStatusCode.BadRequest, Message = result.Errors.FirstOrDefault().Description, Errors = result.Errors.Select(e => e.Description) };

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(userObj);
            string appDomain = _configuration.GetSection("Application:AppDomain").Value;
            string confirmationLink = _configuration.GetSection("Application:EmailConfirmation").Value;

            UserEmailOptionsModel options = new UserEmailOptionsModel
            {
                ToEmails = new List<string> { user.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}", user.FirstName),
                    new KeyValuePair<string, string>("{{Link}}",
                        string.Format(appDomain + confirmationLink, userObj.Id, HttpUtility.UrlEncode(token)))
                }
            };
            await _emailService.SendEmailForEmailConfirmation(options);

            return new ResponseModel { StatusCode = (int)HttpStatusCode.OK, Message = "Please check you email to complete the registration." };
        }

        public async Task<ResponseModel> ConfirmEmailAsync(string uid, string token)
        {
            var user = await _userManager.FindByIdAsync(uid);
            if (user == null)
                return new ResponseModel { StatusCode = (int)HttpStatusCode.BadRequest, Message = "User not found" };

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
                return new ResponseModel { StatusCode = (int)HttpStatusCode.BadRequest, Message = result.Errors.FirstOrDefault().Description, Errors = result.Errors.Select(e => e.Description) };

            return new ResponseModel { StatusCode = (int)HttpStatusCode.OK, Message = "Email confirmed!" };
        }
    }
}
