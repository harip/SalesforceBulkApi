using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;
using BulkApi.ApiWebClient;
using BulkApi.Authentication;
using BulkApi.Extensions;
using BulkApi.Models;

namespace BulkApi.BulkApi
{
    public class BulkApiCalls : IBulkApiCalls
    {
        public IAuthenticationClient AuthenticationClient { get; set; }
        public Dictionary<String, String> Urls { get; set; }
        private IWebClient _webClient;
        public IWebClient WebClient
        {
            get
            {
                if (_webClient == null)
                {
                    _webClient = new BulkApiWebClient { Encoding = Encoding.UTF8 };
                    _webClient.Headers.Add("X-SFDC-Session: " + AuthenticationClient.AuthToken.AccessToken);
                    _webClient.Headers.Add("Content-Type: application/xml");
                }
                return _webClient;
            }
            set { _webClient = value; }
        }

        public JobInfo CreateBulkUpsertJob(JobObject jobObject, string externalIdFieldName)
        {
            var jobInfo = new JobInfo
            {
                ContentType = ContentType.XML, 
                Operation = Operation.upsert,
                JobObject = jobObject,
                ExternalIdFieldName = externalIdFieldName
            };
            var jobResponse = CreateJob(jobInfo);
            return jobResponse;
        }
        public string CloseJob(string jobId)
        {
            var jobInfo = new JobInfo {State = "Closed"};
            var submitBatchUrl = String.Format(Urls["CloseJob"], AuthenticationClient.AuthToken.InstanceUrl, jobId);
            var jobResponse = BulkUploadDataToSalesforce(submitBatchUrl, jobInfo.SerializeToSalesforceXml());
            return jobResponse;
        }
        public JobInfo GetCompletedJob(string jobId)
        {
            var job = GetJob(jobId);
            while (!job.JobCompleted)
            {
                Thread.Sleep(3000);
                job = GetJob(jobId);
            }
            return job;
        }

        public BatchInfo SubmitBatch(string jobId, string batchXmlString)
        {
            var submitBatchUrl = String.Format(Urls["UploadBatch"], AuthenticationClient.AuthToken.InstanceUrl, jobId);
            var jobResponse = BulkUploadDataToSalesforce(submitBatchUrl, batchXmlString);
            var batchInfo = jobResponse.DeSerializeToSalesforceObject<BatchInfo>();
            return batchInfo;
        }
        public string GetBatchResults(string jobId,string batchId)
        {
            var resultUrl = string.Format(Urls["GetBatchResults"], AuthenticationClient.AuthToken.InstanceUrl, jobId, batchId);
            var jobResponse = RequestDataFromSalesforce(resultUrl);
            return jobResponse;
        }
        public XmlDocument GetBatchResultsXml(string jobId, string batchId)
        {
            var jobResponse = GetBatchResults(jobId, batchId);
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(jobResponse);
            return xmlDocument;
        }

        private JobInfo CreateJob(JobInfo jobInfo)
        {
            var createJobUrl = String.Format(Urls["CreateBulkJob"], AuthenticationClient.AuthToken.InstanceUrl);
            var xmlString = jobInfo.SerializeToSalesforceXml();
            var jobResponse = BulkUploadDataToSalesforce(createJobUrl, xmlString);
            return jobResponse.DeSerializeToSalesforceObject<JobInfo>();
        }
        private JobInfo GetJob(string jobId)
        {
            var createJobUrl = String.Format(Urls["CloseJob"], AuthenticationClient.AuthToken.InstanceUrl, jobId);
            var jobResponse = RequestDataFromSalesforce(createJobUrl);
            var jobInfo = jobResponse.DeSerializeToSalesforceObject<JobInfo>();
            return jobInfo;
        }

        private String BulkUploadDataToSalesforce(string restEndPoint,string jobRequest)
        {
            try
            {
                return WebClient.UploadString(restEndPoint, "Post", jobRequest);
            }
            catch (WebException webEx)
            {
                String error = String.Empty;
                if (webEx.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)webEx.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            error = reader.ReadToEnd();
                        }
                    }
                }
                throw;
            }
        }
        private String RequestDataFromSalesforce(string restEndPoint)
        {
            try
            {
                return WebClient.DownloadString(restEndPoint);
            }
            catch (WebException webEx)
            {
                String error = String.Empty;
                if (webEx.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)webEx.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            error = reader.ReadToEnd();
                        }
                    }
                }
                throw;
            }
        }
    }
}