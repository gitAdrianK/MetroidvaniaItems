namespace MetroidvaniaItems.Util.Deserialization
{
    using Microsoft.Xna.Framework;
    using static ModItems;

    public class Item
    {
        public int Screen { get; set; }
        public Vector2 Position { get; set; }
        public ItemType Type { get; set; }
    }
}
