using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeAPI.Models
{
    public class UserModel : IdentityUser
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public byte[] ProfilePicture { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        [NotMapped]
        public string Token { get; set; }
        [NotMapped]
        public ResponseModel Response { get; set; }
    }
}
