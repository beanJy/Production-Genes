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

        protected override void ChangeFullness(Pawn pawn) {
            HediffComp_Lactating comp = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Lactating)?.TryGetComp<HediffComp_Lactating>();
            Log.Message(pawn);
            if (comp != null)
            {
                comp.GreedyConsume(9999999999999999999);
                Log.Message("JINRU ChangeFullness");
            }
        }
    }
}