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
            if (BehaviourSolidWater.IsSolid)
            {
                __result = true;
            }
        }
    }
}
