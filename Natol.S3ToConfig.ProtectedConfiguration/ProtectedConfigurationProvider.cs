using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using LitS3;

namespace Natol.S3ToConfig.ProtectedConfiguration
{
    public class ProtectedConfigurationProvider : System.Configuration.ProtectedConfigurationProvider
    {

        public override System.Xml.XmlNode Decrypt(System.Xml.XmlNode encryptedNode)
        {
            //note: in order to verify the protected configuration route without going via s3, use the following line
            //string xmlRaw = "<sampleConfig><settings sampleConfigSetting=\"Sucess. This Setting came from code.\"></settings></sampleConfig>";
            
            //setup parameters we need to know (note: this could be extended to be further provider-based)
            string awsAccessKey, awsSecretKey, bucketName, objectKey;

            //collect parameter values
            XmlNode settingsNode = encryptedNode.SelectSingleNode("/EncryptedData/s3ProviderInfo");
            awsAccessKey = settingsNode.Attributes["s3AccessKey"].Value;
            awsSecretKey = settingsNode.Attributes["s3SecretKey"].Value;
            bucketName = settingsNode.Attributes["s3BucketName"].Value;
            objectKey = settingsNode.Attributes["objectKey"].Value;

            //get value from s3
            var service = new S3Service
            {
                AccessKeyID = awsAccessKey,
                SecretAccessKey = awsSecretKey,
                UseSsl = true,
                UseSubdomains = true
            };
            string xmlRaw = service.GetObjectString(bucketName, objectKey);
            
            //cast to XmlDocument
            var doc = new XmlDocument();
            doc.LoadXml(xmlRaw);

            //return node
            return doc.ChildNodes[0];
        }


        /// <summary>
        /// This method is not implemented due to the neccessary inclusion of parameters (eg: where to store the information), which we would not have access to
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public override System.Xml.XmlNode Encrypt(System.Xml.XmlNode node)
        {
            //note: this is not implemented because we would not know where to store the info
            //however, this could be implemented based on it's own configured parameters in it's own
            //configuration section, but we would still have the problem of not knowing which key of s3 settings
            //we belonged to. Thankfully, self-writing back to app-configuration (instead of user-configuration) is not something that is often done.
            throw new NotImplementedException();
        }
    }
}
