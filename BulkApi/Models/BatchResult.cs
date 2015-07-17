using System;
using System.Xml.Serialization;
namespace BulkApi.Models
{
    [Serializable]
    [XmlRoot(ElementName = "result", IsNullable = true)]
    public class BatchResult
    {
        [XmlElement("id")]
        public string Id { get; set; }

        [XmlElement("errors")]
        public string Errors { get; set; }

        [XmlElement("success")]
        public bool Success { get; set; }

        [XmlElement("created")]
        public bool Created { get; set; }
    }
}
