namespace CrystalFences
{
    public class ModConfig
    {
        public ModConfigFence WoodFence { get; set; } = new ModConfigFence() { Crystal = "yellow" };
        public ModConfigFence StoneFence { get; set; } = new ModConfigFence() { Crystal = "green" };
        public ModConfigFence IronFence { get; set; } = new ModConfigFence() { Crystal = "red" };
        public ModConfigFence HardwoodFence { get; set; } = new ModConfigFence() { Crystal = "purple" };
    }

    public class ModConfigFence
    {
        // Original 1, custom 2
        public int Model { get; set; } = 1;
        public string Crystal { get; set; } = "yellow";
        public int Stage { get; set; } = 1;
}
