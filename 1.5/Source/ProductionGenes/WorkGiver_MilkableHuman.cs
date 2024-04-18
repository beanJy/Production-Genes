using RimWorld;
using Verse;

namespace DDJY
{
    public class WorkGiver_MilkableHuman : WorkGiver_GatherHumanBodyResources
    {
        protected override JobDef JobDef
        {
            get
            {
                return DDJY_JobDefOf.DDJY_MilkableHuman;
            }
        }
        protected override CompHasGatherableBodyResource GetComp(Pawn animal)
        {
            return animal.TryGetComp<CompMilkableHuman>();
        }
    }
}
