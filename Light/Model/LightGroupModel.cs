using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Light
{
    public class LightGroupModel
    {
        public string Name { get; set; }
        public IEnumerable<LightValueModel> Template { get; set; }
    }
}
