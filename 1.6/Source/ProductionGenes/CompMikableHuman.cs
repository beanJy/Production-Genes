using RimWorld;
using System.Collections.Generic;
using Verse;

namespace DDJY
{
    public class CompMilkableHuman : CompHasGatherableBodyResource
    {
        //基因列表
        protected List<Gene_MilkProduction> geneList = new List<Gene_MilkProduction>();
        //未被覆盖的基因
        public Gene_MilkProduction nonOverriddenGene = null;
        //牛奶基因
        DefModExtension_Gene milk_GeneData;
        public ThingDef produce;
        //产物数量
        public int amount;
        //生产时间
        public int interval;
        //营养上限
        public float fullChargeAmount;
        public CompProperties_MilkableGene Props
        {
            get
            {
                return (CompProperties_MilkableGene)this.props;
            }
        }
        //初始化
        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
            milk_GeneData = DDJY_ThingDefOf.DDJY_MilkProduction.GetModExtension<DefModExtension_Gene>();
            produce = milk_GeneData.produce;
            amount = milk_GeneData.amount; 
            interval = milk_GeneData.interval;
            fullChargeAmount = milk_GeneData.fullChargeAmount;
        }

        //添加基因时调用
        public void AddGene(Gene_MilkProduction gene)
        {
            this.geneList.Add(gene);
            SelectedNonOverriddenGene();
        }
        //移除基因时调用
        public void RemoveGene(Gene_MilkProduction gene)
        {
            this.geneList.Remove(gene);
            SelectedNonOverriddenGene();
        }
        //选择激活基因
        protected void SelectedNonOverriddenGene()
        {
            // 尝试找到第一个未被覆盖的激活基因
            if (geneList.Any())
            {
                foreach (Gene_MilkProduction gene in geneList)
                {
                    if (!gene.Overridden && gene != nonOverriddenGene && gene.Active)
                    {
                        DefModExtension_Gene ext = gene.def.GetModExtension<DefModExtension_Gene>();
                        produce = ext.produce;
                        amount = ext.amount;
                        interval = ext.interval;
                        fullChargeAmount = ext.fullChargeAmount;
                        Fullness = 0f;
                        nonOverriddenGene = gene;
                        AddLactatingHediff(parent as Pawn);
                        Log.Message(" SelectedNonOverriddenGene interval" + interval);

                        return;
                    }
                }
            }
            //没有激活基因，使用默认基因配置
            Pawn pawn = parent as Pawn;
            produce = milk_GeneData.produce;
            amount = milk_GeneData.amount;
            interval = milk_GeneData.interval;
            fullChargeAmount = milk_GeneData.fullChargeAmount;
            nonOverriddenGene = null;
            Hediff lactating = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Lactating);
            if (lactating != null)
            {
                pawn.health.RemoveHediff(lactating);
            }
        }
        //添加哺乳期
        public static void AddLactatingHediff(Pawn pawn)
        {
            if (pawn == null || pawn.health == null || pawn.health.hediffSet == null)
                return;

            var oldHediff = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Lactating);
            if (oldHediff != null)
            {
                pawn.health.RemoveHediff(oldHediff);
            }
            pawn.health.AddHediff(HediffDefOf.Lactating);
        }
        public override void CompTick()
        {
            if (Active)
            {
                
                Pawn pawn = parent as Pawn;
                if (pawn != null)
                {
                    HediffComp_Lactating comp = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Lactating)?.TryGetComp<HediffComp_Lactating>();
                    fullness = comp.Charge / fullChargeAmount;
                }
            }
        }
        public static int GetTicksToFullCharge(HediffComp_Chargeable comp)
        {
            if (IsLactatingComp(comp, out var milkComp))
            {
                if (milkComp != null)
                {
                    Log.Message(" GetTicksToFullCharge" + milkComp.interval);
                    return milkComp.interval * 60000;

                }
            }
            return comp.Props.ticksToFullCharge;
        }

        public static float GetFullChargeAmount(HediffComp_Chargeable comp)
        {
            if (IsLactatingComp(comp, out var milkComp))
            {
                if (milkComp != null)
                {
                    return milkComp.fullChargeAmount;
                }
            }
            return comp.Props.fullChargeAmount;
        }

        public static bool IsLactatingComp(HediffComp_Chargeable comp, out CompMilkableHuman milkComp)
        {
            milkComp = null;
            if (comp is HediffComp_Lactating)
            {
                milkComp = comp.Pawn?.TryGetComp<CompMilkableHuman>();
                return true;
            }
            return false;
        }
        protected override int GatherResourcesIntervalDays
        {
            get
            {
                return this.interval;
            }
        }
        protected override int ResourceAmount
        {
            get
            {
                return this.amount;
            }
        }
        protected override ThingDef ResourceDef
        {
            get
            {
                return this.produce;
            }
        }
        protected override string SaveKey
        {
            get
            {
                return "Fullness";
            }
        }
        public new float Fullness
        {
            get
            {
                return this.fullness;
            }
            set
            {
                this.fullness = value;
            }
        }
        //激活判定
        protected override bool Active
        {
            get
            {

                /*if (base.Active && nonOverriddenGene != null && nonOverriddenGene.Active)
                {
                    return true;
                }*/
                Hediff lactating = (parent as Pawn)?.health?.hediffSet?.GetFirstHediffOfDef(HediffDefOf.Lactating);
                if (base.Active && lactating != null)
                {
                    return true;
                }
                return false;
            }
        }
        public override string CompInspectStringExtra()
        {
            bool flag = !this.Active;
            string result;
            if (flag)
            {
                result = null;
            }
            else
            {
                result = this.produce.label + " " +  this.Props.InspectString.Translate() + ": " + base.Fullness.ToStringPercent();
            }
            return result;
        }
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref fullness, "DDJY_MilkGeneFullness", 0f, false);
            Scribe_Values.Look(ref amount, "DDJY_MilkGeneAmount", 14, false);
            Scribe_Values.Look(ref interval, "DDJY_MilkGeneDays", 1, false);
            Scribe_Values.Look(ref fullness, "DDJY_MilkGeneFullness", 0f, false);
            Scribe_Values.Look(ref fullChargeAmount, "DDJY_fullChargeAmount", 0.5f, false);
            Scribe_Defs.Look(ref produce, "DDJY_MilkProduct");
            Scribe_References.Look(ref nonOverriddenGene, "DDJY_NonOverriddenMilkGene");
            Scribe_Collections.Look(ref geneList, "DDJY_NonOverriddenMilkGeneList", LookMode.Reference);
        }
    }
}

