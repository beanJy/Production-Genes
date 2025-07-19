﻿using RimWorld;
using Verse;

namespace DDJY
{
    [DefOf]
    public static class DDJY_JobDefOf
    {
        static DDJY_JobDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(DDJY_JobDefOf));
        }

        public static JobDef DDJY_MilkableHuman;
        public static JobDef DDJY_HairHuman;
    }
}
