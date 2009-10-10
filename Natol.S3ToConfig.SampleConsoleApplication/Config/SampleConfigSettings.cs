using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Natol.S3ToConfig.SampleConsoleApplication.Config
{
    public class SampleConfigSettings : ConfigurationElement
    {
        /// <summary>
        /// Our first sample settings - a string
        /// </summary>
        [ConfigurationProperty("sampleConfigSetting", DefaultValue = @"")]
        public string SampleConfigSetting
        {
            get
            {
                return this["sampleConfigSetting"].ToString();
            }
        }
    }
}
