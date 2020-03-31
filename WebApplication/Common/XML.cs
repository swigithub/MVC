using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace SWI.Common
{
    public class XML
    {
        public string ToJson(string xmlString)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);

                return JsonConvert.SerializeXmlNode(doc);
            }
            catch(Exception ex)
            {

                throw ex;
            }

        }
    }
}