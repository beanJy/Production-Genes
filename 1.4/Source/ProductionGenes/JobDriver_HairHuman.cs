using RimWorld;
using Verse;

namespace DDJY
{
    public class JobDriver_HairHuman : JobDriver_GatherHumanBodyResources
    {
        protected override CompHasGatherableBodyResource GetComp(Pawn animal)
        {
            return animal.TryGetComp<CompHairHuman>();
        }
    }
}
