namespace MetroidvaniaItems
{
    using System;
    using System.IO;
    using System.Reflection;
    using JumpKing;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public static class ModResources
    {
        private static Point SpritesheetCells { get; } = new Point(4, 4);

        // Menu
        public static Sprite DefaultMenu { get; private set; }
        public static Sprite CustomMenu { get; private set; }

        // None
        private static Sprite IconCustomNone { get; set; }
        private static Sprite DefaultNone { get; set; }

        // Double Jump
        private static Sprite IconCustomDoubleJump { get; set; }
        private static Sprite DefaultDoubleJump { get; set; }
        private static Sprite CustomDoubleJump { get; set; }

        // Long Jump
        private static Sprite IconCustomLongJump { get; set; }
        private static Sprite DefaultLongJump { get; set; }
        private static Sprite CustomLongJump { get; set; }

        // Low Gravity
        private static Sprite IconCustomLowGravity { get; set; }
        private static Sprite DefaultLowGravity { get; set; }
        private static Sprite CustomLowGravity { get; set; }

        // High Gravity
        private static Sprite IconCustomHighGravity { get; set; }
        private static Sprite DefaultHighGravity { get; set; }
        private static Sprite CustomHighGravity { get; set; }

        // Slow Fall
        private static Sprite IconCustomSlowFall { get; set; }
        private static Sprite DefaultSlowFall { get; set; }
        private static Sprite CustomSlowFall { get; set; }

        // Never Water
        private static Sprite IconCustomNeverWater { get; set; }
        private static Sprite DefaultNeverWater { get; set; }
        private static Sprite CustomNeverWater { get; set; }

        // Always Water
        private static Sprite IconCustomAlwaysWater { get; set; }
        private static Sprite DefaultAlwaysWater { get; set; }
        private static Sprite CustomAlwaysWater { get; set; }

        // Solid Water
        private static Sprite IconCustomSolidWater { get; set; }
        private static Sprite DefaultSolidWater { get; set; }
        private static Sprite CustomSolidWater { get; set; }

        // Frozen Water
        private static Sprite IconCustomFrozenWater { get; set; }
        private static Sprite DefaultFrozenWater { get; set; }
        private static Sprite CustomFrozenWater { get; set; }

        // Always Ice
        private static Sprite IconCustomNeverIce { get; set; }
        private static Sprite DefaultNeverIce { get; set; }
        private static Sprite CustomNeverIce { get; set; }

        public static void LoadDefaultTextures()
        {
            var contentManager = Game1.instance.contentManager;
            var dllPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
                          throw new InvalidOperationException();

            var spriteSheet = contentManager.Load<Texture2D>(Path.Combine(dllPath, "Sprites"));
            var spriteArray = SpriteChopUtilGrid(spriteSheet, SpritesheetCells, Vector2.Zero, spriteSheet.Bounds);

            // The first sprite is the menu, as such all items are their enum index + 1
            DefaultMenu = spriteArray[0];

            DefaultNone = spriteArray[(int)ModItems.None + 1];

            DefaultDoubleJump = spriteArray[(int)ModItems.DoubleJump + 1];
            DefaultLongJump = spriteArray[(int)ModItems.LongJump + 1];

            DefaultLowGravity = spriteArray[(int)ModItems.LowGravity + 1];
            DefaultHighGravity = spriteArray[(int)ModItems.HighGravity + 1];

            DefaultSlowFall = spriteArray[(int)ModItems.SlowFall + 1];

            DefaultNeverWater = spriteArray[(int)ModItems.NeverWater + 1];
            DefaultAlwaysWater = spriteArray[(int)ModItems.AlwaysWater + 1];
            DefaultSolidWater = spriteArray[(int)ModItems.SolidWater + 1];
            DefaultFrozenWater = spriteArray[(int)ModItems.FrozenWater + 1];

            DefaultNeverIce = spriteArray[(int)ModItems.NeverIce + 1];
        }

        public static void LoadCustomTextures()
        {
            var contentManager = Game1.instance.contentManager;
            var modFolderPath = Path.Combine(contentManager.root, "metroidvania");

            var uiPath = Path.Combine(modFolderPath, "ui");
            CustomMenu = GetOptionalSprite(contentManager, Path.Combine(uiPath, "Menu"));

            var iconPath = Path.Combine(modFolderPath, "icons");
            IconCustomNone = GetOptionalSprite(contentManager, Path.Combine(iconPath, nameof(ModItems.None)));

            IconCustomDoubleJump =
                GetOptionalSprite(contentManager, Path.Combine(iconPath, nameof(ModItems.DoubleJump)));
            IconCustomLongJump = GetOptionalSprite(contentManager, Path.Combine(iconPath, nameof(ModItems.LongJump)));

            IconCustomLowGravity =
                GetOptionalSprite(contentManager, Path.Combine(iconPath, nameof(ModItems.LowGravity)));
            IconCustomHighGravity =
                GetOptionalSprite(contentManager, Path.Combine(iconPath, nameof(ModItems.HighGravity)));

            IconCustomSlowFall = GetOptionalSprite(contentManager, Path.Combine(iconPath, nameof(ModItems.SlowFall)));

            IconCustomNeverWater =
                GetOptionalSprite(contentManager, Path.Combine(iconPath, nameof(ModItems.NeverWater)));
            IconCustomAlwaysWater =
                GetOptionalSprite(contentManager, Path.Combine(iconPath, nameof(ModItems.AlwaysWater)));
            IconCustomSolidWater =
                GetOptionalSprite(contentManager, Path.Combine(iconPath, nameof(ModItems.SolidWater)));
            IconCustomFrozenWater =
                GetOptionalSprite(contentManager, Path.Combine(iconPath, nameof(ModItems.FrozenWater)));

            IconCustomNeverIce = GetOptionalSprite(contentManager, Path.Combine(iconPath, nameof(ModItems.NeverIce)));

            var itemPath = Path.Combine(modFolderPath, "items");
            CustomDoubleJump = GetOptionalSprite(contentManager, Path.Combine(itemPath, nameof(ModItems.DoubleJump)));
            CustomLongJump = GetOptionalSprite(contentManager, Path.Combine(itemPath, nameof(ModItems.LongJump)));

            CustomLowGravity = GetOptionalSprite(contentManager, Path.Combine(itemPath, nameof(ModItems.LowGravity)));
            CustomHighGravity = GetOptionalSprite(contentManager, Path.Combine(itemPath, nameof(ModItems.HighGravity)));

            CustomSlowFall = GetOptionalSprite(contentManager, Path.Combine(itemPath, nameof(ModItems.SlowFall)));

            CustomNeverWater = GetOptionalSprite(contentManager, Path.Combine(itemPath, nameof(ModItems.NeverWater)));
            CustomAlwaysWater = GetOptionalSprite(contentManager, Path.Combine(itemPath, nameof(ModItems.AlwaysWater)));
            CustomSolidWater = GetOptionalSprite(contentManager, Path.Combine(itemPath, nameof(ModItems.SolidWater)));
            CustomFrozenWater = GetOptionalSprite(contentManager, Path.Combine(itemPath, nameof(ModItems.FrozenWater)));

            CustomNeverIce = GetOptionalSprite(contentManager, Path.Combine(itemPath, nameof(ModItems.NeverIce)));
        }

        private static Sprite[] SpriteChopUtilGrid(Texture2D texture, Point cells, Vector2 center, Rectangle source)
        {
            var cellWidth = source.Width / cells.X;
            var cellHeight = source.Height / cells.Y;
            var spriteArray = new Sprite[cells.X * cells.Y];
            for (var i = 0; i < cells.X; i++)
            {
                for (var j = 0; j < cells.Y; j++)
                {
                    spriteArray[i + (j * cells.X)] = Sprite.CreateSpriteWithCenter(
                        texture,
                        new Rectangle(
                            source.X + (cellWidth * i),
                            source.Y + (cellHeight * j),
                            cellWidth,
                            cellHeight),
                        center);
                }
            }

            return spriteArray;
        }

        private static Sprite GetOptionalSprite(JKContentManager contentManager, string texturePath)
            => File.Exists(texturePath + ".xnb")
                ? Sprite.CreateSprite(contentManager.Load<Texture2D>(texturePath))
                : null;

        public static Sprite GetIconByType(ModItems type)
        {
            switch (type)
            {
                case ModItems.None:
                    return IconCustomNone ?? DefaultNone;

                case ModItems.DoubleJump:
                    return IconCustomDoubleJump ?? DefaultDoubleJump;
                case ModItems.LongJump:
                    return IconCustomLongJump ?? DefaultLongJump;

                case ModItems.LowGravity:
                    return IconCustomLowGravity ?? DefaultLowGravity;
                case ModItems.HighGravity:
                    return IconCustomHighGravity ?? DefaultHighGravity;

                case ModItems.SlowFall:
                    return IconCustomSlowFall ?? DefaultSlowFall;

                case ModItems.NeverWater:
                    return IconCustomNeverWater ?? DefaultNeverWater;
                case ModItems.AlwaysWater:
                    return IconCustomAlwaysWater ?? DefaultAlwaysWater;
                case ModItems.SolidWater:
                    return IconCustomSolidWater ?? DefaultSolidWater;
                case ModItems.FrozenWater:
                    return IconCustomFrozenWater ?? DefaultFrozenWater;

                case ModItems.NeverIce:
                    return IconCustomNeverIce ?? DefaultNeverIce;

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public static Sprite GetInWorldByType(ModItems type)
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
                case ModItems.FrozenWater:
                    return CustomFrozenWater ?? DefaultFrozenWater;

                case ModItems.NeverIce:
                    return CustomNeverIce ?? DefaultNeverIce;

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
