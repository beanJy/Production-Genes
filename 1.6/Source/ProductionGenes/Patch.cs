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
            var GetTicksToFullCharge = AccessTools.Method(typeof(CompMilkableHuman), "GetTicksToFullCharge");
            var GetFullChargeAmount = AccessTools.Method(typeof(CompMilkableHuman), "GetFullChargeAmount");
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
                    Log.Message("1");
                }
                if
                (
                   codes[i].Calls(get_Props) &&
                   codes[i + 1].LoadsField(field_fullChargeAmount)
                )
                {
                    codes[i] = new CodeInstruction(OpCodes.Call, GetFullChargeAmount);
                    codes[i + 1] = new CodeInstruction(OpCodes.Nop);
                    Log.Message("2");
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
            var GetTicksToFullCharge = AccessTools.Method(typeof(CompMilkableHuman), "GetTicksToFullCharge");
            var GetFullChargeAmount = AccessTools.Method(typeof(CompMilkableHuman), "GetFullChargeAmount");
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
                    Log.Message("3");
                }
                if
                (
                   codes[i].Calls(get_Props) &&
                   codes[i + 1].LoadsField(field_fullChargeAmount)
                )
                {
                    codes[i] = new CodeInstruction(OpCodes.Call, GetFullChargeAmount);
                    codes[i + 1] = new CodeInstruction(OpCodes.Nop);
                    Log.Message("4");
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
            var GetFullChargeAmount = AccessTools.Method(typeof(CompMilkableHuman), "GetFullChargeAmount");
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
                    Log.Message("5");
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
            var GetFullChargeAmount = AccessTools.Method(typeof(CompMilkableHuman), "GetFullChargeAmount");
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
                    Log.Message("6");
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
        /*public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var codes = new List<CodeInstruction>(instructions);
            var get_Props = AccessTools.PropertyGetter(typeof(HediffComp_Chargeable), "Props");
            var field_fullChargeAmount = AccessTools.Field(typeof(HediffCompProperties_Chargeable), "fullChargeAmount");
            var GetFullChargeAmount = AccessTools.Method(typeof(CompMilkableHuman), "GetFullChargeAmount");
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
                    Log.Message("7");
                }
            }
            return codes;
        }*/
        public static bool Prefix(HediffComp_Chargeable __instance, ref string __result)
        {
            Pawn pawn = __instance.parent.pawn;
            CompMilkableHuman comp = pawn.TryGetComp<CompMilkableHuman>();
            if (comp != null )
            {
                string produceName = comp.produce.label;
                float fullness = comp.Fullness;

                __result = __instance.Props.labelInBrackets.Formatted(
                    fullness.Named("CHARGEFACTOR"),
                    produceName.Named("PRODUCENAME")
                );
                return false; 
            }
            return true;
        }
    }

    //修改 HediffComp_Chargeable.CompPostMake
    //基因哺乳状态起始奶量清零
    [HarmonyPatchCategory("ProductionGenes_Patch")]
    [HarmonyPatch(typeof(HediffComp_Chargeable), "CompPostMake")]
    internal static class HediffComp_Chargeable_CompPostMake
    {
        public static void Postfix(HediffComp_Chargeable __instance)
        {
            CompMilkableHuman comp = __instance.Pawn.TryGetComp<CompMilkableHuman>();
            if(comp != null && comp.nonOverriddenGene != null && comp.nonOverriddenGene.Active)
            {
                __instance.GreedyConsume(999999999);
            }
        }
    }

}
