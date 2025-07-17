using RimWorld;
using Verse;

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
        protected override CompHasGatherableBodyResource GetComp(Pawn animal)
        {
            return animal.TryGetComp<CompHairHuman>();
        }
    }
}
