using System;
using System.Threading.Tasks;

namespace BigGrayBison.Common.Core
{
    public interface ISettings
    {
        bool UseDefaultAzureSqlToken { get; }
        Task<string> GetConnetionString();
        Func<Task<string>> GetDatabaseAccessToken();
    }
}
