namespace MetroidvaniaItems.Util
{
    public class CustomPadInstance
    {
        public CustomPadState LastState { get; set; }
        public CustomPadState CurrentState { get; set; }

        public CustomPadState GetPressed()
            => new CustomPadState
            {
                OpenCloseItemsMenu = !this.LastState.OpenCloseItemsMenu && this.CurrentState.OpenCloseItemsMenu
            };

        public struct CustomPadState
        {
            public bool OpenCloseItemsMenu { get; set; }
        }
    }
}
