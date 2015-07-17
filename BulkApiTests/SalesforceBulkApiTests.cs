using System;
using System.Collections.Generic;
using BulkApi.Authentication;
using BulkApi.BulkApiTasks.UpsertData;
using BulkApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BulkApiTests
{
    [TestClass]
    public class SalesforceBulkApiTests
    {
        [TestMethod]
        public void BatchResultsXml_Are_Cast_to_BatchResults()
        {
            //Authenticate with salesforce
            var authentication = new AuthenticationClient
            {
                WebClient = new TestWebClient()
            };
            authentication.UsernamePassword("clientid", "secret", "username", "password", "http://authenticationTokenurl.com");

            //Data that needs to be sent
            var exportData = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    {"PersonEmail", "PersonEmail"}
                }
            };

            //Upsert task
            var taskModel = new UpsertDataTaskModel
            {
                AuthenticationClient = authentication,
                UploadData = exportData,
                Urls = new Dictionary<string, string>(),
                WebClient = new TestWebClient
                {
                    SalesforceReturnData = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><results xmlns=\"http://www.force.com/2009/06/asyncapi/dataload\"><result><id>AccountId1</id><success>true</success><created>false</created></result><result><errors><fields>Name</fields><message>Account: bad field names on insert/update call: Name</message><statusCode>INVALID_FIELD_FOR_INSERT_UPDATE</statusCode></errors><errors><fields>XXXXXX</fields><message>Account: bad field names on insert/update call: Name</message><statusCode>INVALID_FIELD_FOR_INSERT_UPDATE</statusCode></errors><success>false</success><created>false</created></result><result><id>AccountId3</id><success>true</success><created>false</created></result></results>"
                },
                JobObject = JobObject.Account,
                ExternalIdFieldName = "AppUserId__c"
            };

            //Urls
            taskModel.Urls["CreateBulkJob"] = "CreateBulkJob";
            taskModel.Urls["UploadBatch"] = "UploadBatch";
            taskModel.Urls["CloseJob"] = "CloseJob";
            taskModel.Urls["CheckBatch"] = "CheckBatch";
            taskModel.Urls["GetBatchResults"] = "GetBatchResults";

            //Execute task
            var task = new UpsertDataTask(taskModel);
            task.Invoke();

            Assert.IsTrue(taskModel.BatchResultList.Count == 3);
            Assert.IsTrue(taskModel.BatchResultList[0].Id == "AccountId1");
            Assert.IsTrue(taskModel.BatchResultList[0].Success);

            Assert.IsTrue(String.IsNullOrEmpty(taskModel.BatchResultList[1].Id));
            Assert.IsFalse(taskModel.BatchResultList[1].Success);

            Assert.IsTrue(taskModel.BatchResultList[2].Id == "AccountId3");
            Assert.IsTrue(taskModel.BatchResultList[2].Success);
        }
    }
}
