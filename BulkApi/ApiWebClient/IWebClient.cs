using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace BulkApi.ApiWebClient
{
    public interface IWebClient : IDisposable
    {
        string UploadString(string address, string method, string data);
        string DownloadString(string address);
        byte[] UploadValues(string address, string method, NameValueCollection data);
        WebHeaderCollection Headers { get; set; }
        Encoding Encoding { get; set; }
    }
}
