using System;
using System.Xml.Serialization;

namespace BulkApi.Models
{
    [Serializable]
    [XmlRoot(ElementName = "result-list", Namespace = "http://www.force.com/2009/06/asyncapi/dataload", IsNullable = true)]
    public class ResultList
    {
        [XmlElement("result")]
        public String Result { get; set; }
    }
}
