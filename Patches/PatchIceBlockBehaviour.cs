// ReSharper disable InconsistentNaming

namespace MetroidvaniaItems.Patches
{
    using Behaviours;
    using HarmonyLib;
    using JetBrains.Annotations;
    using JumpKing.BlockBehaviours;

    [HarmonyPatch(typeof(IceBlockBehaviour), nameof(IceBlockBehaviour.IsPlayerOnBlock), MethodType.Getter)]
    public static class PatchIceBlockBehaviour
    {
        [UsedImplicitly]
        public static void Postfix(ref bool __result)
        {
            if (ModEntry.DataMetroidvania is null)
            {
                return;
            }

            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (ModEntry.DataMetroidvania.Active == ModItems.NeverIce)
            {
                __result = false;
            }
            else if (ModEntry.DataMetroidvania.Active == ModItems.FrozenWater && BehaviourFrozenWater.IsOnFrozenWater)
            {
                __result = true;
            }
        }
    }
}
