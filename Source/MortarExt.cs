using System.Reflection;
using Verse;
using KeepMortarsReady;

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
                if (gun != null)
                {
                    CompChangeableProjectile compChangeableProjectile = gun.TryGetComp<CompChangeableProjectile>();
                    Debug.Log("Loaded: " + compChangeableProjectile.Loaded);

                    bool coolingDown = burstCooldownTicksLeft > 0;
                    Debug.Log("Cooldown complete: " + !coolingDown);

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
            Scribe_Values.Look<bool>(ref ShouldKeepReady, "ShouldKeepReady", false, false);
        }
    }
}
