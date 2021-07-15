using System.Collections.Generic;
using Verse;
using Verse.AI;
using KeepMortarsReady;

namespace RimWorld
{
	public class WorkGiver_ReadyMortar : WorkGiver_Scanner
	{
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.Turret_Mortar);
			}
		}

		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			List<Thing> list = pawn.Map.listerThings.ThingsOfDef(ThingDefOf.Turret_Mortar);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Faction == pawn.Faction)
				{
					MortarExt mortar = list[i] as MortarExt;
					Debug.Log("mortar: " + list[i].GetType());
					if (!mortar.IsReady && mortar.ShouldKeepReady)
                    {
						Debug.Log("ShouldSkip returning false for " + pawn.Name);
						return false;
					}
				}
			}
			Debug.Log("ShouldSkip returning true for " + pawn.Name);
			return true;
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Debug.Log("HasJobOnThing called");
			if (t.Faction != pawn.Faction)
			{
				return false;
			}
			MortarExt building = t as MortarExt;
			Debug.Log("building: " + building);
			return building != null && !building.IsForbidden(pawn) && pawn.CanReserve(building, 1, -1, null, forced) && !building.IsBurning() && !building.IsReady && building.ShouldKeepReady;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Debug.Log("JobOnThing called");

			// We could simply set the expiry interval to something much smaller here instead of using 
			// the man toil's tickAction to check if the job should end, but it seems dangerous to do so
			// unless we set the interval to exactly 1 tick which is probably not a best practice??
			return JobMaker.MakeJob(DefDatabase<JobDef>.GetNamed("ReadyMortar"), t, 1500, true);
		}
	}
}
