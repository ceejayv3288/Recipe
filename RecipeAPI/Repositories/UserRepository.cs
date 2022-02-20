﻿using AutoMapper;
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

        public async Task<UserModel> LoginAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return new UserModel
                {
                    Response = new UserManagerResponseModel
                    {
                        Message = "There is no user with that username",
                        IsSuccess = false
                    }
                };
            }
            var result = await _userManager.CheckPasswordAsync(user, password);
            if (!result)
                return new UserModel
                {
                    Response = new UserManagerResponseModel
                    {
                        Message = "Invalid password",
                        IsSuccess = false,
                    }
                };

            await _signInManager.PasswordSignInAsync(user.UserName, password, true, false);
            //if user was found, generate a jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration.GetSection("AppSettings:Issuer").Value,
                Audience = _configuration.GetSection("AppSettings:Audience").Value
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            user.PasswordHash = string.Empty;
            user.Response = new UserManagerResponseModel { IsSuccess = true, Message = "Login Successful." };

            return user;
        }

        public bool IsUniqueUser(string username)
        {
            var user = _db.Users.SingleOrDefault(x => x.UserName == username);

            if (user == null)
                return true;

            return false;
        }

        public async Task<UserModel> RegisterAsync(UserRegistrationModel user)
        {
            var userObj = _mapper.Map<UserModel>(user);
            var result = await _userManager.CreateAsync(userObj, user.Password);
            if (!result.Succeeded)
            {
                return new UserModel { Response = new UserManagerResponseModel { IsSuccess = false, Errors = result.Errors.Select(e => e.Description) } };
            }

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

            return userObj;
        }

        public async Task<UserManagerResponseModel> ConfirmEmailAsync(string uid, string token)
        {
            UserManagerResponseModel response = new UserManagerResponseModel();
            var user = await _userManager.FindByIdAsync(uid);
            if (user == null)
            {
                response.IsSuccess = false;
                response.Message = "User not found";
                return response;
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                response.IsSuccess = false;
                response.Errors = result.Errors.Select(e => e.Description);
                return response;
            }
            response.IsSuccess = true;
            response.Message = "Email confirmed!";

            return response;
        }
    }
}
