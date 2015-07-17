using System;
using System.Xml.Serialization;

namespace BulkApi.Models
{
    [Serializable]
    [XmlRoot(ElementName = "sObjects", Namespace = "http://www.force.com/2009/06/asyncapi/dataload", IsNullable = true)]
    public class BatchData
    {
    }
}
