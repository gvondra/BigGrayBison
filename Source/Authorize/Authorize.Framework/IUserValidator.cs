namespace BigGrayBison.Authorize.Framework
{
    public interface IUserValidator
    {
        // returns empty string if the user name is valid
        string ValidateUserName(string userName);
        // returns empty string if the password is valid
        string ValidatePassword(string password);
    }
}
