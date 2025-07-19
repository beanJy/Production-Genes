using RimWorld;
using System;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using Verse;

namespace DDJY
{
    public abstract class HediffComp_GatherHuman : HediffComp
    {

        public abstract float Fullness { get; set; }

        protected abstract int ResourceAmount { get; }

        protected abstract ThingDef ResourceDef { get; }

        protected virtual bool Active
        {
            get
            {
                if (Pawn.Faction == null)
                {
                    return false;
                }

                if (Pawn.Suspended)
                {
                    return false;
                }

                return true;
            }
        }
        public bool ActiveAndFull
        {
            get
            {
                if (!Active)
                {
                    return false;
                }

                return Fullness >= 1f;
            }
        }
        public void Gathered(Pawn doer)
        {
            if (!Rand.Chance(doer.GetStatValue(StatDefOf.AnimalGatherYield)))
            {
                MoteMaker.ThrowText((doer.DrawPos + Pawn.DrawPos) / 2f, Pawn.Map, "TextMote_ProductWasted".Translate(), 3.65f);
            }
            else
            {
                int num = GenMath.RoundRandom((float)ResourceAmount * Fullness);
                while (num > 0)
                {
                    int num2 = Mathf.Clamp(num, 1, ResourceDef.stackLimit);
                    num -= num2;
                    Thing thing = ThingMaker.MakeThing(ResourceDef);
                    thing.stackCount = num2;
                    GenPlace.TryPlaceThing(thing, doer.Position, doer.Map, ThingPlaceMode.Near);
                }
            }
            this.Fullness = 0f;
        }

        public override void CompExposeData()
        {
            base.CompExposeData();
        }
    }
}

