using RimWorld;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Resources;
using UnityEngine;
using Verse;
using Verse.AI;

namespace DDJY
{
    public abstract class HediffComp_GatherHuman : HediffComp
    {
        //产物列表列表
        private static List<Thing> geneMilkList = new List<Thing>();
        //保存用
        private List<Thing> geneMilkList_Serializable = new List<Thing>();
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
                    if (thing.def.ingestible != null)
                    {
                        //添加到追踪列表
                        geneMilkList.Add(thing);
                    }
                }
            }
            this.Fullness = 0f;
        }
        //thing是否在产物列表中
        public static bool IsInGeneMilkList(Thing t)
        {
            for (int i = geneMilkList.Count - 1; i >= 0; i--)
            {
                Thing milk = geneMilkList[i];

                // 已销毁或未生成，移除
                if (milk == null || milk.DestroyedOrNull())
                {
                    geneMilkList.Remove(milk);
                    continue;
                }

                //不在囚犯房移除
                Room room = milk.GetRoom();
                if (room != null && room.Role != RoomRoleDefOf.PrisonBarracks && room.Role != RoomRoleDefOf.PrisonCell)
                {
                    geneMilkList.Remove(milk);
                }
            }

            if (geneMilkList.Contains(t))
            {
                return true;
            }
            return false;
        }
        public override void CompExposeData()
        {
            base.CompExposeData();
            if (Scribe.mode == LoadSaveMode.Saving)
            {
                geneMilkList_Serializable = geneMilkList;
            }

            // 读写实例字段
            Scribe_Collections.Look(ref geneMilkList_Serializable, "DDJY_geneMilkList", LookMode.Reference);

            // 读档后将实例字段赋值给静态字段
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                geneMilkList = geneMilkList_Serializable ?? new List<Thing>();
            }

        }
    }
}

