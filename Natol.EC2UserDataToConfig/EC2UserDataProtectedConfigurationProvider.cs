using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace Natol.EC2UserDataToConfig
{
    public class EC2UserDataProtectedConfigurationProvider : System.Configuration.ProtectedConfigurationProvider
    {

        const string REPLACEMENT_VALUE = "#USER_DATA#";

        public override System.Xml.XmlNode Decrypt(System.Xml.XmlNode encryptedNode)
        {
            string userData;
            var settingsNode = encryptedNode.SelectSingleNode("/EncryptedData/ec2UserDataProviderInfo");

            //get user-data string
            try
            {
                userData = new WebClientWithTimeout(6000).DownloadString("http://169.254.169.254/latest/user-data");
            }
            catch
            {
                //probably not in AWS EC2 environment
                userData = null;
            }

            //get configured location for replacement
            string configLocationFormat = settingsNode.Attributes["configLocationFormat"].Value;
            
            //make decision for intended xml file location by interrogating existing files
            string configXmlFileBasePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).Replace("file:\\", String.Empty);
            string configXmlFileFullPath = null;

            if (!String.IsNullOrWhiteSpace(userData))
            {
                //use file with replacement key, if available
                string destinationConfigLocation = Path.Combine(configXmlFileBasePath,
                                                            configLocationFormat.Replace(REPLACEMENT_VALUE, userData));
                if (File.Exists(destinationConfigLocation))
                    configXmlFileFullPath = destinationConfigLocation;
            }

            //no user-data or no file matching user-data?
            if (String.IsNullOrWhiteSpace(configXmlFileFullPath))
            {
                //use default file path
                string defaultUserData = settingsNode.Attributes["defaultUserData"].Value;
                //use file with replacement key, if available
                string destinationConfigLocation = Path.Combine(configXmlFileBasePath,
                                                            configLocationFormat.Replace(REPLACEMENT_VALUE, defaultUserData));
                if (File.Exists(destinationConfigLocation))
                    configXmlFileFullPath = destinationConfigLocation;
                else
                {
                    throw new ConfigurationErrorsException("Default file specified in ec2 user-data configuration provider was not found at: " + configXmlFileFullPath);
                }
            }
            

            //return the xml from intended file
            try
            {
                var configDoc = new XmlDocument();
                configDoc.Load(configXmlFileFullPath);
                //note: this code assumes configuration starts at node with index '1' since node at index '0' should be the xml declaration
                return configDoc.ChildNodes[1];
            }
            catch (Exception ex)
            {
                throw new ConfigurationErrorsException("Could not load file at path (which was 'decrypted' from ec2 user-data) as xml: " + configXmlFileFullPath);
            }
        }

        public override System.Xml.XmlNode Encrypt(System.Xml.XmlNode node)
        {
            throw new NotImplementedException();
        }
    }
}
