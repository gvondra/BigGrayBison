using BigGrayBison.Authorize.Framework;
using System;
using System.Text.RegularExpressions;

namespace BigGrayBison.Authorize.Core
{
    public class UserValidator : IUserValidator
    {
        private const int _passwordMinLength = 12;

        public string ValidatePassword(string password)
        {
            string result = string.Empty;
            if (password.Length < _passwordMinLength)
                result = $"Password must be at least {_passwordMinLength} characters";
            else if (!Regex.IsMatch(password, @"[A-Z]{1}", RegexOptions.None, TimeSpan.FromMilliseconds(200)))
                result = "Password must contain upper case letters";
            else if (!Regex.IsMatch(password, @"[a-z]{1}", RegexOptions.None, TimeSpan.FromMilliseconds(200)))
                result = "Password must contain lower case letters";
            else if (!Regex.IsMatch(password, @"[0-9]{1}", RegexOptions.None, TimeSpan.FromMilliseconds(200)))
                result = "Password must contain numeric characters";
            else if (!Regex.IsMatch(password, @"[^0-9a-zA-Z]{1}", RegexOptions.None, TimeSpan.FromMilliseconds(200)))
                result = "Password must contain punctuation characters";
            return result;
        }

        public string ValidateUserName(string userName)
        {
            string result = string.Empty;
            if (!Regex.IsMatch(userName ?? string.Empty, @"^[0-9a-zA-Z][0-9a-zA-Z\-_+=]{2,254}[0-9a-zA-Z]$", RegexOptions.None, TimeSpan.FromMilliseconds(200)))
            {
                result = "Invalid user name";
            }
            return result;
        }
    }
}
