using RimWorld;
using UnityEngine;
using Verse;

namespace DDJY
{
    public class Gene_HairProduction : Gene
    {
        private Hediff hairProductionHediff;
        public override void PostAdd()
        {
            base.PostAdd();
            if (Active)
            {
                AddHairProductionHediff();
            }
        }
        public override void PostRemove()
        {
            base.PostRemove();
            if (Active)
            {
                RemoveHairProductionHediff();
            }
        }
        public override bool Active
        {
            get
            {
                if (base.Active)
                {
                    DefModExtension_Gene ext = def.GetModExtension<DefModExtension_Gene>();
                    if (!ext.femaleOnly || pawn.gender == Gender.Female)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public override void TickInterval(int delta)
        {
            base.TickInterval(delta);
            if (Active)
            {
                AddHairProductionHediff();
            }
            else
            {
                RemoveHairProductionHediff();
            }
        }

        private void AddHairProductionHediff()
        {

            if (pawn?.health?.hediffSet == null)
                return;

            Hediff existing = pawn.health.hediffSet.GetFirstHediffOfDef(DDJY_HediffDefOf.DDJY_HairProductionHediff);

            if (existing != null)
            {
                if (existing == hairProductionHediff)
                {
                    return;
                }
                else
                {
                    // 不是自己添加的，先移除
                    pawn.health.RemoveHediff(existing);
                }
            }

            // 添加自己管理的哺乳Hediff并记录
            hairProductionHediff = pawn.health.AddHediff(DDJY_HediffDefOf.DDJY_HairProductionHediff);
            var hediffComp_HairHuman = hairProductionHediff.TryGetComp<HediffComp_HairHuman>();
            if (hediffComp_HairHuman != null)
            {
                hediffComp_HairHuman.AddGeneData(this);
            }
        }
        private void RemoveHairProductionHediff()
        {
            if (pawn?.health?.hediffSet == null) return;

            if (hairProductionHediff != null && pawn.health.hediffSet.hediffs.Contains(hairProductionHediff))
            {
                pawn.health.RemoveHediff(hairProductionHediff);
                hairProductionHediff = null;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look(ref hairProductionHediff, "DDJY_hairProductionHediff");

        }
    }

}
