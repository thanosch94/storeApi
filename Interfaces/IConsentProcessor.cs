namespace StoreApi.Interfaces
{
    public interface IConsentProcessor
    {
        Guid CreateConsentCookie(string ip, string userAgent);

    }
}
