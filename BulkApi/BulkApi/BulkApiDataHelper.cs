using System;
using System.Collections.Generic;
using System.Xml;
using BulkApi.Extensions;
using BulkApi.Models;

namespace BulkApi.BulkApi
{
    public class BulkApiDataHelper
    {
        public static string GetBulkDataXml(List<Dictionary<string, string>> mappings)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(new BatchData().SerializeToSalesforceXml());
            var parent = xmlDocument.GetElementsByTagName("sObjects")[0];
            foreach (var data in mappings)
            {
                var report = xmlDocument.CreateElement("sObject", "http://www.force.com/2009/06/asyncapi/dataload");
                foreach (var key in data.Keys)
                {
                    AppendChild(xmlDocument, report, key, data[key]);
                }
                parent.AppendChild(report);
            }
            var x = xmlDocument.SerializeToSalesforceXml();
            return x;
        }
        private static void AppendChild(XmlDocument xmlDocument, XmlElement parent, string newChild, string childText)
        {
            var child = xmlDocument.CreateElement(newChild, "http://www.force.com/2009/06/asyncapi/dataload");
            if (String.IsNullOrEmpty(childText))
            {
                var xsinil = xmlDocument.CreateAttribute("xsi", "nil","http://www.w3.org/2001/XMLSchema-instance");
                xsinil.Value = "true";
                child.SetAttributeNode(xsinil);
            }
            else
            {
                child.InnerText = childText;
            }
            parent.AppendChild(child);
        }
    }
}
