using RimWorld;
using Verse;

namespace NoWealthStorageZone;

[HarmonyLib.HarmonyPatch(typeof(WealthWatcher), "CalculateWealthItems")]
// ReSharper disable once InconsistentNaming
public static class WealthWatcher_CalculateWealthItems_Patch
{
    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Subtracts the market value of items in "HiddenShelf" objects from the total wealth of the map.
    /// </summary>
    /// <param name="__result"> The total wealth of the map. </param>
    public static void Postfix(ref float __result)
    {
        try
        {
            __result -= CalculateHiddenWealth();
        } catch (Exception)
        {
            //I'm lazy, and don't want to deal with exceptions :P
        }
    }
    
    /// <summary>
    /// Calculates the total market value of items in "HiddenShelf" objects on the current map.
    /// </summary>
    /// <returns>The total market value of items in "HiddenShelf" objects.</returns>
    private static float CalculateHiddenWealth()
    {
        var hiddenWealthItems = 
                    //Get all things on the map
            Find.CurrentMap.listerThings.AllThings
                    //Filter out things that are not HiddenShelf
            .Where(x => x.def == ThingDef.Named("HiddenShelf"))
                    //Get all cells occupied by the HiddenShelf
            .SelectMany(GenAdj.CellsOccupiedBy)
                    //Get all things at those cells
            .SelectMany(position => Find.CurrentMap.thingGrid.ThingsListAt(position))
                    //Filter out things that are not items
            .Where(item => item.def.category == ThingCategory.Item);
        //Return the sum of the market value of all items in HiddenShelf objects
        return hiddenWealthItems.Sum(item => item.MarketValue * item.stackCount);
    }
}