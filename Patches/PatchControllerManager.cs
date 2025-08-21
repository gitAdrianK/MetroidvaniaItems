// ReSharper disable InconsistentNaming

namespace MetroidvaniaItems.Patches
{
    using System.Linq;
    using HarmonyLib;
    using JetBrains.Annotations;
    using JumpKing.Controller;
    using Microsoft.Xna.Framework.Input;
    using Util;

    [HarmonyPatch(typeof(ControllerManager), nameof(ControllerManager.Update))]
    public static class PatchControllerManager
    {
        public static CustomPadInstance CustomPadInstance { get; } = new CustomPadInstance();
        private static IPad Pad { get; set; }

        [UsedImplicitly]
        public static void Postfix(ControllerManager __instance)
        {
            Pad = __instance.GetMain().GetPad();
            CustomPadInstance.LastState = CustomPadInstance.CurrentState;
            CustomPadInstance.CurrentState = GetPadState();
        }

        private static CustomPadInstance.CustomPadState GetPadState()
        {
            var pressedButtons = Pad.GetPressedButtons();
            return new CustomPadInstance.CustomPadState
            {
                OpenCloseItemsMenu = IsPressed(
                    pressedButtons, new[] { (int)Keys.Down })
                /*OpenCloseItemsMenu = IsPressed(
                    pressedButtons,
                    ModEntry.Preferences.KeyBindings[Preferences.BindingActions.OpenCloseItemsMenu]),
                ItemsMenuLeft = IsPressed(
                    pressedButtons,
                    ModEntry.Preferences.KeyBindings[Preferences.BindingActions.ItemsMenuLeft]),
                ItemsMenuRight = IsPressed(
                    pressedButtons,
                    ModEntry.Preferences.KeyBindings[Preferences.BindingActions.ItemsMenuRight])
                */
            };
        }

        private static bool IsPressed(int[] pressed, int[] bind) => pressed.Any(bind.Contains);
    }
}
