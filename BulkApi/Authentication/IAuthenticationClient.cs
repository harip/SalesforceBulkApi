using BulkApi.Models;

namespace BulkApi.Authentication
{
    public interface IAuthenticationClient
    {
        AuthToken AuthToken { get; set; }
        string ApiVersion { get; set; }
        void UsernamePassword(string clientId, string clientSecret, string username, string password, string tokenRequestEndpointUrl);
        void Dispose();
    }
}
