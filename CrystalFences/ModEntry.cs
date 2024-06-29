using System;
using System.Xml.Linq;
using System.Xml;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using Newtonsoft.Json;
using xTile;
using Microsoft.Xna.Framework.Graphics;

namespace CrystalFences
{
    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry : Mod
    {
        private ModConfig? config;
        private const string CONTENT_FILEPATH = "content.json";
        private const string IMAGES_FILEPATH = "./assests/";
        public const int DEFAULT_WOOD_FENCE = 1;
        public const int DEFAULT_STONE_FENCE = 2;
        public const int DEFAULT_IRON_FENCE = 3;
        public const int DEFAULT_HARDWOOD_FENCE = 4;

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
            helper.Events.Content.AssetRequested += OnAssetRequested;
        }

        /// <inheritdoc cref="IContentEvents.AssetRequested"/>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnAssetRequested(object sender, AssetRequestedEventArgs e)
        {
            if (e.Name.IsEquivalentTo("LooseSprites/Fence1"))
            {
                e.LoadFromModFile<Texture2D>($"assets/CrystalFence{config?.WoodFence}.png", AssetLoadPriority.Medium);
            }
            else if (e.Name.IsEquivalentTo("LooseSprites/Fence2"))
            {
                e.LoadFromModFile<Texture2D>($"assets/CrystalFence{config?.StoneFence}.png", AssetLoadPriority.Medium);
            }
            else if (e.Name.IsEquivalentTo("LooseSprites/Fence3"))
            {
                e.LoadFromModFile<Texture2D>($"assets/CrystalFence{config?.IronFence}.png", AssetLoadPriority.Medium);
            }
            else if (e.Name.IsEquivalentTo("LooseSprites/Fence5"))
            {
                e.LoadFromModFile<Texture2D>($"assets/CrystalFence{config?.HardwoodFence}.png", AssetLoadPriority.Medium);
            }
        }

        private void CheckConfig()
        {
            int numberOfFenceSprites = GetNumberOfFenceSprites();
            if (config == null) return;
            if (config.WoodFence < 1 || config.WoodFence > numberOfFenceSprites) config.WoodFence = DEFAULT_WOOD_FENCE;
            if (config.StoneFence < 1 || config.StoneFence > numberOfFenceSprites) config.StoneFence = DEFAULT_STONE_FENCE;
            if (config.IronFence < 1 || config.IronFence > numberOfFenceSprites) config.IronFence = DEFAULT_IRON_FENCE;
            if (config.HardwoodFence < 1 || config.HardwoodFence > numberOfFenceSprites) config.HardwoodFence = DEFAULT_HARDWOOD_FENCE;
        }

        private int GetNumberOfFenceSprites()
        {
            string path = Path.Combine(Helper.DirectoryPath, "assets");
            Monitor.Log($"Path to assets: {path}");
            string[] files = Directory.GetFiles(path);

            var filteredFiles = files.Where(file => Path.GetFileName(file).Contains("CrystalFence") && Path.GetFileName(file).EndsWith(".png"));

            return filteredFiles.Count();
        }
    }
}