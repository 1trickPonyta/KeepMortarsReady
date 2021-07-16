using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Verse;

namespace RimWorld
{
    class MortarExt : Building_TurretGun
    {
        public bool ShouldKeepReady = true;

        // Determines if the mortar is currently ready to fire at a moment's notice
        public bool IsReady
        {
            get
            {
                if (this.gun != null)
                {
                    CompChangeableProjectile compChangeableProjectile = this.gun.TryGetComp<CompChangeableProjectile>();
                    KeepMortarsReady.Debug.Log("Loaded: " + compChangeableProjectile.Loaded);

                    bool coolingDown = this.burstCooldownTicksLeft > 0;
                    KeepMortarsReady.Debug.Log("Cooldown complete: " + !coolingDown);

                    if (compChangeableProjectile != null && (!compChangeableProjectile.Loaded || coolingDown))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<bool>(ref this.ShouldKeepReady, "ShouldKeepReady", false, false);
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Gizmo gizmo in base.GetGizmos())
            {
                yield return gizmo;
            }

            if (this.Faction == Faction.OfPlayer)
            {
                yield return new Command_Toggle
                {
                    defaultLabel = "KeepMortarsReady_CommandKeepReady".Translate(),
                    defaultDesc = "KeepMortarsReady_CommandKeepReadyDesc".Translate(),
                    icon = ContentFinder<Texture2D>.Get("UI/KeepReady", true),
                    toggleAction = delegate ()
                    {
                        this.ShouldKeepReady = !this.ShouldKeepReady;
                    },
                    isActive = (() => this.ShouldKeepReady)
                };
            }
        }
    }
}
