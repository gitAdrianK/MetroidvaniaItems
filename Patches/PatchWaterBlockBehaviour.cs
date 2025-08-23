// ReSharper disable InconsistentNaming
namespace MetroidvaniaItems.Patches
{
    using System;
    using System.Diagnostics;
    using HarmonyLib;
    using JetBrains.Annotations;
    using JumpKing.BlockBehaviours;

    [HarmonyPatch(typeof(WaterBlockBehaviour), "get_IsPlayerOnBlock")]
    public static class PatchWaterBlockBehaviour
    {
        [UsedImplicitly]
        public static void Postfix(WaterBlockBehaviour __instance, ref bool __result)
        {
            if (ModEntry.DataItems is null)
            {
                return;
            }

            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (ModEntry.DataItems.Active == ModItems.ItemType.Sponge)
            {
                __result = false;
            }
            else if (ModEntry.DataItems.Active == ModItems.ItemType.WaterBoots)
            {
                __result = true;
            }
        }
    }
}
