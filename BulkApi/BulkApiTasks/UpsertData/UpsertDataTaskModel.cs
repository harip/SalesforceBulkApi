using System;
using System.Collections.Generic;
using System.Xml;
using BulkApi.ApiWebClient;
using BulkApi.Authentication;
using BulkApi.Models;

namespace BulkApi.BulkApiTasks.UpsertData
{
    public class UpsertDataTaskModel
    {
        public IAuthenticationClient AuthenticationClient { get; set; }
        public Dictionary<String, String> Urls { get; set; }
        public List<Dictionary<String, String>> UploadData { get; set; }
        public XmlDocument BatchResults { get; set; }
        public List<BatchResult> BatchResultList { get; set; }
        public JobInfo JobInfo { get; set; }
        public IWebClient WebClient { get; set; }

        public JobObject JobObject { get; set; }
        public string ExternalIdFieldName { get; set; }

        public String UpsertError { get; set; }
    }
}
