// ReSharper disable InconsistentNaming

namespace MetroidvaniaItems.Patches
{
    using HarmonyLib;
    using JetBrains.Annotations;
    using JumpKing.BlockBehaviours;

    public static class PatchWaterBlockBehaviour
    {
        [UsedImplicitly]
        [HarmonyPatch(typeof(WaterBlockBehaviour), "get_IsPlayerOnBlock")]
        [HarmonyPostfix]
        public static void get_IsPlayerOnBlock(ref bool __result)
        {
            if (ModEntry.DataItems is null)
            {
                return;
            }

            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (ModEntry.DataItems.Active == ModItems.NeverWater)
            {
                __result = false;
            }
            else if (ModEntry.DataItems.Active == ModItems.AlwaysWater)
            {
                __result = true;
            }
        }
    }
}
