using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;
using KeepMortarsReady;

namespace RimWorld
{
    public class JobDriver_ReadyMortar : JobDriver_ManTurret
    {
        // We override this method just to hook into the "man" toil's tickAction
        protected override IEnumerable<Toil> MakeNewToils()
        {
            foreach (Toil toil in base.MakeNewToils())
            {
                if (toil.defaultCompleteMode == ToilCompleteMode.Never && toil.tickAction != null)
                {
                    Debug.Log("Found man toil");

                    Action action = (Action) toil.tickAction.Clone();
                    toil.tickAction = delegate ()
                    {
                        action();

                        // During each tick while the mortar is manned, we need to check if it is ready to fire so we can stop the job
                        // before the mortar actually fires
                        MortarExt mortar = toil.actor.CurJob.targetA.Thing as MortarExt;
                        if (mortar.IsReady)
                        {
                            toil.actor.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
                        }
                        else if (!mortar.ShouldKeepReady)
                        {
                            toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
                        }
                    };
                }
                yield return toil;
            }
        }
    }
}
