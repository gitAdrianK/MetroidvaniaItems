// ReSharper disable InconsistentNaming
namespace MetroidvaniaItems.Patches
{
    using HarmonyLib;
    using JetBrains.Annotations;
    using JumpKing.Player;

    [HarmonyPatch(typeof(JumpState), "DoJump")]
    public static class PatchJumpState
    {
        [UsedImplicitly]
        public static void Postfix(JumpState __instance, float p_intensity)
        {
            if (ModEntry.DataItems is null)
            {
                return;
            }

            if (ModEntry.DataItems.Active != ModItems.ItemType.LongJump)
            {
                return;
            }

            __instance.body.Velocity.Y /= 2;
            __instance.body.Velocity.X *= 2;
        }
    }
}
