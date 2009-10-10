using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Natol.S3ToConfig.SampleConsoleApplication.Config;

namespace Natol.S3ToConfig.SampleConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            string demoSettingValueFound = SampleConfigSection.Current.Settings.SampleConfigSetting;
            Console.WriteLine(demoSettingValueFound);
            Console.ReadLine();
        }
    }
}
