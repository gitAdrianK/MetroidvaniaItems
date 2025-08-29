// ReSharper disable InconsistentNaming

namespace MetroidvaniaItems.Patches
{
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

            if (ModEntry.DataItems.Active == ModItems.NeverIce)
            {
                __result = false;
            }
        }
    }
}
