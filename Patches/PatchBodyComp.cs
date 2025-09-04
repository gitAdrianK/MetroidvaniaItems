// ReSharper disable InconsistentNaming

namespace MetroidvaniaItems.Patches
{
    using HarmonyLib;
    using JetBrains.Annotations;
    using JumpKing.Player;

    [HarmonyPatch(typeof(BodyComp), nameof(BodyComp.GetMultipliers))]
    public static class PatchBodyComp
    {
        [UsedImplicitly]
        public static void Postfix(ref float __result)
        {
            if (ModEntry.DataItems is null)
            {
                return;
            }

            if (ModEntry.DataItems.Active == ModItems.FastHighGravity)
            {
                __result *= 1.28f;
            }
        }
    }
}
