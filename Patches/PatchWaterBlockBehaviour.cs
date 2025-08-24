// ReSharper disable InconsistentNaming

namespace MetroidvaniaItems.Patches
{
    using HarmonyLib;
    using JetBrains.Annotations;
    using JumpKing.BlockBehaviours;

    [HarmonyPatch(typeof(WaterBlockBehaviour), "get_IsPlayerOnBlock")]
    public static class PatchWaterBlockBehaviour
    {
        [UsedImplicitly]
        public static void Postfix(ref bool __result)
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
