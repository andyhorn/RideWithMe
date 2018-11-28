using System.Collections.Generic;

namespace WebApiTest
{
    public class WebResponse
    {
        public string Message { get; set; }
        public Dictionary<string, string> Data { get; }

        public WebResponse()
        {
            Data = new Dictionary<string, string>();
        }
    }
}