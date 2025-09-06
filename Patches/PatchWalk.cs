// ReSharper disable InconsistentNaming

namespace MetroidvaniaItems.Patches
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using Data;
    using HarmonyLib;
    using JetBrains.Annotations;
    using JumpKing.Player;

    [HarmonyPatch(typeof(Walk), "MyRun")]
    public static class PatchWalk
    {
        [UsedImplicitly]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            var code = new List<CodeInstruction>(instructions);
            var method = AccessTools.Method("JumpKing.Player.Skins.SkinManager:IsWearingSkin");

            var insertionIndex = -1;
            var continueLabel = il.DefineLabel();

            // Find the first part, that is where we want to insert out own IL instructions.
            for (var i = 0; i < code.Count - 1; i++)
            {
                if (code[i].opcode != OpCodes.Ldc_I4_6
                    || code[i + 1].opcode != OpCodes.Call
                    || code[i + 1].operand as MethodInfo != method)
                {
                    continue;
                }

                insertionIndex = i;
                code[i].labels.Add(continueLabel);
                break;
            }

            if (insertionIndex == -1)
            {
                return code.AsEnumerable();
            }

            var data = AccessTools.PropertyGetter(typeof(ModEntry), nameof(ModEntry.DataMetroidvania));
            var menuState = AccessTools.PropertyGetter(typeof(DataMetroidvania), nameof(DataMetroidvania.MenuState));
            var insert = new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Call, data),
                new CodeInstruction(OpCodes.Brfalse_S, continueLabel),
                new CodeInstruction(OpCodes.Call, data),
                new CodeInstruction(OpCodes.Callvirt, menuState),
                new CodeInstruction(OpCodes.Ldc_I4_1),
                new CodeInstruction(OpCodes.Bne_Un_S, continueLabel),
                new CodeInstruction(OpCodes.Ldc_R4, 0.0f),
                new CodeInstruction(OpCodes.Stloc_1)
            };
            code.InsertRange(insertionIndex, insert);

            return code.AsEnumerable();
        }
    }
}

/* See in text comparer
call      instance class JumpKing.Player.InputComponent JumpKing.Player.PlayerNode::get_input()
callvirt  instance valuetype JumpKing.Player.InputComponent/State JumpKing.Player.InputComponent::GetState()
stloc.0
ldloca.s  V_0
call      instance valuetype [MonoGame.Framework]Microsoft.Xna.Framework.Point JumpKing.Player.InputComponent/State::get_dpad()
ldfld     int32 [MonoGame.Framework]Microsoft.Xna.Framework.Point::X
conv.r4
call      float32 JumpKing.PlayerValues::get_WALK_SPEED()
mul
stloc.1
ldc.i4.6
call      bool JumpKing.Player.Skins.SkinManager::IsWearingSkin(valuetype JumpKing.MiscEntities.WorldItems.Items)
brfalse.s IL_005E
ldc.r4    0.0

call      instance class JumpKing.Player.InputComponent JumpKing.Player.PlayerNode::get_input()
callvirt  instance valuetype JumpKing.Player.InputComponent/State JumpKing.Player.InputComponent::GetState()
stloc.0
ldloca.s  state
call      instance valuetype [MonoGame.Framework]Microsoft.Xna.Framework.Point JumpKing.Player.InputComponent/State::get_dpad()
ldfld     int32 [MonoGame.Framework]Microsoft.Xna.Framework.Point::X
conv.r4
call      float32 JumpKing.PlayerValues::get_WALK_SPEED()
mul
stloc.1
call      class [MetroidvaniaItems]MetroidvaniaItems.Data.DataMetroidvania [MetroidvaniaItems]MetroidvaniaItems.ModEntry::get_DataMetroidvania()
brfalse.s IL_006A
call      class [MetroidvaniaItems]MetroidvaniaItems.Data.DataMetroidvania [MetroidvaniaItems]MetroidvaniaItems.ModEntry::get_DataMetroidvania()
callvirt  instance valuetype [MetroidvaniaItems]MetroidvaniaItems.Util.MenuState [MetroidvaniaItems]MetroidvaniaItems.Data.DataMetroidvania::get_MenuState()
ldc.i4.1
bne.un.s  IL_006A
ldc.r4    0.0
stloc.1
ldc.i4.6
call      bool JumpKing.Player.Skins.SkinManager::IsWearingSkin(valuetype JumpKing.MiscEntities.WorldItems.Items)
brfalse.s IL_0078
ldc.r4    0.0
 */
