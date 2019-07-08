using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Light
{
    public class ConfigModel
    {
        public List<LightGroupModel> LightGroups { get; set; }
        public List<EffectModel> LightEffects { get; set; }
    }
}
