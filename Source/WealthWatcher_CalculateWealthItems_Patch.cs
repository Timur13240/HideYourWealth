using RimWorld;
using Verse;

namespace HideYourWealth;

[HarmonyLib.HarmonyPatch(typeof(WealthWatcher), "CalculateWealthItems")]
// ReSharper disable once InconsistentNaming
public static class WealthWatcher_CalculateWealthItems_Patch
{
    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Subtracts the market value of items on top of hidden storage buildings from the total wealth of the map.
    /// </summary>
    /// <param name="__result"> The original wealth of items </param>
    public static void Postfix(ref float __result)
    {
        try
        {
            __result -= WealthCalculator.HiddenItemWealth();
        }
        catch (NullReferenceException e)
        {
            // Ignore null reference exceptions due to the wealth watcher being null. It always happens the first time the game is loaded.
        }
        catch (Exception e)
        {
            Log.Error("Could not calculate hidden item wealth: " + e);
        }
    }
    

}