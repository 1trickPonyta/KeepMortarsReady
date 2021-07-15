using System.Collections.Generic;
using UnityEngine;
using RimWorld;
using Verse;
using HarmonyLib;

namespace KeepMortarsReady
{
    // Adds the gizmo for toggling keeping a mortar ready
    [HarmonyPatch(typeof(Building_TurretGun))]
    [HarmonyPatch("GetGizmos")]
    class Patch_Building_TurretGun
    {
        public static IEnumerable<Gizmo> Postfix(IEnumerable<Gizmo> gizmos, MortarExt __instance)
        {
            foreach (Gizmo gizmo in gizmos)
            {
                yield return gizmo;
            }

			if (__instance.Faction == Faction.OfPlayer)
			{
				yield return new Command_Toggle
				{
					defaultLabel = "KeepMortarsReady_CommandKeepReady".Translate(),
					defaultDesc = "KeepMortarsReady_CommandKeepReadyDesc".Translate(),
					icon = ContentFinder<Texture2D>.Get("UI/KeepReady", true),
					toggleAction = delegate ()
					{
						__instance.ShouldKeepReady = !__instance.ShouldKeepReady;
					},
					isActive = (() => __instance.ShouldKeepReady)
				};
			}
		}
    }
}
