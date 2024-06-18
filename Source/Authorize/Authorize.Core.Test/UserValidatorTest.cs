using BigGrayBison.Authorize.Core;
using System;
using System.Text.RegularExpressions;

namespace Authorize.Core.Test
{
    [TestClass]
    public class UserValidatorTest
    {
        [TestMethod]
        [DataRow(@"A12B", @"^$")]
        [DataRow(@"A1B", @"^Invalid user name$")]
        [DataRow(@"-12B", @"^Invalid user name$")]
        [DataRow(@"_12B", @"^Invalid user name$")]
        [DataRow(@"+12B", @"^Invalid user name$")]
        [DataRow(@"=12B", @"^Invalid user name$")]
        [DataRow(@"A12-", @"^Invalid user name$")]
        [DataRow(@"A12_", @"^Invalid user name$")]
        [DataRow(@"A12+", @"^Invalid user name$")]
        [DataRow(@"A12=", @"^Invalid user name$")]
        [DataRow(@"A12!B", @"^Invalid user name$")]
        [DataRow(@"A12@B", @"^Invalid user name$")]
        [DataRow(@"A12#B", @"^Invalid user name$")]
        [DataRow(@"A12:B", @"^Invalid user name$")]
        [DataRow(@"A12;B", @"^Invalid user name$")]
        [DataRow(@"A12.B", @"^Invalid user name$")]
        [DataRow(@"A12,B", @"^Invalid user name$")]
        [DataRow(@"A12?B", @"^Invalid user name$")]
        [DataRow(@"", @"^Invalid user name$")]
        public void ValidateUserTest(string userName, string expectedPattern)
        {
            UserValidator userValidator = new UserValidator();
            string result = userValidator.ValidateUserName(userName);
            Assert.IsTrue(Regex.IsMatch(result, expectedPattern, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(200)));
        }

        [TestMethod]
        [DataRow(@"AAAbbb123!!!", @"^$")]
        [DataRow(@"", @"^Password must be at least \d+ characters$")]
        [DataRow(@"AAAbbb123!!", @"^Password must be at least \d+ characters$")]
        [DataRow(@"aaabbb123!!!", @"^Password must contain upper case letters$")]
        [DataRow(@"AAABBB123!!!", @"^Password must contain lower case letters$")]
        [DataRow(@"AAAbbbCCC!!!", @"^Password must contain numeric characters$")]
        [DataRow(@"AAAbbb123456", @"^Password must contain punctuation characters$")]
        public void ValidatePasswordTest(string password, string expectedPattern)
        {
            UserValidator userValidator = new UserValidator();
            string result = userValidator.ValidatePassword(password);
            Assert.IsTrue(Regex.IsMatch(result, expectedPattern, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(200)));
        }
    }
}
