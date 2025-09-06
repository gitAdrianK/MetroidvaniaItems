namespace MetroidvaniaItems.Patches
{
    using HarmonyLib;
    using JetBrains.Annotations;
    using JumpKing.Player;
    using Util;

    [HarmonyPatch(typeof(WalkAnim), "SetGroundSprite")]
    public static class PatchWalkAnim
    {
        [UsedImplicitly]
        public static bool Prefix()
        {
            if (ModEntry.DataMetroidvania is null)
            {
                return true;
            }

            return ModEntry.DataMetroidvania.MenuState != MenuState.Select;
        }
    }
}
