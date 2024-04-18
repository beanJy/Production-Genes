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
        protected Gene_MilkProduction nonOverriddenGene = null;
        //产物类型
        public ThingDef produce = DDJY_ThingDefOf.Milk;
        //产物数量
        public int amount = 14;
        //生产时间
        public int interval = 1;
        public CompProperties_MilkableGene Props
        {
            get
            {
                return (CompProperties_MilkableGene)this.props;
            }
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
            if (geneList.Any())
            {
                foreach(Gene_MilkProduction gene in geneList)
                {
                    if (!gene.Overridden && gene != nonOverriddenGene)
                    {
                        DefModExtension_Gene ext = gene.def.GetModExtension<DefModExtension_Gene>();
                        produce = ext.produce;
                        amount = ext.amount;
                        interval = ext.interval;
                        Fullness = 0f;
                        nonOverriddenGene = gene;
                    }
                }
            }
            else
            {
                nonOverriddenGene = null;
            }
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
        protected override bool Active
        {
            get
            {
                if (base.Active && nonOverriddenGene != null && nonOverriddenGene.Active)
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
            Scribe_Defs.Look(ref produce, "DDJY_MilkProduct");
            Scribe_References.Look(ref nonOverriddenGene, "DDJY_NonOverriddenMilkGene");
            Scribe_Collections.Look(ref geneList, "DDJY_NonOverriddenMilkGeneList", LookMode.Reference);
        }
    }
}

