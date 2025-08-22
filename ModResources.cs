namespace MetroidvaniaItems
{
    using System;
    using System.IO;
    using System.Reflection;
    using JumpKing;
    using Microsoft.Xna.Framework.Graphics;
    using static ModItems;

    public static class ModResources
    {
        public static Texture2D DefaultMenu { get; private set; }
        public static Texture2D CustomMenu { get; private set; }

        private static Texture2D IconDefaultNone { get; set; }
        private static Texture2D IconDefaultDoubleJump { get; set; }
        private static Texture2D IconDefaultLongJump { get; set; }
        private static Texture2D IconDefaultLowGravity { get; set; }
        private static Texture2D IconDefaultHighGravity { get; set; }
        private static Texture2D IconDefaultSponge { get; set; }
        private static Texture2D IconDefaultUmbrella { get; set; }
        private static Texture2D IconDefaultWaterBoots { get; set; }

        private static Texture2D IconCustomNone { get; set; }
        private static Texture2D IconCustomDoubleJump { get; set; }
        private static Texture2D IconCustomLongJump { get; set; }
        private static Texture2D IconCustomLowGravity { get; set; }
        private static Texture2D IconCustomHighGravity { get; set; }
        private static Texture2D IconCustomSponge { get; set; }
        private static Texture2D IconCustomUmbrella { get; set; }
        private static Texture2D IconCustomWaterBoots { get; set; }

        private static Texture2D DefaultDoubleJump { get; set; }
        private static Texture2D DefaultLongJump { get; set; }
        private static Texture2D DefaultLowGravity { get; set; }
        private static Texture2D DefaultHighGravity { get; set; }
        private static Texture2D DefaultSponge { get; set; }
        private static Texture2D DefaultUmbrella { get; set; }
        private static Texture2D DefaultWaterBoots { get; set; }

        private static Texture2D CustomDoubleJump { get; set; }
        private static Texture2D CustomLongJump { get; set; }
        private static Texture2D CustomLowGravity { get; set; }
        private static Texture2D CustomHighGravity { get; set; }
        private static Texture2D CustomSponge { get; set; }
        private static Texture2D CustomUmbrella { get; set; }
        private static Texture2D CustomWaterBoots { get; set; }

        public static void LoadDefaultTextures()
        {
            var contentManager = Game1.instance.contentManager;
            var dllPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
                          throw new InvalidOperationException();
            var texturePath = Path.Combine(dllPath, "textures");

            DefaultMenu = contentManager.Load<Texture2D>(Path.Combine(texturePath, "menu"));

            IconDefaultNone = contentManager.Load<Texture2D>(Path.Combine(texturePath, "iconNone"));
            IconDefaultDoubleJump = contentManager.Load<Texture2D>(Path.Combine(texturePath, "iconDoubleJump"));
            IconDefaultLongJump = contentManager.Load<Texture2D>(Path.Combine(texturePath, "iconLongJump"));
            IconDefaultLowGravity = contentManager.Load<Texture2D>(Path.Combine(texturePath, "iconLowGravity"));
            IconDefaultHighGravity = contentManager.Load<Texture2D>(Path.Combine(texturePath, "iconHighGravity"));
            IconDefaultSponge = contentManager.Load<Texture2D>(Path.Combine(texturePath, "iconSponge"));
            IconDefaultUmbrella = contentManager.Load<Texture2D>(Path.Combine(texturePath, "iconUmbrella"));
            IconDefaultWaterBoots = contentManager.Load<Texture2D>(Path.Combine(texturePath, "iconWaterBoots"));

            DefaultDoubleJump = contentManager.Load<Texture2D>(Path.Combine(texturePath, "doubleJump"));
            DefaultLongJump = contentManager.Load<Texture2D>(Path.Combine(texturePath, "longJump"));
            DefaultLowGravity = contentManager.Load<Texture2D>(Path.Combine(texturePath, "lowGravity"));
            DefaultHighGravity = contentManager.Load<Texture2D>(Path.Combine(texturePath, "highGravity"));
            DefaultSponge = contentManager.Load<Texture2D>(Path.Combine(texturePath, "sponge"));
            DefaultUmbrella = contentManager.Load<Texture2D>(Path.Combine(texturePath, "umbrella"));
            DefaultWaterBoots = contentManager.Load<Texture2D>(Path.Combine(texturePath, "waterBoots"));
        }

        public static void LoadCustomTextures()
        {
            var contentManager = Game1.instance.contentManager;
            var texturePath = Path.Combine(contentManager.root, "props", "metroidvania");

            CustomMenu = GetOptionalTexture(contentManager, Path.Combine(texturePath, "menu"));

            IconCustomNone = GetOptionalTexture(contentManager, Path.Combine(texturePath, "iconNone"));
            IconCustomDoubleJump = GetOptionalTexture(contentManager, Path.Combine(texturePath, "iconDoubleJump"));
            IconCustomLongJump = GetOptionalTexture(contentManager, Path.Combine(texturePath, "iconLongJump"));
            IconCustomLowGravity = GetOptionalTexture(contentManager, Path.Combine(texturePath, "iconLowGravity"));
            IconCustomHighGravity = GetOptionalTexture(contentManager, Path.Combine(texturePath, "iconHighGravity"));
            IconCustomSponge = GetOptionalTexture(contentManager, Path.Combine(texturePath, "iconSponge"));
            IconCustomUmbrella = GetOptionalTexture(contentManager, Path.Combine(texturePath, "iconUmbrella"));
            IconCustomWaterBoots = GetOptionalTexture(contentManager, Path.Combine(texturePath, "iconWaterBoots"));

            CustomDoubleJump = GetOptionalTexture(contentManager, Path.Combine(texturePath, "doubleJump"));
            CustomLongJump = GetOptionalTexture(contentManager, Path.Combine(texturePath, "longJump"));
            CustomLowGravity = GetOptionalTexture(contentManager, Path.Combine(texturePath, "lowGravity"));
            CustomHighGravity = GetOptionalTexture(contentManager, Path.Combine(texturePath, "highGravity"));
            CustomSponge = GetOptionalTexture(contentManager, Path.Combine(texturePath, "sponge"));
            CustomUmbrella = GetOptionalTexture(contentManager, Path.Combine(texturePath, "umbrella"));
            CustomWaterBoots = GetOptionalTexture(contentManager, Path.Combine(texturePath, "waterBoots"));
        }

        private static Texture2D GetOptionalTexture(JKContentManager contentManager, string texturePath)
            => File.Exists(texturePath)
                ? contentManager.Load<Texture2D>(texturePath)
                : null;

        public static Texture2D GetIconByType(ItemType type)
        {
            switch (type)
            {
                case ItemType.None:
                    return IconCustomNone ?? IconDefaultNone;
                case ItemType.DoubleJump:
                    return IconCustomDoubleJump ?? IconDefaultDoubleJump;
                case  ItemType.LongJump:
                    return IconCustomLongJump ?? IconDefaultLongJump;
                case ItemType.LowGravity:
                    return IconCustomLowGravity ?? IconDefaultLowGravity;
                case ItemType.HighGravity:
                    return IconCustomHighGravity ?? IconDefaultHighGravity;
                case ItemType.Sponge:
                    return IconCustomSponge ?? IconDefaultSponge;
                case ItemType.Umbrella:
                    return IconCustomUmbrella ?? IconDefaultUmbrella;
                case ItemType.WaterBoots:
                    return IconCustomWaterBoots ?? IconDefaultWaterBoots;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public static Texture2D GetInWorldByType(ItemType type)
        {
            switch (type)
            {
                case ItemType.None:
                    throw new Exception("Cannot create an in world none item!");
                case ItemType.DoubleJump:
                    return CustomDoubleJump ?? DefaultDoubleJump;
                case ItemType.LongJump:
                    return CustomLongJump ?? DefaultLongJump;
                case ItemType.LowGravity:
                    return CustomLowGravity ?? DefaultLowGravity;
                case ItemType.HighGravity:
                    return CustomHighGravity ?? DefaultHighGravity;
                case ItemType.Sponge:
                    return  CustomSponge ?? DefaultSponge;
                case ItemType.Umbrella:
                    return CustomUmbrella ?? DefaultUmbrella;
                case ItemType.WaterBoots:
                    return CustomWaterBoots ?? DefaultWaterBoots;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
