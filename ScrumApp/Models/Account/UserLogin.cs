using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumApp.Models.Account
{
    public class UserLogin
    {
        [Required]
        public string UserName { get; set; }


        [DataType(DataType.Password), Required]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}

