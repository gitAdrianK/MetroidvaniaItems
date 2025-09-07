// ReSharper disable InconsistentNaming

namespace MetroidvaniaItems.Patches
{
    using HarmonyLib;
    using JetBrains.Annotations;
    using JumpKing.BlockBehaviours;

    [HarmonyPatch(typeof(WaterBlockBehaviour), nameof(WaterBlockBehaviour.IsPlayerOnBlock), MethodType.Getter)]
    public static class PatchWaterBlockBehaviour
    {
        [UsedImplicitly]
        public static void Postfix(ref bool __result)
        {
            if (ModEntry.DataMetroidvania is null)
            {
                return;
            }

            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (ModEntry.DataMetroidvania.Active == ModItems.NeverWater)
            {
                __result = false;
            }
            else if (ModEntry.DataMetroidvania.Active == ModItems.AlwaysWater)
            {
                __result = true;
            }
        }
    }
}
