using System;

namespace BigGrayBison.Authorize.Framework
{
    public interface IUser
    {
        Guid UserId { get; }
        string Name { get; }
        bool IsActive { get; set; }
        DateTime CreateTimestamp { get; }
        DateTime UpdateTimestamp { get; }
    }
}
