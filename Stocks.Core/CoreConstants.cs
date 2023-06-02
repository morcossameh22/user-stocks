using System;
namespace Stocks.Core
{
	public static class CoreConstants
	{
        // Stock Constants
		public const string BlankStockSymbol = "Stock Symbol can't be blank";

        // User Constants
		public const string BlankEmail = "Email can't be blank";
        public const string EmailFormat = "Email should be in a proper email address format";

        public const string BlankPersonName = "Person Name can't be blank";

        public const string BlankPhoneNumber = "Phone number can't be blank";
        public const string PhoneNumberFormat = "Phone number should contain digits only";

        public const string BlankPassword = "Password can't be blank";
        public const string BlankConfirmPassword = "Confirm Password can't be blank";
        public const string PasswordsMatch = "Password and confirm password do not match";

        // Exception Constants
        public const string NotFoundMessage = "Not found";
        public const string InvalidToken = "Invalid token";
        public const string UserNotFound = "User not found";
        public const string InvalidEmailPass = "Invalid email or password";
        public const string InvalidClientReq = "Invalid client request";
        public const string InvalidAccessToken = "Invalid JWT access token";
        public const string InvalidRefreshToken = "Invalid refresh token";
    }
}

