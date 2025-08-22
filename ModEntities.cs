namespace MetroidvaniaItems
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using Entities;
    using JumpKing;
    using JumpKing.Player;
    using Microsoft.Xna.Framework;
    using Util.Deserialization;
    using static ModItems;

    public static class ModEntities
    {
        public static HashSet<ItemType> LoadEntities(PlayerEntity player)
        {
            var loadedItems = new HashSet<ItemType>();

            var contentManager = Game1.instance.contentManager;
            var filePath = Path.Combine(contentManager.root, "props", "metroidvania", "items.xml");

            var doc = XDocument.Load(filePath);
            if (doc.Root == null)
            {
                return loadedItems;
            }

            var collection = doc.Root
                ?.Elements("Item")
                .Select(item => new Item
                {
                    Screen = int.Parse(item.Element("Screen")?.Value ?? throw new InvalidOperationException()) - 1,
                    Position = new Vector2
                    {
                        X = float.Parse(item.Element("Position")
                                            ?.Element("X")
                                            ?.Value ??
                                        throw new InvalidOperationException(),
                            CultureInfo.InvariantCulture),
                        Y = float.Parse(item.Element("Position")
                                            ?.Element("Y")
                                            ?.Value ??
                                        throw new InvalidOperationException(),
                            CultureInfo.InvariantCulture)
                    },
                    Type = (ItemType)Enum.Parse(typeof(ItemType),
                        item.Element("Type")?.Value ?? throw new InvalidOperationException())
                })
                .ToList() ?? new List<Item>();

            _ = new EntityItemMenu(player, ModResources.CustomMenu ?? ModResources.DefaultMenu);
            foreach (var item in collection)
            {
                loadedItems.Add(item.Type);
                if (ModEntry.DataItems.Collected.Contains(new Vector3(item.Position, item.Screen)))
                {
                    continue;
                }

                _ = new EntityItem(item, player, ModResources.GetInWorldByType(item.Type));
            }

            return loadedItems;
        }
    }
}
