using RimWorld;
using Verse;

namespace DDJY
{
    [DefOf]
    public static class DDJY_GeneDefOf
    {
        static DDJY_GeneDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(DDJY_GeneDefOf));
        }

        public static ThingDef Milk;
    }
}