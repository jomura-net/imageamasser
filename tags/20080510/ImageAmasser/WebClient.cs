using System;
using System.Collections.Generic;
using System.Text;

namespace ImageAmasser
{
    public class WebClient : System.Net.WebClient
    {
        protected override System.Net.WebRequest GetWebRequest(Uri address)
        {
            System.Net.WebRequest request = base.GetWebRequest(address);
            request.Timeout = 10000;
            return request;
        }
    }
}
