// ReSharper disable PossibleLossOfFraction

namespace MetroidvaniaItems.Entities
{
    using EntityComponent;
    using JumpKing;
    using JumpKing.Player;
    using Microsoft.Xna.Framework;

    public class EntityItemMenu : Entity
    {
        public EntityItemMenu(PlayerEntity player, Sprite spriteMenu)
        {
            this.Player = player;
            this.SpriteMenu = spriteMenu;
            this.Width = spriteMenu.source.Width;
            this.Height = spriteMenu.source.Height;
        }

        private PlayerEntity Player { get; }
        private Sprite SpriteMenu { get; }
        private int Width { get; }
        private int Height { get; }
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

            var items = ModEntry.DataItems.GetNeighbors(ModEntry.DataItems.Hovering);
            var spritePrev = ModResources.GetIconByType(items[0]);
            var spriteCurr = ModResources.GetIconByType(items[1]);
            var spriteNext = ModResources.GetIconByType(items[2]);

            var iconWidthHalf = this.Width / 2;
            var iconHeightHalf = this.Height / 2;

            this.SpriteMenu.Draw(new Vector2(
                (int)(positionX - (this.Width / 2) + Padding),
                (int)(positionY - (this.Height / 2) - Padding)));

            spritePrev.Draw(new Vector2(
                (int)(positionX - iconWidthHalf - this.Width - Padding),
                (int)(positionY - iconHeightHalf) - Padding));

            spriteCurr.Draw(new Vector2(
                (int)(positionX - iconWidthHalf + Padding),
                (int)(positionY - iconHeightHalf) - Padding));

            spriteNext.Draw(new Vector2(
                (int)(positionX + iconWidthHalf + this.Width + Padding),
                (int)(positionY - iconHeightHalf) - Padding));
        }
    }
}
