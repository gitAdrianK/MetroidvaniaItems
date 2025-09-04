// ReSharper disable InconsistentNaming

namespace MetroidvaniaItems.Patches
{
    using BehaviorTree;
    using HarmonyLib;
    using JetBrains.Annotations;
    using JumpKing.Player;

    [HarmonyPatch(typeof(JumpState), "MyRun")]
    public static class PatchJumpStateMyRun
    {
        public static int JumpFrames { get; private set; }
        private static float PreviousTimer { get; set; }

        [UsedImplicitly]
        public static void Postfix(JumpState __instance, BTresult __result, TickData p_data)
        {
            if (__result == BTresult.Failure)
            {
                return;
            }

            var currentTimer = Traverse.Create(__instance).Field("m_timer").GetValue<float>();
            if (__result == BTresult.Success)
            {
                currentTimer = PreviousTimer + (p_data.delta_time * __instance.body.GetMultipliers());
            }

            if (__instance.last_result != BTresult.Running)
            {
                JumpFrames = -1;
            }

            JumpFrames++;
            PreviousTimer = currentTimer;
        }
    }
}
