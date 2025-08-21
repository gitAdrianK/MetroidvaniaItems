namespace MetroidvaniaItems
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework.Input;

    public sealed class Preferences : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public enum EBinding
        {
            Menu
        }

        private Dictionary<EBinding, int[]> keyBinds = new Dictionary<EBinding, int[]>
        {
            { EBinding.Menu, new[] { (int)Keys.Down } }
        };

        public void ForceUpdate() => this.OnPropertyChanged();

        public Dictionary<EBinding, int[]> KeyBindings
        {
            get => this.keyBinds;
            set
            {
                this.keyBinds = value;
                this.OnPropertyChanged();
            }
        }

        public void SaveToFile(string filePath)
        {
            var doc = new XElement("Preferences",
                new XElement("KeyBinds",
                    this.keyBinds.Select(kv =>
                        new XElement("KeyBind",
                            new XAttribute("Binding", kv.Key),
                            kv.Value.Select(v => new XElement("Key", v))
                        )
                    )
                )
            );

            using (var fs = new FileStream(
                       filePath,
                       FileMode.Create,
                       FileAccess.Write,
                       FileShare.None))
            {
                doc.Save(fs);
            }
        }

        public static Preferences LoadFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new Preferences();
            }

            var doc = XDocument.Load(filePath);

            return new Preferences
            {
                KeyBindings = doc.Root?.Element("KeyBinds")
                    ?.Elements("KeyBind")
                    .ToDictionary(
                        value => (EBinding)Enum.Parse(typeof(EBinding),
                            value.Attribute("Binding")?.Value ?? throw new InvalidOperationException()),
                        key => key.Elements("Key").Select(k => int.Parse(k.Value)).ToArray())
            };
        }
    }
}
