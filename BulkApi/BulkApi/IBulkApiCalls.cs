using System.Collections.Generic;
using System.Xml;
using BulkApi.ApiWebClient;
using BulkApi.Authentication;
using BulkApi.Models;

namespace BulkApi.BulkApi
{
    public interface IBulkApiCalls
    {
        IAuthenticationClient AuthenticationClient { get; set; }
        Dictionary<string, string> Urls { get; set; }
        IWebClient WebClient { get; set; }
        JobInfo CreateBulkUpsertJob(JobObject jobObject, string externalIdFieldName);
        string CloseJob(string jobId);
        JobInfo GetCompletedJob(string jobId);
        BatchInfo SubmitBatch(string jobId, string batchXmlString);
        string GetBatchResults(string jobId,string batchId);
        XmlDocument GetBatchResultsXml(string jobId, string batchId);
    }
}