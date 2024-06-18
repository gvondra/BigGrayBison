using System;
using System.Threading.Tasks;

namespace BigGrayBison.Authorize.Framework
{
    public interface IUserCredentialUpdater
    {
        Task Update(ISettings settings, Guid userId, string password);
    }
}
