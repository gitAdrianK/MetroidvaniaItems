namespace MetroidvaniaItems.Entities
{
    using System;
    using EntityComponent;
    using JumpKing;
    using JumpKing.Player;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Util.Deserialization;

    public class EntityItem : Entity
    {
        public EntityItem(Item item, PlayerEntity player)
        {
            this.Type = item.Type;
            this.Screen = item.Screen;
            this.Position = item.Position;

            this.Texture = ModResources.GetInWorldByType(item.Type);

            this.Hitbox = new Rectangle((int)this.Position.X, (int)this.Position.Y - (item.Screen * 360),
                this.Texture.Width,
                this.Texture.Height);

            this.Player = player;
        }

        private ModItems Type { get; }
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

            var data = ModEntry.DataItems;
            if (!data.Owned.Contains(this.Type))
            {
                data.Owned.Add(this.Type);
            }

            data.Collected.Add(new Vector3(this.Position, this.Screen));

            Game1.instance.contentManager.audio.Plink.PlayOneShot();
            this.Destroy();
        }

        public override void Draw()
        {
            if (Camera.CurrentScreen != this.Screen)
            {
                return;
            }

            Game1.spriteBatch.Draw(
                this.Texture,
                new Vector2(this.Position.X, (float)(this.Position.Y + (Math.Sin(this.Offset * 2) * 2))),
                Color.White);
        }
    }
}
