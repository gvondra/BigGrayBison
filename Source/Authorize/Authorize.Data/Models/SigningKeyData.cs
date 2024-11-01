﻿namespace BigGrayBison.Authorize.Data.Models
{
    public class SigningKeyData : DataManagedStateBase
    {
        [ColumnMapping(IsPrimaryKey = true)] public Guid SigningKeyId { get; set; }
        [ColumnMapping] public Guid KeyVaultKey { get; set; }
        [ColumnMapping] public bool IsActive { get; set; } = true;
        [ColumnMapping(IsUtc = true)] public DateTime CreateTimestamp { get; set; }
        [ColumnMapping(IsUtc = true)] public DateTime UpdateTimestamp { get; set; }
    }
}
