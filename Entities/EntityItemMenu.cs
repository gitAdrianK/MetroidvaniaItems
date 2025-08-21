// ReSharper disable PossibleLossOfFraction

namespace MetroidvaniaItems.Entities
{
    using EntityComponent;
    using JumpKing;
    using JumpKing.Player;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class EntityItemMenu : Entity
    {
        public EntityItemMenu(PlayerEntity player, Texture2D texture)
        {
            this.Player = player;
            this.Texture = texture;
        }

        private PlayerEntity Player { get; }
        private Texture2D Texture { get; }
        private static int Padding => 2;

        public override void Draw()
        {
            if (!ModEntry.IsInMenu)
            {
                return;
            }

            // TODO: Make pretty :D

            var hitbox = this.Player.m_body.GetHitbox();
            var positionX = this.Player.m_body.Position.X + (hitbox.Width / 2);
            var positionY = this.Player.m_body.Position.Y + hitbox.Height;

            Game1.spriteBatch.Draw(
                this.Texture,
                new Vector2(
                    positionX - (this.Texture.Width / 2),
                    positionY - this.Texture.Height - 2),
                Color.White);

            var items = ModEntry.DataItems.GetActiveNeighbors();
            var texturePrev = ModResources.GetIconByType(items[0]);
            var textureCurr = ModResources.GetIconByType(items[1]);
            var textureNext = ModResources.GetIconByType(items[2]);

            Game1.spriteBatch.Draw(
                texturePrev,
                new Vector2(
                    positionX - textureCurr.Width - Padding - (texturePrev.Width /2),
                    positionY -  texturePrev.Height - Padding),
                Color.White);

            Game1.spriteBatch.Draw(
                textureCurr,
                new Vector2(
                    positionX - (textureCurr.Width /2) + Padding,
                    positionY -  textureCurr.Height - Padding),
                Color.White);

            Game1.spriteBatch.Draw(
                textureNext,
                new Vector2(
                    positionX + textureCurr.Width + Padding - (texturePrev.Width /2),
                    positionY -  textureNext.Height - Padding),
                Color.White);
        }
    }
}
