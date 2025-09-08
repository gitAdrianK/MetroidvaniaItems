namespace MetroidvaniaItems
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;
    using JumpKing;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Util.Deserialization;

    public static class ModResources
    {
        // Rust macros could make this prettier, probably

        private static Point SpritesheetCells { get; } = new Point(4, 4);

        private static Dictionary<ModItems, string> CustomDisplayNames { get; set; } =
            new Dictionary<ModItems, string>();

        // Menu
        public static Sprite DefaultMenu { get; private set; }
        public static Sprite CustomMenu { get; private set; }

        // Switch
        public static Sprite DefaultSwitch { get; private set; }
        public static Sprite CustomSwitch { get; private set; }

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

        // Fast High Gravity
        private static Sprite IconCustomFastHighGravity { get; set; }
        private static Sprite DefaultFastHighGravity { get; set; }
        private static Sprite CustomFastHighGravity { get; set; }

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

        // Never Wind
        private static Sprite IconCustomNeverWind { get; set; }
        private static Sprite DefaultNeverWind { get; set; }
        private static Sprite CustomNeverWind { get; set; }

        // Reverse Wind
        private static Sprite IconCustomReverseWind { get; set; }
        private static Sprite DefaultReverseWind { get; set; }
        private static Sprite CustomReverseWind { get; set; }

        public static void LoadDefaultTextures()
        {
            var contentManager = Game1.instance.contentManager;
            var dllPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
                          throw new InvalidOperationException();

            var spriteSheet = contentManager.Load<Texture2D>(Path.Combine(dllPath, "Sprites"));
            var spriteArray = SpriteChopUtilGrid(spriteSheet, SpritesheetCells, Vector2.Zero, spriteSheet.Bounds);

            // The first sprites are menu and switch, as such all items are their enum index + 2
            DefaultMenu = spriteArray[0];
            DefaultSwitch = spriteArray[1];

            DefaultNone = spriteArray[(int)ModItems.None + 2];

            DefaultDoubleJump = spriteArray[(int)ModItems.DoubleJump + 2];
            DefaultLongJump = spriteArray[(int)ModItems.LongJump + 2];

            DefaultLowGravity = spriteArray[(int)ModItems.LowGravity + 2];
            DefaultHighGravity = spriteArray[(int)ModItems.HighGravity + 2];
            DefaultFastHighGravity = spriteArray[(int)ModItems.FastHighGravity + 2];

            DefaultSlowFall = spriteArray[(int)ModItems.SlowFall + 2];

            DefaultNeverWater = spriteArray[(int)ModItems.NeverWater + 2];
            DefaultAlwaysWater = spriteArray[(int)ModItems.AlwaysWater + 2];
            DefaultSolidWater = spriteArray[(int)ModItems.SolidWater + 2];
            DefaultFrozenWater = spriteArray[(int)ModItems.FrozenWater + 2];

            DefaultNeverIce = spriteArray[(int)ModItems.NeverIce + 2];

            DefaultNeverWind = spriteArray[(int)ModItems.NeverWind + 2];
            DefaultReverseWind = spriteArray[(int)ModItems.ReverseWind + 2];
        }

        public static void LoadCustomTextures()
        {
            var contentManager = Game1.instance.contentManager;
            var modFolderPath = Path.Combine(contentManager.root, "metroidvania");

            var uiPath = Path.Combine(modFolderPath, "ui");
            CustomMenu = LoadOptionalSprite(contentManager, Path.Combine(uiPath, "Menu"));
            CustomSwitch = LoadOptionalSprite(contentManager, Path.Combine(uiPath, "Switch"));

            var iconPath = Path.Combine(modFolderPath, "icons");
            IconCustomNone = LoadOptionalSprite(contentManager, Path.Combine(iconPath, nameof(ModItems.None)));

            IconCustomDoubleJump =
                LoadOptionalSprite(contentManager, Path.Combine(iconPath, nameof(ModItems.DoubleJump)));
            IconCustomLongJump = LoadOptionalSprite(contentManager, Path.Combine(iconPath, nameof(ModItems.LongJump)));

            IconCustomLowGravity =
                LoadOptionalSprite(contentManager, Path.Combine(iconPath, nameof(ModItems.LowGravity)));
            IconCustomHighGravity =
                LoadOptionalSprite(contentManager, Path.Combine(iconPath, nameof(ModItems.HighGravity)));
            IconCustomFastHighGravity =
                LoadOptionalSprite(contentManager, Path.Combine(iconPath, nameof(ModItems.FastHighGravity)));

            IconCustomSlowFall = LoadOptionalSprite(contentManager, Path.Combine(iconPath, nameof(ModItems.SlowFall)));

            IconCustomNeverWater =
                LoadOptionalSprite(contentManager, Path.Combine(iconPath, nameof(ModItems.NeverWater)));
            IconCustomAlwaysWater =
                LoadOptionalSprite(contentManager, Path.Combine(iconPath, nameof(ModItems.AlwaysWater)));
            IconCustomSolidWater =
                LoadOptionalSprite(contentManager, Path.Combine(iconPath, nameof(ModItems.SolidWater)));
            IconCustomFrozenWater =
                LoadOptionalSprite(contentManager, Path.Combine(iconPath, nameof(ModItems.FrozenWater)));

            IconCustomNeverIce = LoadOptionalSprite(contentManager, Path.Combine(iconPath, nameof(ModItems.NeverIce)));

            IconCustomNeverWind = LoadOptionalSprite(contentManager, Path.Combine(iconPath, nameof(ModItems.NeverWind)));
            IconCustomReverseWind =
                LoadOptionalSprite(contentManager, Path.Combine(iconPath, nameof(ModItems.ReverseWind)));

            var itemPath = Path.Combine(modFolderPath, "items");
            CustomDoubleJump = LoadOptionalSprite(contentManager, Path.Combine(itemPath, nameof(ModItems.DoubleJump)));
            CustomLongJump = LoadOptionalSprite(contentManager, Path.Combine(itemPath, nameof(ModItems.LongJump)));

            CustomLowGravity = LoadOptionalSprite(contentManager, Path.Combine(itemPath, nameof(ModItems.LowGravity)));
            CustomHighGravity = LoadOptionalSprite(contentManager, Path.Combine(itemPath, nameof(ModItems.HighGravity)));
            CustomFastHighGravity =
                LoadOptionalSprite(contentManager, Path.Combine(itemPath, nameof(ModItems.FastHighGravity)));

            CustomSlowFall = LoadOptionalSprite(contentManager, Path.Combine(itemPath, nameof(ModItems.SlowFall)));

            CustomNeverWater = LoadOptionalSprite(contentManager, Path.Combine(itemPath, nameof(ModItems.NeverWater)));
            CustomAlwaysWater = LoadOptionalSprite(contentManager, Path.Combine(itemPath, nameof(ModItems.AlwaysWater)));
            CustomSolidWater = LoadOptionalSprite(contentManager, Path.Combine(itemPath, nameof(ModItems.SolidWater)));
            CustomFrozenWater = LoadOptionalSprite(contentManager, Path.Combine(itemPath, nameof(ModItems.FrozenWater)));

            CustomNeverIce = LoadOptionalSprite(contentManager, Path.Combine(itemPath, nameof(ModItems.NeverIce)));

            CustomNeverWind = LoadOptionalSprite(contentManager, Path.Combine(itemPath, nameof(ModItems.NeverWind)));
            CustomReverseWind = LoadOptionalSprite(contentManager, Path.Combine(itemPath, nameof(ModItems.ReverseWind)));
        }

        public static void LoadCustomDisplayNames()
        {
            var contentManager = Game1.instance.contentManager;
            var xmlFile = Path.Combine(contentManager.root, "metroidvania", "names.xml");
            if (!File.Exists(xmlFile))
            {
                return;
            }

            var doc = XDocument.Load(xmlFile);
            if (doc.Root == null)
            {
                return;
            }

            CustomDisplayNames = doc.Root
                ?.Elements("Name")
                .Select(name =>
                {
                    if (!Enum.TryParse<ModItems>(name.Element("Type")?.Value, out var resultEnum))
                    {
                        return null;
                    }

                    var displayName = name.Element("DisplayName")?.Value;
                    return displayName is null ? null : new NameMapping { Type = resultEnum, Name = displayName };
                })
                .Where(mapping => !(mapping is null))
                .ToDictionary(mapping => mapping.Type, mapping => mapping.Name);
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

        private static Sprite LoadOptionalSprite(JKContentManager contentManager, string texturePath)
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
                case ModItems.FastHighGravity:
                    return IconCustomFastHighGravity ?? DefaultFastHighGravity;

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

                case ModItems.NeverWind:
                    return IconCustomNeverWind ?? DefaultNeverWind;
                case ModItems.ReverseWind:
                    return IconCustomReverseWind ?? DefaultReverseWind;

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
                case ModItems.FastHighGravity:
                    return CustomFastHighGravity ?? DefaultFastHighGravity;

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

                case ModItems.NeverWind:
                    return CustomNeverWind ?? DefaultNeverWind;
                case ModItems.ReverseWind:
                    return CustomReverseWind ?? DefaultReverseWind;

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public static string GetDisplayNameByType(ModItems type) =>
            CustomDisplayNames.TryGetValue(type, out var name)
                ? name
                : Regex.Replace(type.ToString(), "([a-z])([A-Z])", "$1 $2");
    }
}
