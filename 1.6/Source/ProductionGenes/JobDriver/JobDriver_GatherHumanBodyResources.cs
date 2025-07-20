using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;

namespace DDJY
{
    public abstract class JobDriver_GatherHumanBodyResources : JobDriver
    {
        private float gatherProgress;

        protected const TargetIndex AnimalInd = TargetIndex.A;


        protected abstract HediffComp_GatherHuman GetComp(Pawn targetPawn);

        protected  float WorkTotal
        {
            get
            {
                return 400f;
            }
        }

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(job.GetTarget(TargetIndex.A), job, 1, -1, null, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            Pawn pawn = (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
            this.FailOn(() => pawn.Downed && !pawn.InBed());
            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            this.FailOnNotCasualInterruptible(TargetIndex.A);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
            Toil wait = ToilMaker.MakeToil("MakeNewToils");
            wait.initAction = delegate
            {
                Pawn actor2 = wait.actor;
                Pawn obj = (Pawn)job.GetTarget(TargetIndex.A).Thing;
                actor2.pather.StopDead();
                PawnUtility.ForceWait(obj, 15000, null, maintainPosture: true);
            };
            wait.tickIntervalAction = delegate (int delta)
            {
                Pawn actor = wait.actor;
                Pawn target = (Pawn)job.GetTarget(TargetIndex.A).Thing;
                actor.skills.Learn(SkillDefOf.Animals, 0.13f * (float)delta);
                gatherProgress += actor.GetStatValue(StatDefOf.AnimalGatherSpeed) * (float)delta;
                if (gatherProgress >= WorkTotal)
                {
                    GetComp(target).Gathered(actor);
                    actor.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
                }
            };
            wait.AddFinishAction(delegate
            {
                if (pawn != null && (pawn.CurJobDef == JobDefOf.Wait_MaintainPosture || pawn.CurJobDef == JobDefOf.LayDownAwake))
                {
                    pawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
                }
            });
            wait.FailOnDespawnedOrNull(TargetIndex.A);
            wait.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
            wait.AddEndCondition(() => GetComp((Pawn)(Thing)job.GetTarget(TargetIndex.A)).ActiveAndFull ? JobCondition.Ongoing : JobCondition.Incompletable);
            wait.defaultCompleteMode = ToilCompleteMode.Never;
            wait.WithProgressBar(TargetIndex.A, () => gatherProgress / WorkTotal);
            wait.activeSkill = () => SkillDefOf.Animals;
            yield return wait;
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref gatherProgress, "DDJY_gatherProgress", 0f);
        }
    }
}