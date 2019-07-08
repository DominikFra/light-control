using System.Collections;
using System.Collections.Generic;

namespace LightHost
{
    public class ChannelValue
    {
        public int Channel { get; set; }
        public byte Value { get; set; }
    }
    public class LightEffect
    {
        public IEnumerable<ChannelValue> Values { get; set; }
        public string Name { get; set; }
    }
}