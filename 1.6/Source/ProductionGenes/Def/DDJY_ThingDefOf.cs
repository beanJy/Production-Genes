using RimWorld;
using Verse;

namespace DDJY
{
    [DefOf]
    public static class DDJY_ThingDefOf
    {
        static DDJY_ThingDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(DDJY_ThingDefOf));
        }

        public static ThingDef Milk;
        public static ThingDef WoolSheep;
        public static GeneDef DDJY_MilkProduction;
        public static GeneDef DDJY_InsectJellyProduction;
    }
}