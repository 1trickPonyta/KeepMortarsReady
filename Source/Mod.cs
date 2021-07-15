using Verse;
using HarmonyLib;

namespace KeepMortarsReady
{
	[StaticConstructorOnStartup]
	public static class Mod
    {
		const string PACKAGE_ID = "keepmortarsready.1trickPonyta";

		static Mod()
        {
			var harmony = new Harmony(PACKAGE_ID);
			harmony.PatchAll();

			Log.Message("[Keep Mortars Ready] Loaded.");
        }
	}
}
