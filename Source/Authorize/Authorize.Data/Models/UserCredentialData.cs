namespace BigGrayBison.Authorize.Data.Models
{
    public class UserCredentialData : DataManagedStateBase
    {
        [ColumnMapping(IsPrimaryKey = true)] public Guid UserCredentialId { get; set; }
        [ColumnMapping] public Guid UserId { get; set; }
        [ColumnMapping] public Guid? MasterKey { get; set; }
        [ColumnMapping] public byte[] SecretSalt { get; set; }
        [ColumnMapping] public byte[] SecretKey { get; set; }
        [ColumnMapping] public byte[] Secret { get; set; }
        [ColumnMapping] public bool IsActive { get; set; }
        [ColumnMapping(IsUtc = true)] public DateTime? Expiration { get; set; }
        [ColumnMapping(IsUtc = true)] public DateTime CreateTimestamp { get; set; }
        [ColumnMapping(IsUtc = true)] public DateTime UpdateTimestamp { get; set; }
    }
}
