namespace BigGrayBison.Authorize.Framework
{
    public interface ISettings : BigGrayBison.Common.Core.ISettings
    {
        string SigningKeyVaultAddress { get; }
    }
}
