namespace BigGrayBison.Authorize.Data.Models
{
    public class UserData : DataManagedStateBase
    {
        [ColumnMapping(IsPrimaryKey = true)] public Guid UserId { get; set; }
		[ColumnMapping] public string Name { get; set; }
		[ColumnMapping] public Guid EmailAddressId { get; set; }
		[ColumnMapping] public bool IsActive { get; set; }
		[ColumnMapping] public int Roles { get; set; }
		[ColumnMapping(IsUtc = true)] public DateTime CreateTimestamp { get; set; }
		[ColumnMapping(IsUtc = true)] public DateTime UpdateTimestamp { get; set; }
    }
}
