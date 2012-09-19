using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Natol.EC2UserDataToConfig
{
    public class WebClientWithTimeout : WebClient
    {

        /// <summary>
        /// Time in milliseconds
        /// </summary>
        public int Timeout { get; set; }

        public WebClientWithTimeout()
        {
            this.Timeout = 60000;
        }

        public WebClientWithTimeout(int timeout)
        {
            this.Timeout = timeout;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var result = base.GetWebRequest(address);
            result.Timeout = this.Timeout;
            return result;
        }
    }
}
