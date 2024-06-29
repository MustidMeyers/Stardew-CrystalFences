using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalFences
{
    public class ModConfig
    {
        public int WoodFence { get; set; } = ModEntry.DEFAULT_WOOD_FENCE;
        public int StoneFence { get; set; } = ModEntry.DEFAULT_STONE_FENCE;
        public int IronFence { get; set; } = ModEntry.DEFAULT_IRON_FENCE;
        public int HardwoodFence { get; set; } = ModEntry.DEFAULT_HARDWOOD_FENCE;
    }
}
