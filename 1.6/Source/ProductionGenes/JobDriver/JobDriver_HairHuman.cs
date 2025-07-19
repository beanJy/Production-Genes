using RimWorld;
using Verse;

namespace DDJY
{
    public class JobDriver_HairHuman : JobDriver_GatherHumanBodyResources
    {
        protected override HediffComp_GatherHuman GetComp(Pawn targetPawn)
        {
            return targetPawn.health.hediffSet.GetFirstHediffOfDef(DDJY_HediffDefOf.DDJY_HairProductionHediff)?.TryGetComp<HediffComp_HairHuman>();
        }
    }
}
