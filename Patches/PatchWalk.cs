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
            // TODO: If the keybind doesnt shadow to vanilla left right input i guess its cool to allow
            // reusing left/right -> only one new bind (left/right maybe always same as vanilla left/right?)
            // new left/right -> three new binds

            if (!ModEntry.IsInMenu)
            {
                return true;
            }

            __result = BTresult.Failure;
            return false;
        }
    }
}
