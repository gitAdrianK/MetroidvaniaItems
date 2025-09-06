namespace MetroidvaniaItems.Patches
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using Data;
    using HarmonyLib;
    using JetBrains.Annotations;

    // We patch this because the "AlwaysWater" item, in combination with the "UpsideDown" mod would spam water particles
    [HarmonyPatch("JumpKing.BodyCompBehaviours.WaterParticleSpawningBehaviour", "ExecuteBehaviour")]
    public static class PatchWaterParticleSpawningBehaviour
    {
        [UsedImplicitly]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            var code = new List<CodeInstruction>(instructions);
            var type = AccessTools.TypeByName("JumpKing.BodyCompBehaviours.WaterParticleSpawningBehaviour");
            var field = AccessTools.Field(type, "m_splashParticleSpawner");

            var insertionIndex = -1;
            var continueLabel = il.DefineLabel();

            // Find the first part, that is where we want to insert out own IL instructions.
            for (var i = 0; i < code.Count - 1; i++)
            {
                if (code[i].opcode != OpCodes.Ldarg_0
                    || code[i + 1].opcode != OpCodes.Ldfld
                    || code[i + 1].operand as FieldInfo != field)
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

            var getterData = AccessTools.PropertyGetter(typeof(ModEntry), nameof(ModEntry.DataMetroidvania));
            var getterActive = AccessTools.PropertyGetter(typeof(DataMetroidvania), nameof(DataMetroidvania.Active));
            var continueLabel2 = il.DefineLabel();

            var insert = new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Call, getterData),
                new CodeInstruction(OpCodes.Brtrue_S, continueLabel2),
                new CodeInstruction(OpCodes.Ldc_I4_1),
                new CodeInstruction(OpCodes.Ret),
                new CodeInstruction(OpCodes.Call, getterData),
                new CodeInstruction(OpCodes.Callvirt, getterActive),
                new CodeInstruction(OpCodes.Ldc_I4_8),
                new CodeInstruction(OpCodes.Bne_Un_S, continueLabel),
                new CodeInstruction(OpCodes.Ldc_I4_1),
                new CodeInstruction(OpCodes.Ret)
            };

            insert[4].labels.Add(continueLabel2);

            code.InsertRange(insertionIndex, insert);

            return code.AsEnumerable();
        }
    }
}
/*
IL_0000: ldarg.1
IL_0001: ldfld     class JumpKing.Player.BodyComp JumpKing.BodyCompBehaviours.BehaviourContext::BodyComp
IL_0006: stloc.0
IL_0007: ldloc.0
IL_0008: ldtoken   JumpKing.Level.WaterBlock
IL_000D: call      class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
IL_0012: callvirt  instance class JumpKing.API.IBlockBehaviour JumpKing.Player.BodyComp::GetBlockBehaviour(class [mscorlib]System.Type)
IL_0017: isinst    JumpKing.BlockBehaviours.WaterBlockBehaviour
IL_001C: stloc.1
IL_001D: ldloc.1
IL_001E: brfalse   IL_00AE
IL_0023: ldloc.1
IL_0024: callvirt  instance bool JumpKing.BlockBehaviours.WaterBlockBehaviour::get_PrevIsPlayerOnBlock()
IL_0029: ldloc.1
IL_002A: callvirt  instance bool JumpKing.BlockBehaviours.WaterBlockBehaviour::get_IsPlayerOnBlock()
IL_002F: beq.s     IL_00AE
IL_0031: ldloc.0
IL_0032: callvirt  instance valuetype [MonoGame.Framework]Microsoft.Xna.Framework.Rectangle JumpKing.Player.BodyComp::GetHitbox()
IL_0037: stloc.2
IL_0038: ldloca.s  V_2
IL_003A: call      instance valuetype [MonoGame.Framework]Microsoft.Xna.Framework.Point [MonoGame.Framework]Microsoft.Xna.Framework.Rectangle::get_Center()
IL_003F: stloc.3
IL_0040: ldloc.1
IL_0041: callvirt  instance bool JumpKing.BlockBehaviours.WaterBlockBehaviour::get_IsPlayerOnBlock()
IL_0046: brtrue.s  IL_005A
IL_0048: ldloc.0
IL_0049: ldflda    valuetype [MonoGame.Framework]Microsoft.Xna.Framework.Vector2 JumpKing.Player.BodyComp::Velocity
IL_004E: ldfld     float32 [MonoGame.Framework]Microsoft.Xna.Framework.Vector2::Y
IL_0053: ldc.r4    0.0
IL_0058: blt.s     IL_0074
IL_005A: ldloc.1
IL_005B: callvirt  instance bool JumpKing.BlockBehaviours.WaterBlockBehaviour::get_IsPlayerOnBlock()
IL_0060: brfalse.s IL_0089
IL_0062: ldloc.0
IL_0063: ldflda    valuetype [MonoGame.Framework]Microsoft.Xna.Framework.Vector2 JumpKing.Player.BodyComp::Velocity
IL_0068: ldfld     float32 [MonoGame.Framework]Microsoft.Xna.Framework.Vector2::Y
IL_006D: ldc.r4    0.0
IL_0072: blt.un.s  IL_0089
IL_0074: ldloca.s  V_3
IL_0076: ldflda    int32 [MonoGame.Framework]Microsoft.Xna.Framework.Point::Y
IL_007B: dup
IL_007C: ldind.i4
IL_007D: ldloc.2
IL_007E: ldfld     int32 [MonoGame.Framework]Microsoft.Xna.Framework.Rectangle::Height
IL_0083: ldc.i4.2
IL_0084: div
IL_0085: add
IL_0086: stind.i4
IL_0087: br.s      IL_009C
IL_0089: ldloca.s  V_3
IL_008B: ldflda    int32 [MonoGame.Framework]Microsoft.Xna.Framework.Point::Y
IL_0090: dup
IL_0091: ldind.i4
IL_0092: ldloc.2
IL_0093: ldfld     int32 [MonoGame.Framework]Microsoft.Xna.Framework.Rectangle::Height
IL_0098: ldc.i4.2
IL_0099: div
IL_009A: sub
IL_009B: stind.i4
IL_009C: ldarg.0
IL_009D: ldfld     class JumpKing.API.ISplashParticleSpawner JumpKing.BodyCompBehaviours.WaterParticleSpawningBehaviour::m_splashParticleSpawner
IL_00A2: ldloc.3
IL_00A3: ldloc.1
IL_00A4: callvirt  instance bool JumpKing.BlockBehaviours.WaterBlockBehaviour::get_IsPlayerOnBlock()
IL_00A9: callvirt  instance void JumpKing.API.ISplashParticleSpawner::CreateWaterSplashParticle(valuetype [MonoGame.Framework]Microsoft.Xna.Framework.Point, bool)
IL_00AE: ldc.i4.1
IL_00AF: ret

IL_0000: ldarg.1
IL_0001: ldfld     class JumpKing.Player.BodyComp JumpKing.BodyCompBehaviours.BehaviourContext::BodyComp
IL_0006: stloc.0
IL_0007: ldloc.0
IL_0008: ldtoken   JumpKing.Level.WaterBlock
IL_000D: call      class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
IL_0012: callvirt  instance class JumpKing.API.IBlockBehaviour JumpKing.Player.BodyComp::GetBlockBehaviour(class [mscorlib]System.Type)
IL_0017: isinst    JumpKing.BlockBehaviours.WaterBlockBehaviour
IL_001C: stloc.1
IL_001D: ldloc.1
IL_001E: brfalse   IL_00C9
IL_0023: ldloc.1
IL_0024: callvirt  instance bool JumpKing.BlockBehaviours.WaterBlockBehaviour::get_PrevIsPlayerOnBlock()
IL_0029: ldloc.1
IL_002A: callvirt  instance bool JumpKing.BlockBehaviours.WaterBlockBehaviour::get_IsPlayerOnBlock()
IL_002F: beq       IL_00C9
IL_0034: ldloc.0
IL_0035: callvirt  instance valuetype [MonoGame.Framework]Microsoft.Xna.Framework.Rectangle JumpKing.Player.BodyComp::GetHitbox()
IL_003A: stloc.2
IL_003B: ldloca.s  hitbox
IL_003D: call      instance valuetype [MonoGame.Framework]Microsoft.Xna.Framework.Point [MonoGame.Framework]Microsoft.Xna.Framework.Rectangle::get_Center()
IL_0042: stloc.3
IL_0043: ldloc.1
IL_0044: callvirt  instance bool JumpKing.BlockBehaviours.WaterBlockBehaviour::get_IsPlayerOnBlock()
IL_0049: brtrue.s  IL_005D
IL_004B: ldloc.0
IL_004C: ldflda    valuetype [MonoGame.Framework]Microsoft.Xna.Framework.Vector2 JumpKing.Player.BodyComp::Velocity
IL_0051: ldfld     float32 [MonoGame.Framework]Microsoft.Xna.Framework.Vector2::Y
IL_0056: ldc.r4    0.0
IL_005B: blt.s     IL_0077
IL_005D: ldloc.1
IL_005E: callvirt  instance bool JumpKing.BlockBehaviours.WaterBlockBehaviour::get_IsPlayerOnBlock()
IL_0063: brfalse.s IL_008C
IL_0065: ldloc.0
IL_0066: ldflda    valuetype [MonoGame.Framework]Microsoft.Xna.Framework.Vector2 JumpKing.Player.BodyComp::Velocity
IL_006B: ldfld     float32 [MonoGame.Framework]Microsoft.Xna.Framework.Vector2::Y
IL_0070: ldc.r4    0.0
IL_0075: blt.un.s  IL_008C
IL_0077: ldloca.s  center
IL_0079: ldflda    int32 [MonoGame.Framework]Microsoft.Xna.Framework.Point::Y
IL_007E: dup
IL_007F: ldind.i4
IL_0080: ldloc.2
IL_0081: ldfld     int32 [MonoGame.Framework]Microsoft.Xna.Framework.Rectangle::Height
IL_0086: ldc.i4.2
IL_0087: div
IL_0088: add
IL_0089: stind.i4
IL_008A: br.s      IL_009F
IL_008C: ldloca.s  center
IL_008E: ldflda    int32 [MonoGame.Framework]Microsoft.Xna.Framework.Point::Y
IL_0093: dup
IL_0094: ldind.i4
IL_0095: ldloc.2
IL_0096: ldfld     int32 [MonoGame.Framework]Microsoft.Xna.Framework.Rectangle::Height
IL_009B: ldc.i4.2
IL_009C: div
IL_009D: sub
IL_009E: stind.i4
IL_009F: call      class [MetroidvaniaItems]MetroidvaniaItems.Data.DataMetroidvania [MetroidvaniaItems]MetroidvaniaItems.ModEntry::get_DataItems()
IL_00A4: brtrue.s  IL_00A8
IL_00A6: ldc.i4.1
IL_00A7: ret
IL_00A8: call      class [MetroidvaniaItems]MetroidvaniaItems.Data.DataMetroidvania [MetroidvaniaItems]MetroidvaniaItems.ModEntry::get_DataItems()
IL_00AD: callvirt  instance valuetype [MetroidvaniaItems]MetroidvaniaItems.ModItems [MetroidvaniaItems]MetroidvaniaItems.Data.DataMetroidvania::get_Active()
IL_00B2: ldc.i4.8
IL_00B3: bne.un.s  IL_00B7
IL_00B5: ldc.i4.1
IL_00B6: ret
IL_00B7: ldarg.0
IL_00B8: ldfld     class JumpKing.API.ISplashParticleSpawner JumpKing.BodyCompBehaviours.WaterParticleSpawningBehaviour::m_splashParticleSpawner
IL_00BD: ldloc.3
IL_00BE: ldloc.1
IL_00BF: callvirt  instance bool JumpKing.BlockBehaviours.WaterBlockBehaviour::get_IsPlayerOnBlock()
IL_00C4: callvirt  instance void JumpKing.API.ISplashParticleSpawner::CreateWaterSplashParticle(valuetype [MonoGame.Framework]Microsoft.Xna.Framework.Point, bool)
IL_00C9: ldc.i4.1
IL_00CA: ret
 */
