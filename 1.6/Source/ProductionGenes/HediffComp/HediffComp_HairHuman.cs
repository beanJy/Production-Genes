using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace DDJY
{
    public class HediffComp_HairHuman : HediffComp_GatherHuman
    {
        //未被覆盖的基因
        public Gene_HairProduction nonOverriddenGene = null;
        //产物类型
        public ThingDef produce = DDJY_ThingDefOf.WoolSheep;
        //生产时间
        public int interval = 10;
        //产物数量
        public int amount = 45;
        //营养上限
        public float fullChargeAmount = 0.5f;
        //生长进度
        private float fullness = 0f;
        public bool isOn = false;
        public override float Fullness 
        {
            get => this.fullness;
            set => this.fullness = value;
        }
        internal HediffCompProperties_HairProduction Props
        {
            get
            {
                return (HediffCompProperties_HairProduction)this.props;
            }
        }

        //添加基因时调用
        public void AddGeneData(Gene_HairProduction gene)
        {
            DefModExtension_Gene ext = gene.def.GetModExtension<DefModExtension_Gene>();
            produce = ext.produce;
            amount = ext.amount;
            interval = ext.interval;
            fullness = 0f;
            nonOverriddenGene = gene;
        }
        //tick间隔调用
        public override void CompPostTickInterval(ref float severityAdjustment, int delta)
        {
            {
                if (Active)
                {
                    float num = 1f / (float)(interval * 60000);
                    Pawn pawn = parent.pawn;
                    if (pawn != null)
                    {
                        num *= PawnUtility.BodyResourceGrowthSpeed(pawn);
                    }

                    fullness += num;
                    if (fullness > 1f)
                    {
                        fullness = 1f;
                    }
                }
            }

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
        //面板信息
        public string CompInspectStringExtra()
        {
            string result = produce.label + " ";

            if (this.Props is HediffCompProperties_HairProduction props && !props.InspectString.NullOrEmpty())
            {
                result += props.InspectString.Translate() + ": ";
            }

            result += Fullness.ToStringPercent();

            return result;
        }
        //健康面板隐藏Hediff
        public override bool CompDisallowVisible()
        {
            return true;
        }
        public override string CompLabelInBracketsExtra
        {
            get
            {
                return this.Props.labelInBrackets.Formatted(this.produce.label.Named("PRODUCENAME"), (this.Fullness).Named("CHARGEFACTOR"));
            }
        }
        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_References.Look(ref nonOverriddenGene, "DDJY_NonOverriddenHairGene");
            Scribe_Defs.Look(ref produce, "DDJY_MilkProduct");
            Scribe_Values.Look(ref amount, "DDJY_HairGeneAmount", 10, false);
            Scribe_Values.Look(ref interval, "DDJY_HairGeneDays", 1, false);
            Scribe_Values.Look(ref fullness, "DDJY_HairGeneFullness", 0f, false);
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                if (produce == null)
                {
                    produce = DDJY_ThingDefOf.WoolSheep;
                }
            }
        }
    }
}

