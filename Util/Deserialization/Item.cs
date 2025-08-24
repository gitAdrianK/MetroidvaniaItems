namespace MetroidvaniaItems.Util.Deserialization
{
    using Microsoft.Xna.Framework;

    public class Item
    {
        public int Screen { get; set; }
        public Vector2 Position { get; set; }
        public ModItems Type { get; set; }
    }
}
