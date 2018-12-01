using System.Collections.Generic;

namespace RideWithMeWebApp.Api
{
    public class WebResponse
    {
        public string Message { get; set; }
        public Dictionary<string, object> Data { get; }

        public WebResponse()
        {
            Data = new Dictionary<string, object>();
        }
    }
}