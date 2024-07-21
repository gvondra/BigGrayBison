using System;

namespace BigGrayBison.Authorize.Framework.Enumerations
{
    [Flags]
    public enum UserRole : short
    {
        None = 0,
        UserEditor = 1
    }
}
