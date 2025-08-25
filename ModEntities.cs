namespace MetroidvaniaItems
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;
    using Entities;
    using JumpKing.Player;
    using Microsoft.Xna.Framework;
    using Util.Deserialization;

    public static class ModEntities
    {
        public static HashSet<ModItems> LoadEntities(string xmlFile, PlayerEntity player)
        {
            var loadedItems = new HashSet<ModItems>();

            var doc = XDocument.Load(xmlFile);
            if (doc.Root == null)
            {
                return loadedItems;
            }

            var collection = doc.Root
                ?.Elements("Item")
                .Select(item =>
                {
                    if (int.TryParse(item.Element("Screen")?.Value, out var resultInt))
                    {
                        return null;
                    }

                    if (float.TryParse(item.Element("Position")?.Element("X")?.Value, NumberStyles.Float,
                            CultureInfo.InvariantCulture, out var resultX))
                    {
                        return null;
                    }

                    if (float.TryParse(item.Element("Position")?.Element("Y")?.Value, NumberStyles.Float,
                            CultureInfo.InvariantCulture, out var resultY))
                    {
                        return null;
                    }

                    if (!Enum.TryParse(item.Element("Type")?.Value, out ModItems resultEnum))
                    {
                        return null;
                    }

                    return new Item
                    {
                        Screen = resultInt - 1,
                        Position = new Vector2 { X = resultX, Y = resultY },
                        Type = resultEnum
                    };
                })
                .Where(item => item != null)
                .ToList() ?? new List<Item>();

            _ = new EntityItemMenu(player, ModResources.CustomMenu ?? ModResources.DefaultMenu);
            foreach (var item in collection)
            {
                loadedItems.Add(item.Type);
                if (ModEntry.DataItems.Collected.Contains(new Vector3(item.Position, item.Screen)))
                {
                    continue;
                }

                _ = new EntityItem(item, player);
            }

            return loadedItems;
        }
    }
}
