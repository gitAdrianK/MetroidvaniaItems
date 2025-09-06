// ReSharper disable InconsistentNaming

namespace MetroidvaniaItems.Patches
{
    using HarmonyLib;
    using JetBrains.Annotations;
    using JumpKing;

    [HarmonyPatch(typeof(WindManager), "get_CurrentVelocityRaw")]
    public static class PatchWindManager
    {
        [UsedImplicitly]
        public static void Postfix(ref float __result)
        {
            if (ModEntry.DataMetroidvania is null)
            {
                return;
            }

            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (ModEntry.DataMetroidvania.Active == ModItems.NeverWind)
            {
                __result = 0.0f;
            }
            else if (ModEntry.DataMetroidvania.Active == ModItems.ReverseWind)
            {
                __result = -__result;
            }
        }
    }
}
