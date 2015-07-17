using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using BulkApi.BulkApi;
using BulkApi.Extensions;
using BulkApi.Models;

namespace BulkApi.BulkApiTasks.UpsertData
{
    public class UpsertDataTask
    {
        private readonly BulkApiCalls _bulkApiCalls;
        private readonly UpsertDataTaskModel _taskModel;
        public UpsertDataTask(UpsertDataTaskModel model)
        {
            _taskModel = model;
            _bulkApiCalls = new BulkApiCalls
            {
                AuthenticationClient = model.AuthenticationClient,
                Urls = model.Urls,
                WebClient = model.WebClient,
            };
        }

        public void Invoke()
        {
            try
            {
                //Create job
                var bulkJob = _bulkApiCalls.CreateBulkUpsertJob(_taskModel.JobObject,_taskModel.ExternalIdFieldName);

                //Submit batch
                var bulkDataXml = BulkApiDataHelper.GetBulkDataXml(_taskModel.UploadData);
                var batchInfo = _bulkApiCalls.SubmitBatch(bulkJob.JobId, bulkDataXml);

                //Close job
                var closeJob = _bulkApiCalls.CloseJob(bulkJob.JobId);

                //Wait until job finished
                bulkJob = _bulkApiCalls.GetCompletedJob(bulkJob.JobId);

                //Get batch results
                var batchResults = _bulkApiCalls.GetBatchResultsXml(bulkJob.JobId, batchInfo.BatchId);
                
                _taskModel.JobInfo = bulkJob;
                _taskModel.BatchResults = batchResults;
                GetBatchResultList(_taskModel);
            }
            catch (Exception ex)
            {
                _taskModel.JobInfo = null;
                _taskModel.BatchResults = null;
                _taskModel.UpsertError = ex.Message;
            }
        }

        private void GetBatchResultList(UpsertDataTaskModel msg)
        {
            try
            {
                var parent = msg.BatchResults.GetElementsByTagName("results")[0];
                var batchData = parent.ChildNodes.Cast<XmlElement>()
                    .Select(s => s.OuterXml.Replace(s.NamespaceURI, "").DeSerializeXmlToClass<BatchResult>())
                    .ToList();
                msg.BatchResultList = batchData;
            }
            catch (Exception ex)
            {
                msg.BatchResultList = new List<BatchResult>();
                msg.UpsertError = "Failed to return or process batch results job";
            }
        } 
    }
}
