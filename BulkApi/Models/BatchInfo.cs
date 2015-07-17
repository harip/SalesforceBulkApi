using System;
using System.Xml.Serialization;

namespace BulkApi.Models
{
    [Serializable]
    [XmlRoot(ElementName = "batchInfo", Namespace = "http://www.force.com/2009/06/asyncapi/dataload", IsNullable = true)]
    public class BatchInfo
    {
        [XmlElement("id")]
        public String BatchId { get; set; }
        
        [XmlElement("jobId")]
        public String JobId { get; set; }

        [XmlElement("state")]
        public String State { get; set; }

        [XmlElement("numberRecordsProcessed")]
        public int NumberRecordsProcessed { get; set; }

        [XmlElement("numberRecordsFailed")]
        public int NumberRecordsFailed { get; set; }
    }
}
