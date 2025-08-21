namespace MetroidvaniaItems.Entities
{
    using System;
    using EntityComponent;
    using JumpKing;
    using JumpKing.Player;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Util.Deserialization;
    using static ModItems;

    public class EntityItem : Entity
    {
        public EntityItem(Item item, PlayerEntity player, Texture2D texture)
        {
            this.Type = item.Type;
            this.Screen = item.Screen;
            this.Position = item.Position;
            this.Hitbox = new Rectangle((int)this.Position.X, (int)this.Position.Y, texture.Width, texture.Height);
            this.Player = player;
            this.Texture = texture;
        }

        private ItemType Type { get; }
        private int Screen { get; }
        private Vector2 Position { get; }
        private float Offset { get; set; }
        private Rectangle Hitbox { get; }
        private PlayerEntity Player { get; }
        private Texture2D Texture { get; }

        protected override void Update(float delta)
        {
            this.Offset += delta;

            if (!this.Hitbox.Intersects(this.Player.m_body.GetHitbox()))
            {
                return;
            }

            ModEntry.DataItems.Collected.Add(this.Type);
            Game1.instance.contentManager.audio.Plink.PlayOneShot();
            this.Destroy();
        }

        public override void Draw()
        {
            if (Camera.CurrentScreen != this.Screen
                || ModEntry.DataItems.Collected.Contains(this.Type))
            {
                return;
            }

            Game1.spriteBatch.Draw(
                this.Texture,
                new Vector2(this.Position.X, (float)(this.Position.Y + (Math.Sin(this.Offset) * 3f))),
                Color.White);
        }
    }
}
