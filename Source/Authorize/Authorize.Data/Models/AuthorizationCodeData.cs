namespace BigGrayBison.Authorize.Data.Models
{
    public class AuthorizationCodeData : DataManagedStateBase
    {
        [ColumnMapping(IsPrimaryKey = true)] public Guid AuthorizationCodeId { get; set; }
		[ColumnMapping] public Guid UserId { get; set; }
		[ColumnMapping] public Guid ClientId { get; set; }
		[ColumnMapping] public Guid KeyId { get; set; }
		[ColumnMapping] public byte[] Code { get; set; }
		[ColumnMapping] public string State { get; set; }
		[ColumnMapping(IsUtc = true)] public DateTime Expiration { get; set; }
		[ColumnMapping] public string RedirectUrl { get; set; }
		[ColumnMapping] public byte[] CodeChallenge { get; set; }
		[ColumnMapping] public short CodeChallengeMethod { get; set; }
		[ColumnMapping] public bool IsActive { get; set; }
		[ColumnMapping(IsUtc = true)] public DateTime CreateTimestamp { get; set; }
		[ColumnMapping(IsUtc = true)] public DateTime UpdateTimestamp { get; set; }
    }
}
