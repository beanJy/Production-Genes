using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace DDJY
{
    public class HediffComp_MilkableHuman : HediffComp_GatherHuman
    {
        //基因列表
        //未被覆盖的基因
        public Gene_MilkProduction nonOverriddenGene = null;
        //产物类型
        public ThingDef produce = DDJY_ThingDefOf.Milk;
        //生产时间
        public int interval = 1;
        //产物数量
        public int amount = 10;
        //生长进度 仅实现方法 无用
        private float fullness = 0f;
        //营养上限
        public float fullChargeAmount = 0.5f;
        //是否允许挤奶
        public bool isMilkingAllowed = true;
        public override float Fullness 
        {
            get
            {
                var comp = parent.TryGetComp<HediffComp_Lactating>();
                if (comp != null && fullChargeAmount > 0f)
                {
                    return comp.Charge / fullChargeAmount;
                }
                return 0f;
            }
            set 
            {
                HediffComp_Lactating comp = parent.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Lactating)?.TryGetComp<HediffComp_Lactating>();
                if (comp != null)
                {
                    comp.GreedyConsume(99999);
                }
            }
        }
        internal HediffCompProperties_MilkableHuman Props
        {
            get
            {
                return (HediffCompProperties_MilkableHuman)this.props;
            }
        }

        //添加基因时调用
        public void AddGeneData(Gene_MilkProduction gene)
        {
            DefModExtension_Gene ext = gene.def.GetModExtension<DefModExtension_Gene>();
            produce = ext.produce;
            amount = ext.amount;
            interval = ext.interval;
            fullChargeAmount = ext.fullChargeAmount;
            nonOverriddenGene = gene;
        }
        //资源数量
        protected override int ResourceAmount
        {
            get
            {
                return this.amount;
            }
        }
        //资源种类
        protected override ThingDef ResourceDef
        {
            get
            {
                return this.produce;
            }
        }
        //Patch补丁
        //替换Props.ticksToFullCharge
        public static int GetTicksToFullCharge(HediffComp_Chargeable comp)
        {
            if (IsLactatingComp(comp, out var milkHumanComp))
            {
                if (milkHumanComp != null)
                {
                    return milkHumanComp.interval * 60000;
                }
            }
            return comp.Props.ticksToFullCharge;
        }
        //替换Props.fullChargeAmount
        public static float GetFullChargeAmount(HediffComp_Chargeable comp)
        {
            if (IsLactatingComp(comp, out var milkHumanComp))
            {
                if (milkHumanComp != null)
                {   
                    return milkHumanComp.fullChargeAmount;
                }
            }
            return comp.Props.fullChargeAmount;
        }
        //判断是否是HediffComp_Lactating
        public static bool IsLactatingComp(HediffComp_Chargeable comp, out HediffComp_MilkableHuman milkHumanComp)
        {
            milkHumanComp = null;
            if (comp is HediffComp_Lactating)
            {
                milkHumanComp = comp.parent?.TryGetComp<HediffComp_MilkableHuman>();
                return true;
            }
            return false;
        }
        //判断是否应跳过生育率检测
        public static bool ShouldSkipFertilityCheck(Hediff hediff)
        {
            var comp = hediff.TryGetComp<HediffComp_MilkableHuman>();
            if (comp != null && comp.nonOverriddenGene != null)
            {
                return true;
            }
            return false;
        }
        //面板信息
        public string CompInspectStringExtra()
        {
            string result = produce.label + " ";

            if (this.Props is HediffCompProperties_MilkableHuman props && !props.InspectString.NullOrEmpty())
            {
                result += props.InspectString.Translate() + ": ";
            }

            result += Fullness.ToStringPercent();

            return result;
        }
        //添加允许挤奶按钮
        public override IEnumerable<Gizmo> CompGetGizmos()
        {
            if (base.CompGetGizmos() != null)
            {
                foreach (Gizmo gizmo in base.CompGetGizmos())
                {
                    yield return gizmo;
                }
            }
            if(parent.pawn.workSettings != null && parent.pawn.workSettings.WorkIsActive(WorkTypeDefOf.Childcare))
            {
                if(this.nonOverriddenGene == null || this.nonOverriddenGene.def == DDJY_ThingDefOf.DDJY_MilkProduction || this.nonOverriddenGene.def == DDJY_ThingDefOf.DDJY_InsectJellyProduction)
                {
                    Command_Toggle allowMilking_button = new Command_Toggle();
                    allowMilking_button.defaultLabel = "DDJY_HediffComp_MilkableHuman_AllowMilking_Label".Translate();
                    allowMilking_button.defaultDesc = "DDJY_HediffComp_MilkableHuman_AllowMilking_Desc".Translate();
                    allowMilking_button.icon = ContentFinder<Texture2D>.Get("UI/Milk", true);
                    allowMilking_button.isActive = () => isMilkingAllowed;
                    allowMilking_button.toggleAction = delegate
                    {
                        isMilkingAllowed = !isMilkingAllowed;
                    };
                    yield return allowMilking_button;
                }
            }

        }
        //基因为激活移除hediff
        public override void CompPostTickInterval(ref float severityAdjustment, int delta)
        {
            base.CompPostTickInterval(ref severityAdjustment, delta);
            if (nonOverriddenGene!=null && !nonOverriddenGene.Active)
            {
                Pawn.health.RemoveHediff(nonOverriddenGene.lactatingHediff);
            }
        }

        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_References.Look(ref nonOverriddenGene, "DDJY_NonOverriddenMilkGene");
            Scribe_Defs.Look(ref produce, "DDJY_MilkProduct");
            Scribe_Values.Look(ref amount, "DDJY_MilkGeneAmount", 10, false);
            Scribe_Values.Look(ref interval, "DDJY_MilkGeneDays", 1, false);
            Scribe_Values.Look(ref fullChargeAmount, "DDJY_MilkFullChargeAmount", 0.5f, false);
            Scribe_Values.Look(ref isMilkingAllowed, "DDJY_IsMilkingAllowed", true);
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                if (produce == null)
                {
                    produce = DDJY_ThingDefOf.Milk;
                }
            }
        }
    }
}

