using System;
using System.Xml.Serialization;

namespace BulkApi.Models
{
    [Serializable]
    [XmlRoot(ElementName = "jobInfo", Namespace = "http://www.force.com/2009/06/asyncapi/dataload", IsNullable = true)]
    public class JobInfo
    {
        [XmlElement("id")]
        public string JobId { get; set; }

        [XmlElement("operation")]
        public Operation? Operation { get; set; }

        [XmlElement("object")]
        public JobObject? JobObject { get; set; }

        [XmlElement("state")]
        public String State { get; set; }
        
        [XmlElement("externalIdFieldName")]
        public String ExternalIdFieldName { get; set; }

        [XmlElement("contentType")]
        public ContentType? ContentType { get; set; }
        
        [XmlElement("numberBatchesQueued")]
        public int NumberBatchesQueued { get; set; }

        [XmlElement("numberBatchesInProgress")]
        public int NumberBatchesInProgress { get; set; }

        [XmlElement("numberBatchesCompleted")]
        public int NumberBatchesCompleted { get; set; }

        [XmlElement("numberBatchesFailed")]
        public int NumberBatchesFailed { get; set; }

        [XmlElement("numberBatchesTotal")]
        public int NumberBatchesTotal { get; set; }

        [XmlElement("numberRecordsProcessed")]
        public int NumberRecordsProcessed { get; set; }

        [XmlElement("numberRecordsFailed")]
        public int NumberRecordsFailed { get; set; }

        public bool JobCompleted
        {
            get
            {
                if (State == "Aborted" || State == "Completed")
                {
                    return true;
                }
                if (NumberBatchesTotal == NumberBatchesFailed + NumberBatchesCompleted)
                {
                    return true;
                }
                return false;
            }
        }
    }

    public enum JobObject
    {
        None=0,
        Account = 1,
        Opportunity = 2
    }

    public enum ContentType
    {
        None = 0,
        CSV = 1,
        XML = 2
    }

    public enum Operation
    {
        None = 0,
        upsert = 1,
    }
}
