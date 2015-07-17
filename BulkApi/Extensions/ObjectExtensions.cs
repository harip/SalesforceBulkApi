using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace BulkApi.Extensions
{
    public static class ObjectExtensions
    {
        public static string SerializeToSalesforceXml(this object toXmlObject)
        {
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "http://www.force.com/2009/06/asyncapi/dataload");

            var serializer = new XmlSerializer(toXmlObject.GetType());
            string utf8;
            using (StringWriter writer = new Utf8StringWriter())
            {
                serializer.Serialize(writer, toXmlObject);
                utf8 = writer.ToString();
            }
            return utf8;
        }

        public static string SerializeToSalesforceXml(this XmlDocument xmlDocument)
        {
            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter))
            {
                xmlDocument.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                return stringWriter.GetStringBuilder().ToString();
            }
        }

        public static T DeSerializeToSalesforceObject<T>(this String xmlString)
        {
            TextReader reader = null;
            try
            {
                reader = new StringReader(xmlString);
                var serializer = new XmlSerializer(typeof(T));
                var result = (T)serializer.Deserialize(reader);
                return result;
            }
            catch (Exception ex)
            {
                return default(T);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public static T DeSerializeXmlToClass<T>(this String xmlString)
        {
            TextReader reader = null;
            try
            {
                reader = new StringReader(xmlString);
                var serializer = new XmlSerializer(typeof(T));
                var result = (T)serializer.Deserialize(reader);
                return result;
            }
            catch (Exception ex)
            {
                return default(T);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }
    }
}
