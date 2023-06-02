using System.ComponentModel.DataAnnotations;

namespace Stocks.Core.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = CoreConstants.BlankEmail)]
        [EmailAddress(ErrorMessage = CoreConstants.EmailFormat)]
        public string Email { get; set; } = string.Empty;


        [Required(ErrorMessage = CoreConstants.BlankPassword)]
        public string Password { get; set; } = string.Empty;
    }
}

