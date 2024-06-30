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

            SetupFenceTextures();

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

        private void SetupFenceTextures()
        {
            if (config == null) return;
            string woodFencePath = Path.Combine(Helper.DirectoryPath, "assets", "fences", config.WoodFence.Model == 1 ? "Fence1.png" : "CustomFence1.png");
            string stoneFencePath = Path.Combine(Helper.DirectoryPath, "assets", "fences", config.WoodFence.Model == 1 ? "Fence2.png" : "CustomFence1.png");
            string ironFencePath = Path.Combine(Helper.DirectoryPath, "assets", "fences", config.WoodFence.Model == 1 ? "Fence3.png" : "CustomFence1.png");
            string hardwoodFencePath = Path.Combine(Helper.DirectoryPath, "assets", "fences", config.WoodFence.Model == 1 ? "Fence5.png" : "CustomFence1.png");

            Texture2D woodFence = Helper.ModContent.Load<Texture2D>(woodFencePath);
            Texture2D stoneFence = Helper.ModContent.Load<Texture2D>(stoneFencePath);
            Texture2D ironFence = Helper.ModContent.Load<Texture2D>(ironFencePath);
            Texture2D hardwoodFence = Helper.ModContent.Load<Texture2D>(hardwoodFencePath);

            if (!string.IsNullOrEmpty(config.WoodFence.Crystal))
            {
                Texture2D crystal = Helper.ModContent.Load<Texture2D>(Path.Combine(Helper.DirectoryPath, "assets", "crystals", config.WoodFence.Crystal));
                Texture2D newTexture = OverlayTextures(woodFence, crystal);
                SaveTextureAsPng(newTexture, Path.Combine(Helper.DirectoryPath, "assets", "sprites", "Fence1.png"));
            }
            if (!string.IsNullOrEmpty(config.StoneFence.Crystal))
            {
                Texture2D crystal = Helper.ModContent.Load<Texture2D>(Path.Combine(Helper.DirectoryPath, "assets", "crystals", config.StoneFence.Crystal));
                Texture2D newTexture = OverlayTextures(stoneFence, crystal);
                SaveTextureAsPng(newTexture, Path.Combine(Helper.DirectoryPath, "assets", "sprites", "Fence2.png"));
            }
            if (!string.IsNullOrEmpty(config.IronFence.Crystal))
            {
                Texture2D crystal = Helper.ModContent.Load<Texture2D>(Path.Combine(Helper.DirectoryPath, "assets", "crystals", config.IronFence.Crystal));
                Texture2D newTexture = OverlayTextures(ironFence, crystal);
                SaveTextureAsPng(newTexture, Path.Combine(Helper.DirectoryPath, "assets", "sprites", "Fence3.png"));
            }
            if (!string.IsNullOrEmpty(config.HardwoodFence.Crystal))
            {
                Texture2D crystal = Helper.ModContent.Load<Texture2D>(Path.Combine(Helper.DirectoryPath, "assets", "crystals", config.HardwoodFence.Crystal));
                Texture2D newTexture = OverlayTextures(hardwoodFence, crystal);
                SaveTextureAsPng(newTexture, Path.Combine(Helper.DirectoryPath, "assets", "sprites", "Fence5.png"));
            }
        }

        private Texture2D OverlayTextures(Texture2D baseTexture, Texture2D overlayTexture)
        {
            if (baseTexture.Width != overlayTexture.Width || baseTexture.Height != overlayTexture.Height)
            {
                throw new System.Exception("Textures must be of the same size");
            }

            Color[] baseColors = new Color[baseTexture.Width * baseTexture.Height];
            Color[] overlayColors = new Color[overlayTexture.Width * overlayTexture.Height];
            Color[] resultColors = new Color[baseTexture.Width * baseTexture.Height];

            baseTexture.GetData(baseColors);
            overlayTexture.GetData(overlayColors);

            for (int i = 0; i < baseColors.Length; i++)
            {
                Color baseColor = baseColors[i];
                Color overlayColor = overlayColors[i];

                // Simple overlay blending: blending based on alpha
                Color combinedColor = Color.Lerp(baseColor, overlayColor, overlayColor.A / 255f);

                resultColors[i] = combinedColor;
            }

            Texture2D resultTexture = new Texture2D(baseTexture.GraphicsDevice, baseTexture.Width, baseTexture.Height);
            resultTexture.SetData(resultColors);

            return resultTexture;
        }

        private void SaveTextureAsPng(Texture2D texture, string path)
        {
            int width = texture.Width;
            int height = texture.Height;
            Color[] data = new Color[width * height];
            texture.GetData(data);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                texture.SaveAsPng(stream, texture.Width, texture.Height);
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
        }
    }
}