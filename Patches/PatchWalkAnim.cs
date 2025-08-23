namespace MetroidvaniaItems.Patches
{
    using HarmonyLib;
    using JetBrains.Annotations;
    using JumpKing.Player;

    [HarmonyPatch(typeof(WalkAnim), "SetGroundSprite")]
    public static class PatchWalkAnim
    {
        [UsedImplicitly]
        public static bool Prefix() => !ModEntry.IsInMenu;
    }
}
