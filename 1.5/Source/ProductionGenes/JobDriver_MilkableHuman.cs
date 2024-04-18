using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace DDJY
{
    public class JobDriver_MilkableHuman : JobDriver_GatherHumanBodyResources
    {
        protected override CompHasGatherableBodyResource GetComp(Pawn animal)
        {
            return animal.TryGetComp<CompMilkableHuman>();
        }
    }
}