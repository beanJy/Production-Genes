using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace DDJY
{
    public class JobDriver_GatherHumanBodyResources : JobDriver_GatherAnimalBodyResources
    {
        protected override float WorkTotal
        {
            get
            {
                return 400f;
            }
        }
        protected override CompHasGatherableBodyResource GetComp(Pawn animal)
        {
            return null;
        }
        protected override IEnumerable<Toil> MakeNewToils()
        {
            Pawn pawn = (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
            this.FailOn(() => pawn.Downed && !pawn.InBed());
            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            this.FailOnNotCasualInterruptible(TargetIndex.A);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
            Toil wait = ToilMaker.MakeToil("MakeNewToils");
            wait.initAction = delegate ()
            {
                Pawn actor = wait.actor;
                actor.pather.StopDead();
                PawnUtility.ForceWait(pawn, 15000, null, true, false);
            };
            wait.tickAction = delegate ()
            {
                Pawn actor = wait.actor;
                actor.skills.Learn(SkillDefOf.Animals, 0.13f, false);
                this.gatherProgress += actor.GetStatValue(StatDefOf.AnimalGatherSpeed, true, -1);
                bool flag = this.gatherProgress >= this.WorkTotal;
                if (flag)
                {
                    this.GetComp((Pawn)((Thing)this.job.GetTarget(TargetIndex.A))).Gathered(this.pawn);
                    actor.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
                }
            };
            wait.AddFinishAction(delegate
            {
                bool flag = pawn != null && pawn.CurJobDef == JobDefOf.Wait_MaintainPosture;
                if (flag)
                {
                    pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
                }
            });
            wait.FailOnDespawnedOrNull(TargetIndex.A);
            wait.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
            wait.AddEndCondition(delegate
            {
                bool flag = !this.GetComp((Pawn)((Thing)this.job.GetTarget(TargetIndex.A))).ActiveAndFull;
                JobCondition result;
                if (flag)
                {
                    result = JobCondition.Incompletable;
                }
                else
                {
                    result = JobCondition.Ongoing;
                }
                return result;
            });
            wait.defaultCompleteMode = ToilCompleteMode.Never;
            wait.WithProgressBar(TargetIndex.A, () => this.gatherProgress / this.WorkTotal, false, -0.5f, false);
            wait.activeSkill = (() => SkillDefOf.Animals);
            yield return wait;
            yield break;
        }
        private float gatherProgress;
    }
}