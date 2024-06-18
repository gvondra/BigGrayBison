namespace BigGrayBison.Authorize.Data
{
    public interface IUserDataFactory
    {
        Task<UserData> GetByName(ISqlSettings settings, string name);
        Task<bool> GetUserNameAvailable(ISqlSettings settings, string name);
    }
}
