// ReSharper disable InconsistentNaming

namespace MetroidvaniaItems.Patches
{
    using Behaviours;
    using HarmonyLib;
    using JetBrains.Annotations;
    using JumpKing.Level;
    using static ModItems.ItemType;

    [HarmonyPatch(typeof(WaterBlock), "get_canBlockPlayer")]
    public static class PatchWaterBlock
    {
        [UsedImplicitly]
        public static void Postfix(ref bool __result)
        {
            if (ModEntry.DataItems is null)
            {
                return;
            }

            if (ModEntry.DataItems.Active == WaterWalker && BehaviourWaterWalker.IsSolid)
            {
                __result = true;
            }
        }
    }
}
