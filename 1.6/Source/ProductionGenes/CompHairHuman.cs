using RimWorld;
using System.Collections.Generic;
using Verse;

namespace DDJY
{
    public class CompHairHuman : CompHasGatherableBodyResource
    {
        //基因列表
        protected List<Gene_HairProduction> geneList = new List<Gene_HairProduction>();
        //基因
        protected Gene_HairProduction nonOverriddenGene = null;
        //产物类型
        public ThingDef produce = DDJY_ThingDefOf.Milk;
        //产物数量
        public int amount = 14;
        //生产时间
        public int interval = 1;
        public CompProperties_HairGene Props
        {
            get
            {
                return (CompProperties_HairGene)this.props;
            }
        }
        //添加基因时调用
        public void AddGene(Gene_HairProduction gene)
        {
            this.geneList.Add(gene);
            SelectedNonOverriddenGene();
        }
        //移除基因时调用
        public void RemoveGene(Gene_HairProduction gene)
        {
            this.geneList.Remove(gene);
            SelectedNonOverriddenGene();
        }
        //选择激活基因
        protected void SelectedNonOverriddenGene()
        {
            if (geneList.Any())
            {
                foreach (Gene_HairProduction gene in geneList)
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
                result = this.produce.label + " " + this.Props.InspectString.Translate() +  ": " + base.Fullness.ToStringPercent();
            }
            return result;
        }
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref fullness, "DDJY_HairGeneFullness", 0f, false);
            Scribe_Values.Look(ref amount, "DDJY_HairGeneAmount", 14, false);
            Scribe_Values.Look(ref interval, "DDJY_HairGeneDays", 1, false);
            Scribe_Defs.Look(ref produce, "DDJY_HairProduct");
            Scribe_References.Look(ref nonOverriddenGene, "DDJY_NonOverriddenHairGene");
            Scribe_Collections.Look(ref geneList, "DDJY_NonOverriddenHairGeneList", LookMode.Reference);
        }
    }
}
