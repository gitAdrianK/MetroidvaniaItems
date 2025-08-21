namespace MetroidvaniaItems.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using JumpKing;
    using JumpKing.SaveThread;
    using static ModItems;

    public class DataItems
    {
        public ItemType Active { get; set; } = ItemType.None;
        public List<ItemType> Collected { get; private set; } = new List<ItemType> { ItemType.None };

        public static DataItems ReadFromFile()
        {
            var file = Path.Combine(
                Game1.instance.contentManager.root,
                "zebrasSaves",
                "metroidvaniaItems.sav");

            if (SaveManager.instance.IsNewGame || !File.Exists(file))
            {
                return new DataItems();
            }

            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var doc = XDocument.Load(fs);
                var root = doc.Root;
                if (root == null)
                {
                    return new DataItems();
                }

                return new DataItems
                {
                    Active = (ItemType)Enum.Parse(typeof(ItemType), root.Element("Active")?.Value ?? "None"),
                    Collected = root.Element("Collected")
                        ?.Elements("Item")
                        .Select(item => (ItemType)Enum.Parse(typeof(ItemType),
                            item.Value))
                        .ToList() ?? throw new InvalidOperationException()
                };
            }
        }

        /// <summary>
        ///     Saves the data to file.
        /// </summary>
        public void SaveToFile()
        {
            var path = Path.Combine(
                Game1.instance.contentManager.root,
                "zebrasSaves");
            if (!Directory.Exists(path))
            {
                _ = Directory.CreateDirectory(path);
            }

            var doc = new XElement("ItemData",
                new XElement("Active", this.Active),
                new XElement("Collected",
                    this.Collected.Select(c => new XElement("Item", c)))
            );

            using (var fs = new FileStream(
                       Path.Combine(path, "metroidvaniaItems.sav"),
                       FileMode.Create,
                       FileAccess.Write,
                       FileShare.None))
            {
                doc.Save(fs);
            }
        }

        public ItemType[] GetActiveNeighbors()
        {
            var collected = this.Collected;
            var active = collected.FindIndex(entry => entry.Equals(this.Active));
            return new[]
            {
                collected[(active - 1 + collected.Count) % collected.Count],
                collected[active],
                collected[(active + 1 + collected.Count) % collected.Count]
            };
        }
    }
}
