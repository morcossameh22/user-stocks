using System.ComponentModel.DataAnnotations;

namespace Stocks.Core.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = CoreConstants.BlankPersonName)]
        public string PersonName { get; set; } = string.Empty;


        [Required(ErrorMessage = CoreConstants.BlankEmail)]
        [EmailAddress(ErrorMessage = CoreConstants.EmailFormat)]
        public string Email { get; set; } = string.Empty;


        [Required(ErrorMessage = CoreConstants.BlankPhoneNumber)]
        [RegularExpression("^[0-9]*$", ErrorMessage = CoreConstants.PhoneNumberFormat)]
        public string PhoneNumber { get; set; } = string.Empty;


        [Required(ErrorMessage = CoreConstants.BlankPassword)]
        public string Password { get; set; } = string.Empty;


        [Required(ErrorMessage = CoreConstants.BlankConfirmPassword)]
        [Compare("Password", ErrorMessage = CoreConstants.PasswordsMatch)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}

