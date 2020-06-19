using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistrationLogin.Models
{
    public class LoginUser
    {
        [Required(ErrorMessage="Email is required!")]
        [EmailAddress(ErrorMessage="Please enter a valid email address")]
        public string LoginEmail {get;set;}

        
        [Required(ErrorMessage="Password is required")]
        [MinLength(8, ErrorMessage="Password must be 8 characters or longer")]
        [DataType("Password")]
        public string LoginPassword { get; set; }
    }
}