using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace LightService.API
{
    [RoutePrefix("api/light")]
    public class LightController : ApiController
    {
        LightControl light = new LightControl(s => { });
        [HttpPost, Route("setChannelValue")]
        public IEnumerable<byte> setChannelValue(int channel, byte value)
        {
            light.SetChannelValue(channel, value);
            return light.Current();
        }

        [HttpGet, Route("echo/{text}")]
        public string EchoText(string text)
        {
            return "This is a Test for: " + text;
        }
        [HttpGet, Route("test")]
        public string GetTest()
        {
            return "This is a Test";
        }
    }
}
