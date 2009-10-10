Project Home: http://s3toconfig.codeplex.com
Release: 1.0

How to convert any .net configuration section to load from Amazon S3:

   1. Add the Natol.S3ToConfig.ProtectedConfiguration.dll file to your projects bin folder, or the GAC on the machine
   2. Define the custom configuration provider in your app.config or web.config after the configSections element:
         1. <configProtectedData defaultProvider="SampleProvider"><providers><add name="SampleProvider" type="Natol.S3ToConfig.ProtectedConfiguration.ProtectedConfigurationProvider, Natol.S3ToConfig.ProtectedConfiguration" /></providers> </configProtectedData>
   3. Add a file to Amazon S3 at your desired location, giving it a key that represents the function of both your app and the context it runs in (remembering amazon s3 bucket names are globally unique). In this file, put the contents of your configuration section, eg:
         1. <sampleConfig><settings sampleConfigSetting="This Setting came from s3"></settings></sampleConfig>
   4. Replace the contents of your static configuration section with an EncyptedData element containing the location of your new configuration object in Amazon S3 and let it know our custom provider should handle the 'decryption', eg:
         1. <sampleConfig configProtectionProvider="s3ConfigSectionProvider"> <EncryptedData> <s3ProviderInfo s3AccessKey="REPLACEWITHYOURVALUE" s3SecretKey="REPLACEWITHYOURVALUE" s3BucketName="REPLACEWITHYOUR_VALUE" objectKey="test-s3toconfig-sampleconsoleapplication-sampleconfig" /></EncryptedData> </sampleConfig>
         2. That's it. Your configuration section will work exactly as normal, even calling ConfigurationManager.RefreshSection() which will then reload from S3... :-) Seriously - try the demo
         
Thanks
This project uses the amazing LitS3 library to retreive objects from Amazon's S3 Web Service, which has its home at http://code.google.com/p/lits3/.

Limitations
Modifying your configsection and attempting to persist it back to s3 is not currently supported, though this is conceptually possible. Please contribute!

Suggestions

    * Use the excellent and free Cloudberry Explorer application to manage / upload to Amazon S3 - http://cloudberrylab.com/
    * Use Amazon Web Services for your hosting / server environment - it's incredible! - http://aws.amazon.com/
    * Set up a separate AWS account to the one you use to manage your main services, and grant it access to these buckets. Then, if the data you are storing in your local configuration files falls in to the wrong hands, just change the access keys on that account, or even delete the account and create a new one.