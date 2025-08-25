namespace MetroidvaniaItems.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using JumpKing;
    using JumpKing.SaveThread;
    using Microsoft.Xna.Framework;

    public class DataItems
    {
        public ModItems Active { get; set; } = ModItems.None;
        public ModItems Hovering { get; set; } = ModItems.None;
        public List<ModItems> Owned { get; private set; } = new List<ModItems> { ModItems.None };
        public List<Vector3> Collected { get; private set; } = new List<Vector3>();

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
                    Active = (ModItems)Enum.Parse(typeof(ModItems), root.Element("Active")?.Value ?? "None"),
                    Owned = root.Element("Owned")
                                ?.Elements("Item")
                                .Select(item => (ModItems)Enum.Parse(typeof(ModItems),
                                    item.Value))
                                .ToList() ??
                            throw new InvalidOperationException(),
                    Collected = root.Element("Collected")
                                    ?.Elements("Item")
                                    .Select(item => new Vector3(float.Parse(item.Element("X")
                                                                                ?.Value
                                                                            ?? throw new InvalidOperationException(),
                                            CultureInfo.InvariantCulture),
                                        float.Parse(item.Element("Y")
                                                        ?.Value
                                                    ?? throw new InvalidOperationException(),
                                            CultureInfo.InvariantCulture),
                                        int.Parse(item.Element("Screen")
                                                      ?.Value
                                                  ?? throw new InvalidOperationException())))
                                    .ToList()
                                ?? throw new InvalidOperationException()
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
                new XElement("Owned",
                    this.Owned.Select(c => new XElement("Item", c))),
                new XElement("Collected",
                    this.Collected.Select(vec =>
                        new XElement("Item",
                            new XElement("X", vec.X),
                            new XElement("Y", vec.Y),
                            new XElement("Screen", vec.Z)
                        )
                    ))
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

        public ModItems[] GetNeighbors(ModItems item)
        {
            var index = this.Owned.FindIndex(entry => entry.Equals(item));
            return new[]
            {
                this.Owned[(index - 1 + this.Owned.Count) % this.Owned.Count], this.Owned[index],
                this.Owned[(index + 1 + this.Owned.Count) % this.Owned.Count]
            };
        }
    }
}
