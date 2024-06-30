using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley.TerrainFeatures;

namespace CrystalFences
{
    public class Textures
    {
        public static readonly Color YELLOW_COLOUR = new Color(255, 255, 0);
        public static readonly Color RED_COLOUR = new Color(255, 40, 40);
        public static readonly Color GREEN_COLOUR = new Color(0, 230, 20);
        public static readonly Color PURPLE_COLOUR = new Color(173, 0, 230);

        public static Texture2D TintTexture(Texture2D originalTexture, Color tint)
        {
            Texture2D tintedTexture = new Texture2D(originalTexture.GraphicsDevice, originalTexture.Width, originalTexture.Height);

            Color[] originalColorData = new Color[originalTexture.Width * originalTexture.Height];
            originalTexture.GetData(originalColorData);

            Color[] tintedColorData = new Color[originalTexture.Width * originalTexture.Height];

            for (int i = 0; i < originalColorData.Length; i++)
            {
                // Extract the greyscale value (assuming it's the red component, since R=G=B in greyscale)
                float greyScale = originalColorData[i].R / 255.0f;  // Normalize to 0-1 range

                byte alpha = originalColorData[i].A;

                byte red = (byte)(tint.R * greyScale);
                byte green = (byte)(tint.G * greyScale);
                byte blue = (byte)(tint.B * greyScale);

                tintedColorData[i] = new Color(red, green, blue, alpha);
            }

            tintedTexture.SetData(tintedColorData);

            return tintedTexture;
        }

        public static void SetupFenceTextures(ModConfig config, IModHelper helper)
        {
            if (config == null) return;
            string woodFencePath = Path.Combine(helper.DirectoryPath, "assets", "fences", config.WoodFence.Model == 1 ? "Fence1.png" : $"CustomFence{config.WoodFence.Model - 1}.png");
            string stoneFencePath = Path.Combine(helper.DirectoryPath, "assets", "fences", config.StoneFence.Model == 1 ? "Fence2.png" : $"CustomFence{config.StoneFence.Model - 1}.png");
            string ironFencePath = Path.Combine(helper.DirectoryPath, "assets", "fences", config.IronFence.Model == 1 ? "Fence3.png" : $"CustomFence{config.IronFence.Model - 1}.png");
            string hardwoodFencePath = Path.Combine(helper.DirectoryPath, "assets", "fences", config.HardwoodFence.Model == 1 ? "Fence5.png" : $"CustomFence{config.HardwoodFence.Model - 1}.png");

            Texture2D woodFence = helper.ModContent.Load<Texture2D>(woodFencePath);
            Texture2D stoneFence = helper.ModContent.Load<Texture2D>(stoneFencePath);
            Texture2D ironFence = helper.ModContent.Load<Texture2D>(ironFencePath);
            Texture2D hardwoodFence = helper.ModContent.Load<Texture2D>(hardwoodFencePath);

            if (!string.IsNullOrEmpty(config.WoodFence.Crystal))
            {
                Texture2D crystal = helper.ModContent.Load<Texture2D>(Path.Combine(helper.DirectoryPath, "assets", "crystals", config.WoodFence.Crystal));
                Texture2D newTexture = Textures.OverlayTextures(woodFence, crystal);
                Texture2D stage = helper.ModContent.Load<Texture2D>(Path.Combine(helper.DirectoryPath, "assets", "stages", $"Type{GetFenceType(config, "wood")}Stage{config.WoodFence.Stage}"));
                if (config.WoodFence.Stage > 0) stage = TintTexture(stage, FetchColour(config.WoodFence.Crystal));
                newTexture = OverlayTextures(newTexture, stage);
                SaveTextureAsPng(newTexture, Path.Combine(helper.DirectoryPath, "assets", "sprites", "Fence1.png"));
            }
            if (!string.IsNullOrEmpty(config.StoneFence.Crystal))
            {
                Texture2D crystal = helper.ModContent.Load<Texture2D>(Path.Combine(helper.DirectoryPath, "assets", "crystals", config.StoneFence.Crystal));
                Texture2D newTexture = Textures.OverlayTextures(stoneFence, crystal);
                Texture2D stage = helper.ModContent.Load<Texture2D>(Path.Combine(helper.DirectoryPath, "assets", "stages", $"Type{GetFenceType(config, "stone")}Stage{config.StoneFence.Stage}"));
                if (config.WoodFence.Stage > 0) stage = TintTexture(stage, FetchColour(config.StoneFence.Crystal));
                newTexture = OverlayTextures(newTexture, stage);
                SaveTextureAsPng(newTexture, Path.Combine(helper.DirectoryPath, "assets", "sprites", "Fence2.png"));
            }
            if (!string.IsNullOrEmpty(config.IronFence.Crystal))
            {
                Texture2D crystal = helper.ModContent.Load<Texture2D>(Path.Combine(helper.DirectoryPath, "assets", "crystals", config.IronFence.Crystal));
                Texture2D newTexture = Textures.OverlayTextures(ironFence, crystal);
                Texture2D stage = helper.ModContent.Load<Texture2D>(Path.Combine(helper.DirectoryPath, "assets", "stages", $"Type{GetFenceType(config, "iron")}Stage{config.IronFence.Stage}"));
                if (config.WoodFence.Stage > 0) stage = TintTexture(stage, FetchColour(config.IronFence.Crystal));
                newTexture = OverlayTextures(newTexture, stage);
                SaveTextureAsPng(newTexture, Path.Combine(helper.DirectoryPath, "assets", "sprites", "Fence3.png"));
            }
            if (!string.IsNullOrEmpty(config.HardwoodFence.Crystal))
            {
                Texture2D crystal = helper.ModContent.Load<Texture2D>(Path.Combine(helper.DirectoryPath, "assets", "crystals", config.HardwoodFence.Crystal));
                Texture2D newTexture = Textures.OverlayTextures(hardwoodFence, crystal);
                Texture2D stage = helper.ModContent.Load<Texture2D>(Path.Combine(helper.DirectoryPath, "assets", "stages", $"Type{GetFenceType(config, "hardwood")}Stage{config.HardwoodFence.Stage}"));
                if (config.WoodFence.Stage > 0) stage = TintTexture(stage, FetchColour(config.HardwoodFence.Crystal));
                newTexture = OverlayTextures(newTexture, stage);
                SaveTextureAsPng(newTexture, Path.Combine(helper.DirectoryPath, "assets", "sprites", "Fence5.png"));
            }
        }

        public static int GetFenceType(ModConfig config, string fence)
        {
            switch (fence.ToLower())
            {
                case "wood":
                case "hardwood":
                    return 1;
                case "stone":
                    if (config.StoneFence.Model == 1) return 1; //2
                    return 1;
                case "iron":
                    if (config.IronFence.Model == 1) return 3;
                    return 1;
            }
            return 1;
        }

        public static Texture2D OverlayTextures(Texture2D baseTexture, Texture2D overlayTexture)
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

        public static void SaveTextureAsPng(Texture2D texture, string path)
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

        private static Color FetchColour(string colour)
        {
            switch (colour.ToLower())
            {
                case "yellow":
                    return YELLOW_COLOUR;
                case "red":
                    return RED_COLOUR;
                case "green":
                    return GREEN_COLOUR;
                case "purple":
                    return PURPLE_COLOUR;
                default:
                    return Color.White;
            }
        }
    }
}
