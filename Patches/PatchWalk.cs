// ReSharper disable InconsistentNaming

namespace MetroidvaniaItems.Patches
{
    using BehaviorTree;
    using HarmonyLib;
    using JetBrains.Annotations;
    using JumpKing.Player;

    [HarmonyPatch(typeof(Walk), "MyRun")]
    public static class PatchWalk
    {
        [UsedImplicitly]
        public static bool Prefix(ref BTresult __result)
        {
            if (!ModEntry.IsInMenu)
            {
                return true;
            }

            __result = BTresult.Failure;
            return false;
        }
    }
}
