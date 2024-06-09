using BigGrayBison.Authorize.Data.Models;
using BigGrayBison.Authorize.Framework;
using System;

namespace BigGrayBison.Authorize.Core
{
    public class User : IUser
    {
        private readonly UserData _data;

        public User(UserData data)
        {
            _data = data;
        }

        public Guid UserId => _data.UserId;

        public string Name => _data.Name;

        public bool IsActive { get => _data.IsActive; set => _data.IsActive = value; }

        public DateTime CreateTimestamp => _data.CreateTimestamp;

        public DateTime UpdateTimestamp => _data.UpdateTimestamp;
    }
}
