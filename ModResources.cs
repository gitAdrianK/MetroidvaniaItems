namespace MetroidvaniaItems
{
    using System;
    using System.IO;
    using System.Reflection;
    using JumpKing;
    using Microsoft.Xna.Framework.Graphics;

    public static class ModResources
    {
        // Menu
        public static Texture2D DefaultMenu { get; private set; }
        public static Texture2D CustomMenu { get; private set; }

        // None
        private static Texture2D IconDefaultNone { get; set; }
        private static Texture2D IconCustomNone { get; set; }

        // Double Jump
        private static Texture2D DefaultDoubleJump { get; set; }
        private static Texture2D IconDefaultDoubleJump { get; set; }
        private static Texture2D IconCustomDoubleJump { get; set; }
        private static Texture2D CustomDoubleJump { get; set; }

        // Long Jump
        private static Texture2D IconDefaultLongJump { get; set; }
        private static Texture2D IconCustomLongJump { get; set; }
        private static Texture2D DefaultLongJump { get; set; }
        private static Texture2D CustomLongJump { get; set; }

        // Low Gravity
        private static Texture2D IconDefaultLowGravity { get; set; }
        private static Texture2D IconCustomLowGravity { get; set; }
        private static Texture2D DefaultLowGravity { get; set; }
        private static Texture2D CustomLowGravity { get; set; }

        // High Gravity
        private static Texture2D IconDefaultHighGravity { get; set; }
        private static Texture2D DefaultHighGravity { get; set; }
        private static Texture2D IconCustomHighGravity { get; set; }
        private static Texture2D CustomHighGravity { get; set; }

        // Slow Fall
        private static Texture2D IconDefaultSlowFall { get; set; }
        private static Texture2D IconCustomSlowFall { get; set; }
        private static Texture2D DefaultSlowFall { get; set; }
        private static Texture2D CustomSlowFall { get; set; }

        // Never Water
        private static Texture2D IconDefaultNeverWater { get; set; }
        private static Texture2D IconCustomNeverWater { get; set; }
        private static Texture2D DefaultNeverWater { get; set; }
        private static Texture2D CustomNeverWater { get; set; }

        // Always Water
        private static Texture2D IconDefaultAlwaysWater { get; set; }
        private static Texture2D IconCustomAlwaysWater { get; set; }
        private static Texture2D DefaultAlwaysWater { get; set; }
        private static Texture2D CustomAlwaysWater { get; set; }

        // Solid Water
        private static Texture2D IconDefaultSolidWater { get; set; }
        private static Texture2D IconCustomSolidWater { get; set; }
        private static Texture2D DefaultSolidWater { get; set; }
        private static Texture2D CustomSolidWater { get; set; }

        // Always Ice
        private static Texture2D IconDefaultNeverIce { get; set; }
        private static Texture2D IconCustomNeverIce { get; set; }
        private static Texture2D DefaultNeverIce { get; set; }
        private static Texture2D CustomNeverIce { get; set; }

        public static void LoadDefaultTextures()
        {
            var contentManager = Game1.instance.contentManager;
            var dllPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
                          throw new InvalidOperationException();

            var uiPath = Path.Combine(dllPath, "ui");
            DefaultMenu = contentManager.Load<Texture2D>(Path.Combine(uiPath, "Menu"));

            var iconPath = Path.Combine(dllPath, "icons");
            IconDefaultNone = contentManager.Load<Texture2D>(Path.Combine(iconPath, nameof(ModItems.None)));

            IconDefaultDoubleJump = contentManager.Load<Texture2D>(Path.Combine(iconPath, nameof(ModItems.DoubleJump)));
            IconDefaultLongJump = contentManager.Load<Texture2D>(Path.Combine(iconPath, nameof(ModItems.LongJump)));

            IconDefaultLowGravity = contentManager.Load<Texture2D>(Path.Combine(iconPath, nameof(ModItems.LowGravity)));
            IconDefaultHighGravity =
                contentManager.Load<Texture2D>(Path.Combine(iconPath, nameof(ModItems.HighGravity)));

            IconDefaultSlowFall = contentManager.Load<Texture2D>(Path.Combine(iconPath, nameof(ModItems.SlowFall)));

            IconDefaultNeverWater = contentManager.Load<Texture2D>(Path.Combine(iconPath, nameof(ModItems.NeverWater)));
            IconDefaultAlwaysWater =
                contentManager.Load<Texture2D>(Path.Combine(iconPath, nameof(ModItems.AlwaysWater)));
            IconDefaultSolidWater = contentManager.Load<Texture2D>(Path.Combine(iconPath, nameof(ModItems.SolidWater)));

            IconDefaultNeverIce = contentManager.Load<Texture2D>(Path.Combine(iconPath, nameof(ModItems.NeverIce)));

            var itemPath = Path.Combine(dllPath, "items");
            DefaultDoubleJump = contentManager.Load<Texture2D>(Path.Combine(itemPath, nameof(ModItems.DoubleJump)));
            DefaultLongJump = contentManager.Load<Texture2D>(Path.Combine(itemPath, nameof(ModItems.LongJump)));

            DefaultLowGravity = contentManager.Load<Texture2D>(Path.Combine(itemPath, nameof(ModItems.LowGravity)));
            DefaultHighGravity = contentManager.Load<Texture2D>(Path.Combine(itemPath, nameof(ModItems.HighGravity)));

            DefaultSlowFall = contentManager.Load<Texture2D>(Path.Combine(itemPath, nameof(ModItems.SlowFall)));

            DefaultNeverWater = contentManager.Load<Texture2D>(Path.Combine(itemPath, nameof(ModItems.NeverWater)));
            DefaultAlwaysWater = contentManager.Load<Texture2D>(Path.Combine(itemPath, nameof(ModItems.AlwaysWater)));
            DefaultSolidWater = contentManager.Load<Texture2D>(Path.Combine(itemPath, nameof(ModItems.SolidWater)));

            DefaultNeverIce = contentManager.Load<Texture2D>(Path.Combine(itemPath, nameof(ModItems.NeverIce)));
        }

        public static void LoadCustomTextures()
        {
            var contentManager = Game1.instance.contentManager;
            var modFolderPath = Path.Combine(contentManager.root, "metroidvania");

            var uiPath = Path.Combine(modFolderPath, "ui");
            CustomMenu = GetOptionalTexture(contentManager, Path.Combine(uiPath, "Menu"));

            var iconPath = Path.Combine(modFolderPath, "icons");
            IconCustomNone = GetOptionalTexture(contentManager, Path.Combine(iconPath, nameof(ModItems.None)));

            IconCustomDoubleJump =
                GetOptionalTexture(contentManager, Path.Combine(iconPath, nameof(ModItems.DoubleJump)));
            IconCustomLongJump = GetOptionalTexture(contentManager, Path.Combine(iconPath, nameof(ModItems.LongJump)));

            IconCustomLowGravity =
                GetOptionalTexture(contentManager, Path.Combine(iconPath, nameof(ModItems.LowGravity)));
            IconCustomHighGravity =
                GetOptionalTexture(contentManager, Path.Combine(iconPath, nameof(ModItems.HighGravity)));

            IconCustomSlowFall = GetOptionalTexture(contentManager, Path.Combine(iconPath, nameof(ModItems.SlowFall)));

            IconCustomNeverWater =
                GetOptionalTexture(contentManager, Path.Combine(iconPath, nameof(ModItems.NeverWater)));
            IconCustomAlwaysWater =
                GetOptionalTexture(contentManager, Path.Combine(iconPath, nameof(ModItems.AlwaysWater)));
            IconCustomSolidWater =
                GetOptionalTexture(contentManager, Path.Combine(iconPath, nameof(ModItems.SolidWater)));

            IconCustomNeverIce = GetOptionalTexture(contentManager, Path.Combine(iconPath, nameof(ModItems.NeverIce)));

            var itemPath = Path.Combine(modFolderPath, "items");
            CustomDoubleJump = GetOptionalTexture(contentManager, Path.Combine(itemPath, nameof(ModItems.DoubleJump)));
            CustomLongJump = GetOptionalTexture(contentManager, Path.Combine(itemPath, nameof(ModItems.LongJump)));

            CustomLowGravity = GetOptionalTexture(contentManager, Path.Combine(itemPath, nameof(ModItems.LowGravity)));
            CustomHighGravity =
                GetOptionalTexture(contentManager, Path.Combine(itemPath, nameof(ModItems.HighGravity)));

            CustomSlowFall = GetOptionalTexture(contentManager, Path.Combine(itemPath, nameof(ModItems.SlowFall)));

            CustomNeverWater = GetOptionalTexture(contentManager, Path.Combine(itemPath, nameof(ModItems.NeverWater)));
            CustomAlwaysWater =
                GetOptionalTexture(contentManager, Path.Combine(itemPath, nameof(ModItems.AlwaysWater)));
            CustomSolidWater = GetOptionalTexture(contentManager, Path.Combine(itemPath, nameof(ModItems.SolidWater)));

            CustomNeverIce = GetOptionalTexture(contentManager, Path.Combine(itemPath, nameof(ModItems.NeverIce)));
        }

        private static Texture2D GetOptionalTexture(JKContentManager contentManager, string texturePath)
            => File.Exists(texturePath)
                ? contentManager.Load<Texture2D>(texturePath)
                : null;

        public static Texture2D GetIconByType(ModItems type)
        {
            switch (type)
            {
                case ModItems.None:
                    return IconCustomNone ?? IconDefaultNone;

                case ModItems.DoubleJump:
                    return IconCustomDoubleJump ?? IconDefaultDoubleJump;
                case ModItems.LongJump:
                    return IconCustomLongJump ?? IconDefaultLongJump;

                case ModItems.LowGravity:
                    return IconCustomLowGravity ?? IconDefaultLowGravity;
                case ModItems.HighGravity:
                    return IconCustomHighGravity ?? IconDefaultHighGravity;

                case ModItems.SlowFall:
                    return IconCustomSlowFall ?? IconDefaultSlowFall;

                case ModItems.NeverWater:
                    return IconCustomNeverWater ?? IconDefaultNeverWater;
                case ModItems.AlwaysWater:
                    return IconCustomAlwaysWater ?? IconDefaultAlwaysWater;
                case ModItems.SolidWater:
                    return IconCustomSolidWater ?? IconDefaultSolidWater;

                case ModItems.NeverIce:
                    return IconCustomNeverIce ?? IconDefaultNeverIce;

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public static Texture2D GetInWorldByType(ModItems type)
        {
            switch (type)
            {
                case ModItems.None:
                    throw new Exception("Cannot create an in world none item!");

                case ModItems.DoubleJump:
                    return CustomDoubleJump ?? DefaultDoubleJump;
                case ModItems.LongJump:
                    return CustomLongJump ?? DefaultLongJump;

                case ModItems.LowGravity:
                    return CustomLowGravity ?? DefaultLowGravity;
                case ModItems.HighGravity:
                    return CustomHighGravity ?? DefaultHighGravity;

                case ModItems.SlowFall:
                    return CustomSlowFall ?? DefaultSlowFall;

                case ModItems.NeverWater:
                    return CustomNeverWater ?? DefaultNeverWater;
                case ModItems.AlwaysWater:
                    return CustomAlwaysWater ?? DefaultAlwaysWater;
                case ModItems.SolidWater:
                    return CustomSolidWater ?? DefaultSolidWater;

                case ModItems.NeverIce:
                    return CustomNeverIce ?? DefaultNeverIce;

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
