using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace MVCProject.Common
{
    public class UtilXML
    {
        public static string ConvertListToXML<T>(IEnumerable<T> list)
        {
            string xmlString = null;
            XmlSerializer xmlSerializer = new XmlSerializer(list.GetType());
            using (MemoryStream memoryStream = new MemoryStream())
            {
                xmlSerializer.Serialize(memoryStream, list);
                memoryStream.Position = 0;
                xmlString = new StreamReader(memoryStream).ReadToEnd();
            }
            return xmlString;
        }
       
    }
}