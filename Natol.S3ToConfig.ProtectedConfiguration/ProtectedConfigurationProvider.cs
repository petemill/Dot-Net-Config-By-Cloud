using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Natol.S3ToConfig.ProtectedConfiguration
{
    public class ProtectedConfigurationProvider : System.Configuration.ProtectedConfigurationProvider
    {

        public override System.Xml.XmlNode Decrypt(System.Xml.XmlNode encryptedNode)
        {
            string xmlRaw =
                "<sampleConfig><settings sampleConfigSetting=\"Sucess. This Setting came from code.\"></settings></sampleConfig>";

            var doc = new XmlDocument();
            doc.LoadXml(xmlRaw);
            return doc.ChildNodes[0];
        }

        public override System.Xml.XmlNode Encrypt(System.Xml.XmlNode node)
        {
            throw new NotImplementedException();
        }
    }
}
