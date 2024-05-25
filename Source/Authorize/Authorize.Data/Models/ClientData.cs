namespace BigGrayBison.Authorize.Data.Models
{
    public class ClientData : DataManagedStateBase
    {
        [ColumnMapping(IsPrimaryKey = true)] public Guid ClientId { get; set; }
        [ColumnMapping] public short Type { get; set; }
        [ColumnMapping] public Guid? SecretKey { get; set; }
        [ColumnMapping] public byte[] SecretSalt { get; set; }
        [ColumnMapping] public string Roles { get; set; }
        [ColumnMapping] public string RedirectionUrls { get; set; }
        [ColumnMapping] public bool IsActive { get; set; }
        [ColumnMapping(IsUtc = true)] public DateTime CreateTimestamp { get; set; }
        [ColumnMapping(IsUtc = true)] public DateTime UpdateTimestamp { get; set; }
    }
}
