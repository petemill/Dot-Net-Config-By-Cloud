This project contains libraries that enable .Net Configuration Sections to be loaded from sources relevant to services provided in the Amazon Web Services platform.

This project was inspired by the following article:
[http://www.wrox.com/WileyCDA/Section/Redirecting-Configuration-with-a-Custom-Provider.id-291932.html]

...and a desire for secure centralised configuration management in .net!


## Load any ConfigurationSection from any file on **S3**
- Add the Natol.S3ToConfig.ProtectedConfiguration.dll file to your project's bin folder
- Define the custom configuration provider in your app.config or web.config *after* the _configSections_ element:
    <configProtectedData defaultProvider="s3ConfigSectionProvider"><providers><add name="s3ConfigSectionProvider" type="Natol.S3ToConfig.ProtectedConfiguration.ProtectedConfigurationProvider, Natol.S3ToConfig.ProtectedConfiguration"   /></providers> </configProtectedData>
- Add a file to Amazon S3 at your desired location, giving it a key that represents the function of both your app and the context it runs in (remembering amazon s3 bucket names are globally unique). In this file, put the contents of your configuration section, eg:
    <sampleConfig><settings sampleConfigSetting="This Setting came from s3"></settings></sampleConfig>
- Replace the contents of your static configuration section with an _EncyptedData_ element containing the location of your new configuration object in Amazon S3 and let it know our custom provider should handle the 'decryption', eg:
    <sampleConfig configProtectionProvider="s3ConfigSectionProvider"> <EncryptedData> <s3ProviderInfo  s3AccessKey="REPLACE_WITH_YOUR_VALUE"  s3SecretKey="REPLACE_WITH_YOUR_VALUE"  s3BucketName="REPLACE_WITH_YOUR_VALUE"  objectKey="test-s3toconfig-sampleconsoleapplication-sampleconfig" /></EncryptedData> </sampleConfig>
- *That's it!* Your configuration section will work exactly as normal, even calling ConfigurationManager.RefreshSection() which will then reload from S3... :-) Seriously - try the demo

## Load a different file's ConfigurationSection depending on the **UserData of an EC2 instance**
*This can be used to store an app on an EC2 AMI image, but have it load different sets of configuration depending on the user-data string passed through at instance-creation time*
- Add the Natol.EC2UserData.dll file to your project's bin folder
- Define the custom configuration provider in your app.config or web.config *after* the _configSections_ element:
    <!-- defining our custom configuration provider -->
    <configProtectedData defaultProvider="s3ConfigSectionProvider">
        <providers>
            <add name="ec2UserDataConfigSectionProvider" type="Natol.EC2UserDataToConfig.EC2UserDataProtectedConfigurationProvider, Natol.EC2UserDataToConfig"/>
        </providers>
    </configProtectedData>
- Setup the configuration including the string format which contains the text `#USER_DATA#` - **this text will be replaced with the instance's user-data string**:
    <EncryptedData>
        <ec2UserDataProviderInfo 
            defaultUserData="default"
            configLocationFormat="_ConfigurationContent\DemoConfiguration_LoadedFromEC2UserData_#USER_DATA#.config"
        />
    </EncryptedData>
- Create a selection of files with names according to your filename schema defined above, and a default one too just in case.

###Thanks
This project uses the LitS3 library to retreive objects from Amazon's S3 Web Service, which has its home at [http://code.google.com/p/lits3/].


###Limitations
Modifying your configsection and attempting to persist it back to s3 is not currently supported, though this is conceptually possible. Please contribute!
