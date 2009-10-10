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
            //NOTE: before running this sample, you must replace the values in the DemoConfigurationFile
            //with your s3 access details. You must also copy the contents of the configuration
            //to the location you have chosen in s3.

            //show user some command guidelines
            WriteLineBreak(); WriteLineBreak();
            Console.WriteLine("s3ToConfig Sample");
            Console.WriteLine("NOTE: before running this sample, you must replace the values in the DemoConfigurationFile with your s3 access details. You must also copy the contents of the configuration to the location you have chosen in s3.");
            
            

            //parse input from user until asked to quit
            bool keepLooping = true;
            while (keepLooping)
            {
                WriteLineBreak();
                Console.WriteLine("Commands (press Enter to exit):");
                ListCommands();
                Console.Write("Enter a command: ");
                string usrInput = Console.ReadLine().Trim();
                if (String.IsNullOrEmpty(usrInput))
                    keepLooping=false;
                else if (usrInput.ToLower()=="refreshconfig")
                {
                    SampleConfigSection.RefreshConfig();
                    Console.WriteLine("Config refreshed");
                }
                else if (usrInput.ToLower()=="readvalue")
                {
                    Console.WriteLine(String.Concat("Demo Setting Value: ",SampleConfigSection.Current.Settings.SampleConfigSetting));
                }
            }

            string demoSettingValueFound = SampleConfigSection.Current.Settings.SampleConfigSetting;
            Console.WriteLine(demoSettingValueFound);
            Console.ReadLine();
        }

        #region Console Helper Methods

        public static void WriteLineBreak()
        {
            Console.WriteLine("-----------------------------------------------------");
        }
        public static void ListCommands()
        {
            string[] commandList =
                {
                    "readvalue","refreshconfig"
                };
            foreach (string command in commandList)
            {
                Console.WriteLine(String.Concat(" -", command));
            }
        }
        public static bool GetDebugOptions()
        {
            return AskYesNoQuestion("Write out status of each action?");
        }
        public static int AskForInteger(string message)
        {
            int tryParse;
            if (Int32.TryParse(AskForInfo(message), out tryParse))
            {
                return tryParse;
            }
            else
            {
                Console.WriteLine("Error parsing text. Please enter a valid Integer.");
                //recursive
                return AskForInteger(message);
            }
        }
        public static String AskForInfo(string message)
        {
            Console.WriteLine();
            Console.Write(message);
            string result = Console.ReadLine();
            return result;
        }
        public static bool AskYesNoQuestion(string message)
        {
            Console.Write(String.Concat(message, " Press Y/N:"));
            ConsoleKeyInfo key = Console.ReadKey(false);
            Console.WriteLine(); return (key.Key == ConsoleKey.Y);

        }
        #endregion Console Helper Methods
    }
}
