using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Natol.S3ToConfig.SampleConsoleApplication.Config
{
    public class SampleConfigSection : ConfigurationSection
    {
        #region static factory
        
        /// <summary>
        /// Statically-stored strongly-typed configuration section
        /// </summary>
        private static SampleConfigSection _current = null;

        /// <summary>
        /// Retreive and statically cache this section from configuration
        /// </summary>
        public static SampleConfigSection Current
        {
            get
            {
                if (_current == null)
                   return (SampleConfigSection)ConfigurationManager.GetSection("sampleConfig");
                return _current;
            }
        }
        
        /// <summary>
        /// Refresh the statically cached section
        /// </summary>
        public static void RefreshConfig()
        {
            ConfigurationManager.RefreshSection("sampleConfig");
            _current = null;
        }
        
        #endregion static factory

        /// <summary>
        /// A collection of standard settings for this app. This is to demonstrate the types of settings that can be retreived via our protected configuration provider.
        /// </summary>
        [ConfigurationProperty("settings")]
        public SampleConfigSettings Settings
        {
            get
            {
                return (SampleConfigSettings)this["settings"];
            }
        }
    }
}
