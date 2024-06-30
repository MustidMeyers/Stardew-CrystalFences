using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace CrystalFences
{
    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry : Mod
    {
        private ModConfig? config;
        public const int DEFAULT_WOOD_FENCE = 1;
        public const int DEFAULT_STONE_FENCE = 2;
        public const int DEFAULT_IRON_FENCE = 3;
        public const int DEFAULT_HARDWOOD_FENCE = 4;
        public static readonly string[] AVAILABLE_CRYSTAL_COLOURS = new string[] { "", "yellow", "green", "red", "purple" };

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            config = Helper.ReadConfig<ModConfig>();
            CheckConfig();
            Helper.WriteConfig(config);

            Textures.SetupFenceTextures(config, helper);

            helper.Events.Content.AssetRequested += OnAssetRequested;
        }

        /// <inheritdoc cref="IContentEvents.AssetRequested"/>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnAssetRequested(object sender, AssetRequestedEventArgs e)
        {
            if (e.Name.IsEquivalentTo("LooseSprites/Fence1"))
            {
                e.LoadFromModFile<Texture2D>($"assets/sprites/Fence1.png", AssetLoadPriority.Medium);
            }
            else if (e.Name.IsEquivalentTo("LooseSprites/Fence2"))
            {
                e.LoadFromModFile<Texture2D>($"assets/sprites/Fence2.png", AssetLoadPriority.Medium);
            }
            else if (e.Name.IsEquivalentTo("LooseSprites/Fence3"))
            {
                e.LoadFromModFile<Texture2D>($"assets/sprites/Fence3.png", AssetLoadPriority.Medium);
            }
            else if (e.Name.IsEquivalentTo("LooseSprites/Fence5"))
            {
                e.LoadFromModFile<Texture2D>($"assets/sprites/Fence5.png", AssetLoadPriority.Medium);
            }
        }

        private void CheckConfig()
        {
            if (config == null) return;
            CheckFenceConfigs(config.WoodFence);
            CheckFenceConfigs(config.StoneFence);
            CheckFenceConfigs(config.IronFence);
            CheckFenceConfigs(config.HardwoodFence);
        }

        private void CheckFenceConfigs(ModConfigFence fence)
        {
            if (!AVAILABLE_CRYSTAL_COLOURS.Contains(fence.Crystal.ToLower())) fence.Crystal = "";
            fence.Crystal = fence.Crystal.ToLower();
            if (fence.Model < 1 && fence.Model > 2) fence.Model = 1;
            if (fence.Stage < 0 && fence.Stage > 4) fence.Stage = 0;
        }
    }
}