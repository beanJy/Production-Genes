using RimWorld;
using Verse;

namespace DDJY
{
    [DefOf]
    public static class DDJY_HediffDefOf
    {
        static DDJY_HediffDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(DDJY_HediffDefOf));
        }

        public static HediffDef DDJY_HairProductionHediff;
    }
}