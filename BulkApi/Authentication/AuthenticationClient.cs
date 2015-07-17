using System;
using System.Collections.Generic;
using System.Text;
using BulkApi.ApiWebClient;
using BulkApi.Models;
using Newtonsoft.Json;

namespace BulkApi.Authentication
{
    public class AuthenticationClient:IAuthenticationClient, IDisposable
    {
        public string ApiVersion { get; set; }
        public AuthToken AuthToken { get; set; }
        public Dictionary<string, string> Urls { get; set; }
        private IWebClient _webClient;
        public IWebClient WebClient
        {
            get { return _webClient ?? (_webClient = new BulkApiWebClient() ); }
            set { _webClient = value; }
        }

        public AuthenticationClient()
        {
            ApiVersion = "v32.0";
        }

        public void UsernamePassword(string clientId, string clientSecret, string username, string password, string tokenRequestEndpointUrl)
        {
            if (string.IsNullOrEmpty(clientId)) throw new ArgumentNullException("clientId");
            if (string.IsNullOrEmpty(clientSecret)) throw new ArgumentNullException("clientSecret");
            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException("username");
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException("password");
            if (string.IsNullOrEmpty(tokenRequestEndpointUrl)) throw new ArgumentNullException("tokenRequestEndpointUrl");
            if (!Uri.IsWellFormedUriString(tokenRequestEndpointUrl, UriKind.Absolute)) throw new FormatException("tokenRequestEndpointUrl");

            var content = new System.Collections.Specialized.NameValueCollection
            {
                {"grant_type", "password"},
                {"client_id", clientId},
                {"client_secret", clientSecret},
                {"username", username},
                {"password", password}
            };

            AuthToken = new AuthToken();
            try
            {
                var responseBytes = WebClient.UploadValues(tokenRequestEndpointUrl, "POST", content);
                var responseBody = Encoding.UTF8.GetString(responseBytes);
                AuthToken = JsonConvert.DeserializeObject<AuthToken>(responseBody);
            }
            catch (Exception ex)
            {
                AuthToken.Errors = ex.Message;
            }
        }
        
        public void Dispose()
        {
            WebClient.Dispose();
        }
    }
}
