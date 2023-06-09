using System.ComponentModel.DataAnnotations;

namespace Stocks.Core.DTO
{
    /* The LoginDTO class contains properties for email and password, with validation attributes for
    required fields and email format. */
    public class LoginDTO
    {
        [Required(ErrorMessage = CoreConstants.BlankEmail)]
        [EmailAddress(ErrorMessage = CoreConstants.EmailFormat)]
        public string Email { get; set; } = string.Empty;


        [Required(ErrorMessage = CoreConstants.BlankPassword)]
        public string Password { get; set; } = string.Empty;
    }
}

