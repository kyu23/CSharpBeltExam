using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace RegistrationLogin.Models
{
    public class StrongPasswordAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var reg = new Regex(@"^(?=.*?\d)(?=.*?[A-Z])(?=.*?[a-z])[A-Za-z\d,!@#$%^&*+=]{8,}$");
            if (!reg.IsMatch((string)value))
                return new ValidationResult("Password must have at least one special and regular character.");
            return ValidationResult.Success;
        }
    }
}