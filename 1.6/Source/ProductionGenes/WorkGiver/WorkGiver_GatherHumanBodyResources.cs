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

        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            return pawn.Map.mapPawns.FreeColonistsAndPrisonersSpawned;
        }

        public override PathEndMode PathEndMode
        {
            get
            {
                return PathEndMode.Touch;
            }
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            return JobMaker.MakeJob(this.JobDef, t);
        }
    }

}
