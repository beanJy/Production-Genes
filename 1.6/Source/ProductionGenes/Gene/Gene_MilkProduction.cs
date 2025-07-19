using RimWorld;
using Verse;

namespace DDJY
{
    public class Gene_MilkProduction : Gene
    {
        public Hediff lactatingHediff;

        public override void PostAdd() 
        {
            base.PostAdd();
            {
                if(Active)
                {
                    AddLactatingHediff();
                }
            }
        }
        public override void PostRemove()
        {
            base.PostRemove();
            { 
                RemoveLactatingHediff();
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
                AddLactatingHediff();
            }
        }
        //添加哺乳期
        private void AddLactatingHediff()
        {
            if (pawn?.health?.hediffSet == null)
                return;

            Hediff existing = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Lactating);

            if (existing != null)
            {
                if (existing == lactatingHediff)
                {
                    //刷新严重程度 防止10天取消哺乳期
                    existing.Severity = 1f;
                    return;
                }
                else
                {
                    // 不是自己添加的，先移除
                    pawn.health.RemoveHediff(existing);
                }
            }

            // 添加自己管理的哺乳Hediff并记录
            lactatingHediff = pawn.health.AddHediff(HediffDefOf.Lactating);
            var hediffComp_MilkableHuman = lactatingHediff.TryGetComp<HediffComp_MilkableHuman>();
            var hediffComp_Lactating = lactatingHediff.TryGetComp<HediffComp_Lactating>();
            if (hediffComp_MilkableHuman != null)
            {
                hediffComp_MilkableHuman.AddGeneData(this);
            }
            if(hediffComp_Lactating != null)
            {
               hediffComp_Lactating.GreedyConsume(99999);
            }
        }
        //移除哺乳期
        private void RemoveLactatingHediff()
        {
            if (pawn?.health?.hediffSet == null) return;

            if (lactatingHediff != null && pawn.health.hediffSet.hediffs.Contains(lactatingHediff))
            {
                pawn.health.RemoveHediff(lactatingHediff);
                lactatingHediff = null;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look(ref lactatingHediff, "DDJY_lactatingHediff");

        }
    }


}
