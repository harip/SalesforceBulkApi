using BulkApi.ApiWebClient;
using BulkApi.Models;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using BulkApi.Extensions;

namespace BulkApiTests
{
    public class TestWebClient : IWebClient
    {
        public string SalesforceReturnData { get; set; }
        public string UploadString(string address, string method, string data)
        {
            switch (address)
            {
                case "CreateBulkJob":
                    var jobInfo = new JobInfo { JobId = "JobId" };
                    return jobInfo.SerializeToSalesforceXml();
                case "UploadBatch":
                    var batchInfo = new BatchInfo { JobId = "JobId", BatchId = "BatchId" };
                    return batchInfo.SerializeToSalesforceXml();
            }
            return null;
        }
        public string DownloadString(string address)
        {
            switch (address)
            {
                case "CloseJob":
                    var jobInfo = new JobInfo
                    {
                        JobId = "JobId",
                        State = "Completed"
                    };
                    return jobInfo.SerializeToSalesforceXml();
                case "GetBatchResults":
                    return SalesforceReturnData;
            }
            return null;
        }
        public WebHeaderCollection Headers { get; set; }
        public Encoding Encoding { get; set; }
        public void Dispose() { }
        public byte[] UploadValues(string address, string method, NameValueCollection data)
        {
            const string responseData = "{\"id\":\"https://test.salesforce.com/id/nasa/nasa\",\"issued_at\":\"nasa\",\"token_type\":\"Bearer\",\"instance_url\":\"https://nasa.salesforce.com\",\"signature\":\"zZpWFdZDZ2YU/rCv14=\",\"access_token\":\"ThisIsATestToken\"}";
            var dataBytes = Encoding.UTF8.GetBytes(responseData);
            return dataBytes;
        }
    }
}
