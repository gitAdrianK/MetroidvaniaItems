// ReSharper disable InconsistentNaming

namespace MetroidvaniaItems.Patches
{
    using Behaviours;
    using HarmonyLib;
    using JetBrains.Annotations;
    using JumpKing.BlockBehaviours;

    [HarmonyPatch(typeof(IceBlockBehaviour), "get_IsPlayerOnBlock")]
    public static class PatchIceBlockBehaviour
    {
        [UsedImplicitly]
        public static void Postfix(ref bool __result)
        {
            if (ModEntry.DataItems is null)
            {
                return;
            }

            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (ModEntry.DataItems.Active == ModItems.NeverIce)
            {
                __result = false;
            }
            else if (ModEntry.DataItems.Active == ModItems.FrozenWater && BehaviourFrozenWater.IsOnFrozenWater)
            {
                __result = true;
            }
        }
    }
}
