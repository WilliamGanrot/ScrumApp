using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumApp.Models.Account
{
    public class UserRegister
    {
        [Required]
        public string UserName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DataType(DataType.Password), Required]
        public string Password { get; set; }

        [NotMapped]
        [DataType(DataType.Password), Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !"), Required]
        public string ConfirmPassword { get; set; }

        public UserRegister()
        {

        }

        public UserRegister(AppUser appUser)
        {
            UserName = appUser.UserName;
            Email = appUser.Email;
            Password = appUser.PasswordHash;
        }
    }
}
