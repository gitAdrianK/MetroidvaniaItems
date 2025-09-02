// ReSharper disable InconsistentNaming

namespace MetroidvaniaItems.Patches
{
    using Behaviours;
    using HarmonyLib;
    using JetBrains.Annotations;
    using JumpKing.Level;

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

            if ((ModEntry.DataItems.Active == ModItems.FrozenWater ||
                ModEntry.DataItems.Active == ModItems.SolidWater) && BehaviourSolidWater.IsSolid)
            {
                __result = true;
            }
        }
    }
}
