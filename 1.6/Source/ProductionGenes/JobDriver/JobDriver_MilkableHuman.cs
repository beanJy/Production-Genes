using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace DDJY
{
    public class JobDriver_MilkableHuman : JobDriver_GatherHumanBodyResources
    {
        protected override HediffComp_GatherHuman GetComp(Pawn targetPawn)
        {
            return targetPawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Lactating)?.TryGetComp<HediffComp_MilkableHuman>();
        }
    }
}