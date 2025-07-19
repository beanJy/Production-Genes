using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace DDJY
{
    public class WorkGiver_HairHuman : WorkGiver_GatherHumanBodyResources
    {
        protected override JobDef JobDef
        {
            get
            {
                return DDJY_JobDefOf.DDJY_HairHuman;
            }
        }
        protected HediffComp_HairHuman GetComp(Pawn targetPawn)
        {
            if (targetPawn.health != null)
            {
                Hediff hairProductionHediff = targetPawn.health.hediffSet?.GetFirstHediffOfDef(DDJY_HediffDefOf.DDJY_HairProductionHediff);
                if (hairProductionHediff != null)
                {
                    return hairProductionHediff.TryGetComp<HediffComp_HairHuman>();

                }
            }
            return null;
        }

        public override bool ShouldSkip(Pawn targetPawn, bool forced = false)
        {
            List<Pawn> list = targetPawn.Map.mapPawns.FreeColonistsAndPrisonersSpawned;
            for (int i = 0; i < list.Count; i++)
            {
                if (!list[i].RaceProps.Humanlike) continue;

                HediffComp_HairHuman comp = this.GetComp(list[i]);
                if (comp != null && comp.ActiveAndFull)
                {
                    return false;
                }
            }
            return true;
        }

        public override bool HasJobOnThing(Pawn workerPawn, Thing t, bool forced = false)
        {
            Pawn targetPawn = t as Pawn;
            if (targetPawn == null || !targetPawn.RaceProps.Humanlike || targetPawn.Drafted || targetPawn.InAggroMentalState || targetPawn.IsFormingCaravan() || targetPawn == workerPawn)
            {
                return false;
            }
            if (targetPawn.Downed && !targetPawn.InBed())
            {
                return false;
            }
            HediffComp_HairHuman comp = this.GetComp(targetPawn);
            return comp != null && comp.ActiveAndFull && (targetPawn.roping == null || !targetPawn.roping.IsRopedByPawn) && targetPawn.CanCasuallyInteractNow(false, false, false) && workerPawn.CanReserve(targetPawn, 1, -1, null, forced);
        }
    }
}
