using System;

namespace BigGrayBison.Authorize.Common.Exception
{
    public class InvalidUserException : ApplicationException
    {
        public InvalidUserException(string message)
            : base(message)
        { }
    }
}
