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
        private static Texture2D IconDefaultLowGravity { get; set; }
        private static Texture2D IconDefaultUmbrella { get; set; }

        private static Texture2D IconCustomNone { get; set; }
        private static Texture2D IconCustomDoubleJump { get; set; }
        private static Texture2D IconCustomLowGravity { get; set; }
        private static Texture2D IconCustomUmbrella { get; set; }

        private static Texture2D DefaultDoubleJump { get; set; }
        private static Texture2D DefaultLowGravity { get; set; }
        private static Texture2D DefaultUmbrella { get; set; }

        private static Texture2D CustomDoubleJump { get; set; }
        private static Texture2D CustomLowGravity { get; set; }
        private static Texture2D CustomUmbrella { get; set; }

        public static void LoadDefaultTextures()
        {
            var contentManager = Game1.instance.contentManager;
            var dllPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
                          throw new InvalidOperationException();
            var texturePath = Path.Combine(dllPath, "textures");

            DefaultMenu = contentManager.Load<Texture2D>(Path.Combine(texturePath, "menu"));

            IconDefaultNone = contentManager.Load<Texture2D>(Path.Combine(texturePath, "iconNone"));
            IconDefaultDoubleJump = contentManager.Load<Texture2D>(Path.Combine(texturePath, "iconDoubleJump"));
            IconDefaultLowGravity = contentManager.Load<Texture2D>(Path.Combine(texturePath, "iconLowGravity"));
            IconDefaultUmbrella = contentManager.Load<Texture2D>(Path.Combine(texturePath, "iconUmbrella"));

            DefaultDoubleJump = contentManager.Load<Texture2D>(Path.Combine(texturePath, "doubleJump"));
            DefaultLowGravity = contentManager.Load<Texture2D>(Path.Combine(texturePath, "lowGravity"));
            DefaultUmbrella = contentManager.Load<Texture2D>(Path.Combine(texturePath, "umbrella"));
        }

        public static void LoadCustomTextures()
        {
            var contentManager = Game1.instance.contentManager;
            var texturePath = Path.Combine(contentManager.root, "props", "metroidvania");

            CustomMenu = GetOptionalTexture(contentManager, Path.Combine(texturePath, "menu"));

            IconCustomNone = GetOptionalTexture(contentManager, Path.Combine(texturePath, "iconNone"));
            IconCustomDoubleJump = GetOptionalTexture(contentManager, Path.Combine(texturePath, "iconDoubleJump"));
            IconCustomLowGravity = GetOptionalTexture(contentManager, Path.Combine(texturePath, "iconLowGravity"));
            IconCustomUmbrella = GetOptionalTexture(contentManager, Path.Combine(texturePath, "iconUmbrella"));

            CustomDoubleJump = GetOptionalTexture(contentManager, Path.Combine(texturePath, "doubleJump"));
            CustomLowGravity = GetOptionalTexture(contentManager, Path.Combine(texturePath, "lowGravity"));
            CustomUmbrella = GetOptionalTexture(contentManager, Path.Combine(texturePath, "umbrella"));
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
                case ItemType.LowGravity:
                    return IconCustomLowGravity ?? IconDefaultLowGravity;
                case ItemType.Umbrella:
                    return IconCustomUmbrella ?? IconDefaultUmbrella;
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
                case ItemType.LowGravity:
                    return CustomLowGravity ?? DefaultLowGravity;
                case ItemType.Umbrella:
                    return CustomUmbrella ?? DefaultUmbrella;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
