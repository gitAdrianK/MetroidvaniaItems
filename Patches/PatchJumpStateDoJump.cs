// ReSharper disable InconsistentNaming

namespace MetroidvaniaItems.Patches
{
    using HarmonyLib;
    using JetBrains.Annotations;
    using JumpKing.Player;

    [HarmonyPatch(typeof(JumpState), "DoJump")]
    public static class PatchJumpStateDoJump
    {
        [UsedImplicitly]
        public static void Postfix(JumpState __instance)
        {
            if (ModEntry.DataMetroidvania is null)
            {
                return;
            }

            if (ModEntry.DataMetroidvania.Active != ModItems.LongJump)
            {
                return;
            }

            __instance.body.Velocity.Y /= 2;
            __instance.body.Velocity.X *= 2;
        }
    }
}
