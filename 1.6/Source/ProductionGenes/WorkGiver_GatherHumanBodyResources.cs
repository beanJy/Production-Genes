using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;
using RimWorld.Planet;

namespace DDJY
{
    public abstract class WorkGiver_GatherHumanBodyResources : WorkGiver_Scanner
    {

        protected abstract JobDef JobDef { get; }

        protected abstract CompHasGatherableBodyResource GetComp(Pawn Humanlike);

        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            return pawn.Map.mapPawns.FreeColonistsAndPrisonersSpawned;
        }

        public override bool ShouldSkip(Pawn pawn, bool forced = false)
        {
            List<Pawn> list = pawn.Map.mapPawns.FreeColonistsAndPrisonersSpawned;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].RaceProps.Humanlike)
                {
                    CompHasGatherableBodyResource comp = this.GetComp(list[i]);
                    if (comp != null && comp.ActiveAndFull)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public override PathEndMode PathEndMode
        {
            get
            {
                return PathEndMode.Touch;
            }
        }

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Pawn pawn2 = t as Pawn;
            if (pawn2 == null || !pawn2.RaceProps.Humanlike || pawn2.Drafted || pawn2.InAggroMentalState || pawn2.IsFormingCaravan()|| pawn2 == pawn )
            {
                return false;
            }
            if (pawn2.Downed && !pawn2.InBed())
            {
                return false;
            }
            CompHasGatherableBodyResource comp = this.GetComp(pawn2);
            return comp != null && comp.ActiveAndFull &&  (pawn2.roping == null || !pawn2.roping.IsRopedByPawn) && pawn2.CanCasuallyInteractNow(false, false, false) && pawn.CanReserve(pawn2, 1, -1, null, forced);
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            return JobMaker.MakeJob(this.JobDef, t);
        }
    }

}
