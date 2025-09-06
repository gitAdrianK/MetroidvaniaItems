// ReSharper disable PossibleLossOfFraction

namespace MetroidvaniaItems.Entities
{
    using System;
    using Data;
    using EntityComponent;
    using JumpKing;
    using JumpKing.Player;
    using Microsoft.Xna.Framework;
    using Util;

    public class EntityItemMenu : Entity
    {
        public EntityItemMenu(DataMetroidvania data, PlayerEntity player, Sprite spriteMenu, Sprite spriteSwitch)
        {
            this.Data = data;
            this.Player = player;
            this.SpriteMenu = spriteMenu;
            this.SpriteSwitch = spriteSwitch;
            this.Width = spriteMenu.source.Width;
            this.Height = spriteMenu.source.Height;
        }

        private DataMetroidvania Data { get; }
        private PlayerEntity Player { get; }
        private Sprite SpriteMenu { get; }
        private Sprite SpriteSwitch { get; }
        private int Width { get; }
        private int Height { get; }

        protected override void Update(float delta)
        {
            switch (this.Data.MenuState)
            {
                case MenuState.Closed:
                case MenuState.Select:
                    return;
                case MenuState.Previous:
                    this.Data.MenuDuration -= delta;
                    if (this.Data.MenuDuration <= 0.0f)
                    {
                        this.Data.MenuState = MenuState.Closed;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void Draw()
        {
            switch (this.Data.MenuState)
            {
                case MenuState.Closed:
                    return;
                case MenuState.Select:
                    this.DrawSelect();
                    break;
                case MenuState.Previous:
                    this.DrawPrevious();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void DrawSelect()
        {
            var hitbox = this.Player.m_body.GetHitbox();
            var positionX = this.Player.m_body.Position.X + (hitbox.Width / 2);
            var positionY = this.Player.m_body.Position.Y + hitbox.Height - (hitbox.Height * 0.2f) +
                            (Camera.CurrentScreen * 360);

            var items = ModEntry.DataMetroidvania.GetNeighbours(ModEntry.DataMetroidvania.Hovering);
            var spritePrev = ModResources.GetIconByType(items[0]);
            var spriteCurr = ModResources.GetIconByType(items[1]);
            var spriteNext = ModResources.GetIconByType(items[2]);

            var iconWidthHalf = this.Width / 2;
            var iconHeightHalf = this.Height / 2;

            this.SpriteMenu.Draw(new Vector2(
                (int)(positionX - iconWidthHalf),
                (int)(positionY - iconHeightHalf)));

            spritePrev.Draw(new Vector2(
                (int)(positionX - iconWidthHalf - this.Width),
                (int)(positionY - iconHeightHalf)));

            spriteCurr.Draw(new Vector2(
                (int)(positionX - iconWidthHalf),
                (int)(positionY - iconHeightHalf)));

            spriteNext.Draw(new Vector2(
                (int)(positionX + iconWidthHalf),
                (int)(positionY - iconHeightHalf)));
        }

        private void DrawPrevious()
        {
            var hitbox = this.Player.m_body.GetHitbox();
            var positionX = this.Player.m_body.Position.X + (hitbox.Width / 2);
            var positionY = this.Player.m_body.Position.Y + hitbox.Height - (hitbox.Height * 0.2f) +
                            (Camera.CurrentScreen * 360);

            var iconWidthHalf = this.Width / 2;
            var iconHeightHalf = this.Height / 2;

            var spriteNone = ModResources.GetIconByType(ModItems.None);
            var spritePrev = ModResources.GetIconByType(this.Data.Previous);

            if (this.Data.Active == ModItems.None)
            {
                this.SpriteMenu.Draw(new Vector2(
                    (int)(positionX - iconWidthHalf - this.Width),
                    (int)(positionY - iconHeightHalf)));
            }
            else
            {
                this.SpriteMenu.Draw(new Vector2(
                    (int)(positionX + iconWidthHalf),
                    (int)(positionY - iconHeightHalf)));
            }

            spriteNone.Draw(new Vector2(
                (int)(positionX - iconWidthHalf - this.Width),
                (int)(positionY - iconHeightHalf)));

            this.SpriteSwitch.Draw(new Vector2(
                (int)(positionX - iconWidthHalf),
                (int)(positionY - iconHeightHalf)));

            spritePrev.Draw(new Vector2(
                (int)(positionX + iconWidthHalf),
                (int)(positionY - iconHeightHalf)));
        }
    }
}
