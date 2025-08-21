namespace MetroidvaniaItems.Menus
{
    using BehaviorTree;
    using EntityComponent;
    using EntityComponent.BT;
    using JumpKing;
    using JumpKing.Controller;
    using JumpKing.PauseMenu;
    using JumpKing.PauseMenu.BT;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using static Preferences;


    public class MenuBindDisplay : EntityBTNode, IMenuItem, UnSelectable
    {
        private EBinding Button { get; }
        private static SpriteFont Font => Game1.instance.contentManager.font.MenuFontSmall;

        public MenuBindDisplay(Entity entity, EBinding button)
            : base(entity) =>
            this.Button = button;

        public void Draw(int x, int y, bool selected)
        {
            var m_pad = ControllerManager.instance.GetMain();

            // key type
            var buttonName = this.Button + " : ";

            var pad = m_pad.GetPad();

            MenuItemHelper.Draw(x, y, buttonName, Color.Gray, Font);

            var x2 = this.GetSize().X;
            foreach (var bind in ModEntry.Preferences.KeyBindings[this.Button])
            {
                x += (int)(x2 / 3f);
                buttonName = pad.ButtonToString(bind);

                buttonName = this.FormatString(buttonName);
                MenuItemHelper.Draw(x, y, buttonName, Color.Gray, Font);
            }

            if (ModEntry.Preferences.KeyBindings[this.Button].Length != 0)
            {
                return;
            }

            x += (int)(x2 / 3f);
            buttonName = "-";
            MenuItemHelper.Draw(x + (int)(x2 / 3f * 1f), y, buttonName, Color.Gray, Font);
        }

        private string FormatString(string formatString)
        {
            var num = this.GetSize().X / 3;
            if (MenuItemHelper.GetSize(formatString, Font).X <= num)
            {
                return formatString;
            }

            while (MenuItemHelper.GetSize(formatString, Font).X > num)
            {
                formatString = formatString.Substring(0, formatString.Length - 1);
            }

            return formatString.Substring(0, formatString.Length - 1) + "*";
        }

        public Point GetSize() => MenuItemHelper.GetSize("xbox 360 controller 1____         ", Font);

        protected override BTresult MyRun(TickData data) => BTresult.Failure;
    }
}
