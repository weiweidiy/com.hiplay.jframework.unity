using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace JFramework
{
    public class XmlParaser : IParaser
    {
        public IChainData Parase(string xmlString)
        {
            //XmlSerializer xmlSerializer = new XmlSerializer(,);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);
            //XmlElement element = xmlDoc[_rootName];
            XmlElement root = xmlDoc.DocumentElement;
            return new XmlChainData(root);
        }
    }
}
