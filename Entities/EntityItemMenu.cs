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

            var hitbox = this.Player.m_body.GetHitbox();
            var positionX = this.Player.m_body.Position.X + (hitbox.Width / 2);
            var positionY = this.Player.m_body.Position.Y + hitbox.Height - (hitbox.Height * 0.2f) +
                            (Camera.CurrentScreen * 360);

            Game1.spriteBatch.Draw(
                this.Texture,
                new Vector2(
                    (int)(positionX - (this.Texture.Width / 2) + Padding),
                    (int)(positionY - (this.Texture.Height / 2) - Padding)),
                Color.White);

            var items = ModEntry.DataItems.GetActiveNeighbors();
            var texturePrev = ModResources.GetIconByType(items[0]);
            var textureCurr = ModResources.GetIconByType(items[1]);
            var textureNext = ModResources.GetIconByType(items[2]);

            var iconWidthHalf = textureCurr.Width / 2;
            var iconHeightHalf = textureCurr.Height / 2;

            Game1.spriteBatch.Draw(
                texturePrev,
                new Vector2(
                    (int)(positionX - iconWidthHalf - textureCurr.Width - Padding),
                    (int)(positionY - iconHeightHalf)),
                new Color(128, 128, 128, 128));

            Game1.spriteBatch.Draw(
                textureCurr,
                new Vector2(
                    (int)(positionX - iconWidthHalf + Padding),
                    (int)(positionY - iconHeightHalf)),
                Color.White);

            Game1.spriteBatch.Draw(
                textureNext,
                new Vector2(
                    (int)(positionX + iconWidthHalf + textureCurr.Width + Padding),
                    (int)(positionY - iconHeightHalf)),
                new Color(128, 128, 128, 128));
        }
    }
}
