// ReSharper disable InconsistentNaming

namespace MetroidvaniaItems.Patches
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
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

            var insert = new List<CodeInstruction>
            {
                new CodeInstruction(
                    OpCodes.Call,
                    AccessTools.PropertyGetter(typeof(ModEntry), nameof(ModEntry.IsInMenu))),
                new CodeInstruction(OpCodes.Brfalse_S, continueLabel),
                new CodeInstruction(OpCodes.Ldc_R4, 0.0f),
                new CodeInstruction(OpCodes.Stloc_1)
            };
            code.InsertRange(insertionIndex, insert);

            return code.AsEnumerable();
        }
    }
}

/* See in text comparer
ldarg.0
call      instance class JumpKing.Player.BodyComp JumpKing.Player.PlayerNode::get_body()
ldflda    valuetype [MonoGame.Framework]Microsoft.Xna.Framework.Vector2 JumpKing.Player.BodyComp::Velocity
ldfld     float32 [MonoGame.Framework]Microsoft.Xna.Framework.Vector2::Y
ldc.r4    0.0
bge.un.s  IL_0030
ldarg.0
call      instance class JumpKing.Player.BodyComp JumpKing.Player.PlayerNode::get_body()
ldtoken   JumpKing.Level.SandBlock
call      class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
callvirt  instance bool JumpKing.Player.BodyComp::IsOnBlock(class [mscorlib]System.Type)
brfalse.s IL_0030
ldc.i4.2
ret
ldarg.0
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
stloc.1
ldarg.0
call      instance class JumpKing.Player.BodyComp JumpKing.Player.PlayerNode::get_body()
ldtoken   JumpKing.Level.SnowBlock
call      class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
callvirt  instance bool JumpKing.Player.BodyComp::IsOnBlock(class [mscorlib]System.Type)
brfalse.s IL_008F
ldarg.0
call      instance class JumpKing.Player.BodyComp JumpKing.Player.PlayerNode::get_body()
ldflda    valuetype [MonoGame.Framework]Microsoft.Xna.Framework.Vector2 JumpKing.Player.BodyComp::Velocity
ldc.r4    0.0
stfld     float32 [MonoGame.Framework]Microsoft.Xna.Framework.Vector2::X
br        IL_0130
ldarg.0
call      instance class JumpKing.Player.BodyComp JumpKing.Player.PlayerNode::get_body()
ldtoken   JumpKing.Level.IceBlock
call      class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
callvirt  instance bool JumpKing.Player.BodyComp::IsOnBlock(class [mscorlib]System.Type)
brfalse.s IL_011F
ldloc.1
call      int32 [mscorlib]System.Math::Sign(float32)
ldarg.0
call      instance class JumpKing.Player.BodyComp JumpKing.Player.PlayerNode::get_body()
ldflda    valuetype [MonoGame.Framework]Microsoft.Xna.Framework.Vector2 JumpKing.Player.BodyComp::Velocity
ldfld     float32 [MonoGame.Framework]Microsoft.Xna.Framework.Vector2::X
call      int32 [mscorlib]System.Math::Sign(float32)
ceq
stloc.2
ldloc.1
ldc.r4    0.0
beq.s     IL_0130
ldloc.2
brfalse.s IL_00EC
ldloc.1
call      float32 [mscorlib]System.Math::Abs(float32)
ldarg.0
call      instance class JumpKing.Player.BodyComp JumpKing.Player.PlayerNode::get_body()
ldflda    valuetype [MonoGame.Framework]Microsoft.Xna.Framework.Vector2 JumpKing.Player.BodyComp::Velocity
ldfld     float32 [MonoGame.Framework]Microsoft.Xna.Framework.Vector2::X
call      float32 [mscorlib]System.Math::Abs(float32)
ble.un.s  IL_0130
ldarg.0
call      instance class JumpKing.Player.BodyComp JumpKing.Player.PlayerNode::get_body()
ldflda    valuetype [MonoGame.Framework]Microsoft.Xna.Framework.Vector2 JumpKing.Player.BodyComp::Velocity
ldarg.0
call      instance class JumpKing.Player.BodyComp JumpKing.Player.PlayerNode::get_body()
ldflda    valuetype [MonoGame.Framework]Microsoft.Xna.Framework.Vector2 JumpKing.Player.BodyComp::Velocity
ldfld     float32 [MonoGame.Framework]Microsoft.Xna.Framework.Vector2::X
ldloc.1
call      float32 JumpKing.PlayerValues::get_ICE_FRICTION()
ldc.r4    2
mul
call      float32 ErikMaths.ErikMath::MoveTowards(float32, float32, float32)
stfld     float32 [MonoGame.Framework]Microsoft.Xna.Framework.Vector2::X
br.s      IL_0130
ldarg.0
call      instance class JumpKing.Player.BodyComp JumpKing.Player.PlayerNode::get_body()
ldflda    valuetype [MonoGame.Framework]Microsoft.Xna.Framework.Vector2 JumpKing.Player.BodyComp::Velocity
ldloc.1
stfld     float32 [MonoGame.Framework]Microsoft.Xna.Framework.Vector2::X
ldarg.0
call      instance class JumpKing.Player.PlayerEntity JumpKing.Player.PlayerNode::get_player()
ldloca.s  V_0
call      instance valuetype [MonoGame.Framework]Microsoft.Xna.Framework.Point JumpKing.Player.InputComponent/State::get_dpad()
ldfld     int32 [MonoGame.Framework]Microsoft.Xna.Framework.Point::X
callvirt  instance void JumpKing.Player.PlayerEntity::SetDirection(int32)
ldloca.s  V_0
call      instance valuetype [MonoGame.Framework]Microsoft.Xna.Framework.Point JumpKing.Player.InputComponent/State::get_dpad()
ldfld     int32 [MonoGame.Framework]Microsoft.Xna.Framework.Point::X
brtrue.s  IL_0157
ldc.i4.2
ret
ldc.i4.1
ret

ldarg.0
call      instance class JumpKing.Player.BodyComp JumpKing.Player.PlayerNode::get_body()
ldflda    valuetype [MonoGame.Framework]Microsoft.Xna.Framework.Vector2 JumpKing.Player.BodyComp::Velocity
ldfld     float32 [MonoGame.Framework]Microsoft.Xna.Framework.Vector2::Y
ldc.r4    0.0
bge.un.s  IL_0030
ldarg.0
call      instance class JumpKing.Player.BodyComp JumpKing.Player.PlayerNode::get_body()
ldtoken   JumpKing.Level.SandBlock
call      class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
callvirt  instance bool JumpKing.Player.BodyComp::IsOnBlock(class [mscorlib]System.Type)
brfalse.s IL_0030
ldc.i4.2
ret
ldarg.0
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
call      bool [MetroidvaniaItems]MetroidvaniaItems.ModEntry::get_IsInMenu()
brfalse.s IL_005D
ldc.r4    0.0
stloc.1
ldc.i4.6
call      bool JumpKing.Player.Skins.SkinManager::IsWearingSkin(valuetype JumpKing.MiscEntities.WorldItems.Items)
brfalse.s IL_006B
ldc.r4    0.0
stloc.1
ldarg.0
call      instance class JumpKing.Player.BodyComp JumpKing.Player.PlayerNode::get_body()
ldtoken   JumpKing.Level.SnowBlock
call      class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
callvirt  instance bool JumpKing.Player.BodyComp::IsOnBlock(class [mscorlib]System.Type)
brfalse.s IL_009C
ldarg.0
call      instance class JumpKing.Player.BodyComp JumpKing.Player.PlayerNode::get_body()
ldflda    valuetype [MonoGame.Framework]Microsoft.Xna.Framework.Vector2 JumpKing.Player.BodyComp::Velocity
ldc.r4    0.0
stfld     float32 [MonoGame.Framework]Microsoft.Xna.Framework.Vector2::X
br        IL_013D
ldarg.0
call      instance class JumpKing.Player.BodyComp JumpKing.Player.PlayerNode::get_body()
ldtoken   JumpKing.Level.IceBlock
call      class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
callvirt  instance bool JumpKing.Player.BodyComp::IsOnBlock(class [mscorlib]System.Type)
brfalse.s IL_012C
ldloc.1
call      int32 [mscorlib]System.Math::Sign(float32)
ldarg.0
call      instance class JumpKing.Player.BodyComp JumpKing.Player.PlayerNode::get_body()
ldflda    valuetype [MonoGame.Framework]Microsoft.Xna.Framework.Vector2 JumpKing.Player.BodyComp::Velocity
ldfld     float32 [MonoGame.Framework]Microsoft.Xna.Framework.Vector2::X
call      int32 [mscorlib]System.Math::Sign(float32)
ceq
stloc.2
ldloc.1
ldc.r4    0.0
beq.s     IL_013D
ldloc.2
brfalse.s IL_00F9
ldloc.1
call      float32 [mscorlib]System.Math::Abs(float32)
ldarg.0
call      instance class JumpKing.Player.BodyComp JumpKing.Player.PlayerNode::get_body()
ldflda    valuetype [MonoGame.Framework]Microsoft.Xna.Framework.Vector2 JumpKing.Player.BodyComp::Velocity
ldfld     float32 [MonoGame.Framework]Microsoft.Xna.Framework.Vector2::X
call      float32 [mscorlib]System.Math::Abs(float32)
ble.un.s  IL_013D
ldarg.0
call      instance class JumpKing.Player.BodyComp JumpKing.Player.PlayerNode::get_body()
ldflda    valuetype [MonoGame.Framework]Microsoft.Xna.Framework.Vector2 JumpKing.Player.BodyComp::Velocity
ldarg.0
call      instance class JumpKing.Player.BodyComp JumpKing.Player.PlayerNode::get_body()
ldflda    valuetype [MonoGame.Framework]Microsoft.Xna.Framework.Vector2 JumpKing.Player.BodyComp::Velocity
ldfld     float32 [MonoGame.Framework]Microsoft.Xna.Framework.Vector2::X
ldloc.1
call      float32 JumpKing.PlayerValues::get_ICE_FRICTION()
ldc.r4    2
mul
call      float32 ErikMaths.ErikMath::MoveTowards(float32, float32, float32)
stfld     float32 [MonoGame.Framework]Microsoft.Xna.Framework.Vector2::X
br.s      IL_013D
ldarg.0
call      instance class JumpKing.Player.BodyComp JumpKing.Player.PlayerNode::get_body()
ldflda    valuetype [MonoGame.Framework]Microsoft.Xna.Framework.Vector2 JumpKing.Player.BodyComp::Velocity
ldloc.1
stfld     float32 [MonoGame.Framework]Microsoft.Xna.Framework.Vector2::X
ldarg.0
call      instance class JumpKing.Player.PlayerEntity JumpKing.Player.PlayerNode::get_player()
ldloca.s  state
call      instance valuetype [MonoGame.Framework]Microsoft.Xna.Framework.Point JumpKing.Player.InputComponent/State::get_dpad()
ldfld     int32 [MonoGame.Framework]Microsoft.Xna.Framework.Point::X
callvirt  instance void JumpKing.Player.PlayerEntity::SetDirection(int32)
ldloca.s  state
call      instance valuetype [MonoGame.Framework]Microsoft.Xna.Framework.Point JumpKing.Player.InputComponent/State::get_dpad()
ldfld     int32 [MonoGame.Framework]Microsoft.Xna.Framework.Point::X
brtrue.s  IL_0164
ldc.i4.2
ret
ldc.i4.1
ret
 */
