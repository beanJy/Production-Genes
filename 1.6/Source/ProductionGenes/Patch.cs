using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;
using System.Linq;
using System;
using System.Reflection.Emit;
using static HarmonyLib.Code;
using static UnityEngine.Scripting.GarbageCollector;
using System.Reflection;
using System.Runtime.InteropServices;
using static RimWorld.TransferableUIUtility;
using System.Text;
using static RimWorld.ColonistBar;
using static RimWorld.ChildcareUtility;
using Verse.AI;

namespace DDJY.Patch
{
    //HarmonyLib初始化
    [StaticConstructorOnStartup]
    public class ProductionGenes_Patch
    {

        static ProductionGenes_Patch()
        {
            instance.PatchCategory("ProductionGenes_Patch");
        }
        public static Harmony instance = new Harmony("ProductionGenes_Patch");
    }


    //修改 HediffComp_Chargeable.CompPostTickInterval
    [HarmonyPatchCategory("ProductionGenes_Patch")]
    [HarmonyPatch(typeof(HediffComp_Chargeable), "CompPostTickInterval")]
    internal static class HediffComp_Chargeable_CompPostTickInterval
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var codes = new List<CodeInstruction>(instructions);
            var get_Props = AccessTools.PropertyGetter(typeof(HediffComp_Chargeable), "Props");
            var field_ticksToFullCharge = AccessTools.Field(typeof(HediffCompProperties_Chargeable), "ticksToFullCharge");
            var field_fullChargeAmount = AccessTools.Field(typeof(HediffCompProperties_Chargeable), "fullChargeAmount");
            var GetTicksToFullCharge = AccessTools.Method(typeof(HediffComp_MilkableHuman), "GetTicksToFullCharge");
            var GetFullChargeAmount = AccessTools.Method(typeof(HediffComp_MilkableHuman), "GetFullChargeAmount");
            for (var i = 0; i < codes.Count - 1; i++)
            {
                if
                (
                    codes[i].Calls(get_Props) &&
                    codes[i + 1].LoadsField(field_ticksToFullCharge)
                )
                {
                    codes[i] = new CodeInstruction(OpCodes.Call, GetTicksToFullCharge);
                    codes[i + 1] = new CodeInstruction(OpCodes.Nop);
                }
                if
                (
                   codes[i].Calls(get_Props) &&
                   codes[i + 1].LoadsField(field_fullChargeAmount)
                )
                {
                    codes[i] = new CodeInstruction(OpCodes.Call, GetFullChargeAmount);
                    codes[i + 1] = new CodeInstruction(OpCodes.Nop);
                }
            }
            return codes;
        }
    }

    //修改 HediffComp_Lactating.AddedNutritionPerDay
    [HarmonyPatchCategory("ProductionGenes_Patch")]
    [HarmonyPatch(typeof(HediffComp_Lactating), "AddedNutritionPerDay")]
    internal static class HediffComp_Lactating_AddedNutritionPerDay
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var codes = new List<CodeInstruction>(instructions);
            var get_Props = AccessTools.PropertyGetter(typeof(HediffComp_Chargeable), "Props");
            var field_ticksToFullCharge = AccessTools.Field(typeof(HediffCompProperties_Chargeable), "ticksToFullCharge");
            var field_fullChargeAmount = AccessTools.Field(typeof(HediffCompProperties_Chargeable), "fullChargeAmount");
            var GetTicksToFullCharge = AccessTools.Method(typeof(HediffComp_MilkableHuman), "GetTicksToFullCharge");
            var GetFullChargeAmount = AccessTools.Method(typeof(HediffComp_MilkableHuman), "GetFullChargeAmount");
            for (var i = 0; i < codes.Count - 1; i++)
            {
                if
                (
                    codes[i].Calls(get_Props) &&
                    codes[i + 1].LoadsField(field_ticksToFullCharge)
                )
                {
                    codes[i] = new CodeInstruction(OpCodes.Call, GetTicksToFullCharge);
                    codes[i + 1] = new CodeInstruction(OpCodes.Nop);
                }
                if
                (
                   codes[i].Calls(get_Props) &&
                   codes[i + 1].LoadsField(field_fullChargeAmount)
                )
                {
                    codes[i] = new CodeInstruction(OpCodes.Call, GetFullChargeAmount);
                    codes[i + 1] = new CodeInstruction(OpCodes.Nop);
                }
            }
            return codes;
        }
    }

    //修改 HediffComp_Chargeable.set_Charge
    [HarmonyPatchCategory("ProductionGenes_Patch")]
    [HarmonyPatch(typeof(HediffComp_Chargeable), "set_Charge")]
    internal static class HediffComp_Chargeable_set_Charge
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var codes = new List<CodeInstruction>(instructions);
            var get_Props = AccessTools.PropertyGetter(typeof(HediffComp_Chargeable), "Props");
            var field_fullChargeAmount = AccessTools.Field(typeof(HediffCompProperties_Chargeable), "fullChargeAmount");
            var GetFullChargeAmount = AccessTools.Method(typeof(HediffComp_MilkableHuman), "GetFullChargeAmount");
            for (var i = 0; i < codes.Count - 1; i++)
            {
                if
                (
                   codes[i].Calls(get_Props) &&
                   codes[i + 1].LoadsField(field_fullChargeAmount)
                )
                {
                    codes[i] = new CodeInstruction(OpCodes.Call, GetFullChargeAmount);
                    codes[i + 1] = new CodeInstruction(OpCodes.Nop);
                }
            }
            return codes;
        }
    }

    //修改 HediffComp_Lactating.get_CompTipStringExtra
    [HarmonyPatchCategory("ProductionGenes_Patch")]
    [HarmonyPatch(typeof(HediffComp_Lactating), "get_CompTipStringExtra")]
    internal static class HediffComp_Lactating_get_CompTipStringExtra
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var codes = new List<CodeInstruction>(instructions);
            var get_Props = AccessTools.PropertyGetter(typeof(HediffComp_Chargeable), "Props");
            var field_fullChargeAmount = AccessTools.Field(typeof(HediffCompProperties_Chargeable), "fullChargeAmount");
            var GetFullChargeAmount = AccessTools.Method(typeof(HediffComp_MilkableHuman), "GetFullChargeAmount");
            for (var i = 0; i < codes.Count - 1; i++)
            {
                if
                (
                   codes[i].Calls(get_Props) &&
                   codes[i + 1].LoadsField(field_fullChargeAmount)
                )
                {
                    codes[i] = new CodeInstruction(OpCodes.Call, GetFullChargeAmount);
                    codes[i + 1] = new CodeInstruction(OpCodes.Nop);
                }
            }
            return codes;
        }
    }

    //修改 HediffComp_Lactating.CompLabelInBracketsExtra
    [HarmonyPatchCategory("ProductionGenes_Patch")]
    [HarmonyPatch(typeof(HediffComp_Chargeable), "get_CompLabelInBracketsExtra")]
    internal static class HediffComp_Chargeable_get_CompLabelInBracketsExtra
    {
        public static bool Prefix(HediffComp_Chargeable __instance, ref string __result)
        {
            var pawn = __instance.parent?.pawn;
            if (pawn != null)
            {
                var hediff = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Lactating);
                var comp = hediff?.TryGetComp<HediffComp_MilkableHuman>();
                if (comp != null)
                {
                    __result = __instance.Props.labelInBrackets.Formatted(
                        comp.Fullness.Named("CHARGEFACTOR"),
                        comp.produce.label.Named("PRODUCENAME")
                    );
                    return false;
                }
            }
            __result = __instance.Props.labelInBrackets.Formatted(
                (__instance.Charge / (__instance.Props.fullChargeAmount * 4)).Named("CHARGEFACTOR"),
                DDJY_ThingDefOf.Milk.label.Named("PRODUCENAME")
            );
            return false;
        }
    }
    //取消营养不良移除哺乳状态
    [HarmonyPatchCategory("ProductionGenes_Patch")]
    [HarmonyPatch(typeof(HediffComp_RemoveIfOtherHediff), "ShouldRemove")]
    internal static class HediffComp_RemoveIfOtherHediff_ShouldRemove
    {
        static bool Prefix(HediffComp_RemoveIfOtherHediff __instance, int delta, ref bool __result)
        {
            HediffComp_MilkableHuman comp = __instance.parent.TryGetComp<HediffComp_MilkableHuman>();
            if (comp != null &&comp.nonOverriddenGene != null)
            {
                __result = false;
                return false;
            }
            return true; 
        }
    }
    //只有奶和虫胶可哺乳
    [HarmonyPatchCategory("ProductionGenes_Patch")]
    [HarmonyPatch(typeof(ChildcareUtility), "CanMomAutoBreastfeedBabyNow")]
    internal static class ChildcareUtility_CanMomAutoBreastfeedBabyNow
    {
        public static void Postfix(Pawn mother,ref bool __result, ref BreastfeedFailReason? reason)
        {
            if (!__result) return;
            if (mother == null || mother.health == null || mother.health.hediffSet == null)
            {
                return;
            }
            Hediff lactating = mother.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Lactating);
            if (lactating != null )
            {
                var comp = lactating.TryGetComp<HediffComp_MilkableHuman>();
                if (comp.nonOverriddenGene == null || comp.nonOverriddenGene.def == DDJY_ThingDefOf.DDJY_MilkProduction || comp.nonOverriddenGene.def == DDJY_ThingDefOf.DDJY_InsectJellyProduction)
                {
                    return;
                }
                reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.MomNotEnoughMilk);
                __result = false;
                
            }
            return;
        }
    }
    //跳过生育率检测
    [HarmonyPatchCategory("ProductionGenes_Patch")]
    [HarmonyPatch(typeof(StatPart_FertilityByHediffs), "Factor")]
    internal static class StatPart_FertilityByHediffs_Factor
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var codes = new List<CodeInstruction>(instructions);
            var label1 = generator.DefineLabel();
            MethodInfo getCurrentMethod = AccessTools.Method(typeof(List<Verse.Hediff>.Enumerator), "get_Current");
            MethodInfo moveNextMethod = AccessTools.Method(typeof(List<Verse.Hediff>.Enumerator), "MoveNext");
            var shouldSkipMethod = AccessTools.Method(typeof(HediffComp_MilkableHuman), "ShouldSkipFertilityCheck");

            // 绑定label
            for (var i = 0; i < codes.Count - 2; i++)
            {
                if (codes[i].opcode == OpCodes.Ldloca_S &&
                    codes[i + 1].Calls(moveNextMethod) &&
                    codes[i + 2].opcode == OpCodes.Brtrue_S)
                {
                    codes[i].labels.Add(label1);
                    break;
                }
            }

            // 插入判断代码
            for (var i = 0; i < codes.Count - 2; i++)
            {
                if (codes[i].opcode == OpCodes.Ldloca_S &&
                    codes[i + 1].Calls(getCurrentMethod) &&
                    codes[i + 2].opcode == OpCodes.Stloc_2)
                {
                    codes.InsertRange(i + 3, new[]
                    {
                        new CodeInstruction(OpCodes.Ldloc_2),
                        new CodeInstruction(OpCodes.Call, shouldSkipMethod),
                        new CodeInstruction(OpCodes.Brtrue, label1),
                    });
                    break;
                }
            }

            return codes;
        }
    }

    //可搬运囚犯区奶
    [HarmonyPatchCategory("ProductionGenes_Patch")]
    [HarmonyPatch(typeof(HaulAIUtility), "PawnCanAutomaticallyHaulFast")]
    public static class HaulAIUtility_PawnCanAutomaticallyHaulFast
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var codes = new List<CodeInstruction>(instructions);
            var IsSociallyProper = AccessTools.Method(typeof(SocialProperness), "IsSociallyProper",new Type[] { typeof(Thing), typeof(Pawn), typeof(bool), typeof(bool) });
            var IsInGeneMilkList = AccessTools.Method(typeof(HediffComp_GatherHuman), "IsInGeneMilkList");
            for (var i = 0; i < codes.Count - 2; i++)
            {
                if (
                    codes[i].Calls(IsSociallyProper)&&
                    codes[i + 1].opcode == OpCodes.Brtrue_S)
                {
                    var copyBrtrue_S = new CodeInstruction(codes[i + 1]);
                    codes.InsertRange(i + 2, new[]
                    {
                        new CodeInstruction(OpCodes.Ldarg_1),
                        new CodeInstruction(OpCodes.Call, IsInGeneMilkList),
                        copyBrtrue_S,
                    });
                    break;
                }

                

            }
            return codes;
        }
    }
    /*//添加面板显示
    [HarmonyPatchCategory("ProductionGenes_Patch")]
    [HarmonyPatch(typeof(ThingWithComps), "InspectStringPartsFromComps")]
    internal static class ThingWithComps_InspectStringPartsFromComps
    {
        public static void Postfix(ThingWithComps __instance, ref string __result)
        {
            if (__instance is Pawn pawn && pawn.health != null)
            {
                StringBuilder stringBuilder = new StringBuilder();

                if (!__result.NullOrEmpty())
                {
                    stringBuilder.Append(__result.TrimEnd('\n', '\r'));
                    stringBuilder.AppendLine();
                }

                // 泌乳信息
                Hediff lactating = pawn.health.hediffSet?.GetFirstHediffOfDef(HediffDefOf.Lactating);
                if (lactating != null)
                {
                    var comp = lactating.TryGetComp<HediffComp_MilkableHuman>();
                    if (comp != null)
                    {
                        string extra = comp.CompInspectStringExtra();
                        if (!extra.NullOrEmpty())
                        {
                            stringBuilder.AppendLine(extra);
                        }
                    }
                }

                // 毛发信息
                Hediff hairProductionHediff = pawn.health.hediffSet?.GetFirstHediffOfDef(DDJY_HediffDefOf.DDJY_HairProductionHediff);
                if (hairProductionHediff != null)
                {
                    var comp = hairProductionHediff.TryGetComp<HediffComp_HairHuman>();
                    if (comp != null)
                    {
                        string extra = comp.CompInspectStringExtra();
                        if (!extra.NullOrEmpty())
                        {
                            stringBuilder.AppendLine(extra);
                        }
                    }
                }

                __result = stringBuilder.ToString().TrimEnd(); 
            }
        }
    }*/
}
