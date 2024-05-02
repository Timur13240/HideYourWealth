using RimWorld;
using Verse;

namespace NoWealthStorageZone;

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
            __result -= FindHiddenItemWealth();
        } catch (Exception)
        {
            //I'm lazy, and don't want to deal with exceptions, since they seem to only happen at the beginning of the game :P
        }
    }
    
    /// <summary>
    /// Calculates the total market value of all hidden items.
    /// </summary>
    /// <returns> The total market value of all hidden items. </returns>
    private static float FindHiddenItemWealth()
    {
        var buildingDefs = HywMod.GetHiddenStorageBuildingDefs();
        if (buildingDefs.Count == 0) return 0;
        
        var buildings = Find.CurrentMap.listerBuildings.allBuildingsColonist
            .Where(building => buildingDefs.Contains(building.def)).ToList();
        if (buildings.Count == 0) return 0;
        
        List<Thing> wealthRemovedItems = new();
        wealthRemovedItems.AddRange(FindItemsOnBuildings(buildings));
        wealthRemovedItems.AddRange(FindGenePacks(buildings));
        
        return wealthRemovedItems.Sum(item => item.MarketValue * item.stackCount);
    }

    
    /// <summary>
    /// Gets all items on top of the buildings.
    /// </summary>
    /// <returns> A list of all items on top of the buildings. </returns>
    private static List<Thing> FindItemsOnBuildings(IEnumerable<Building> buildings)
    {
        
        var itemsOnTop = buildings
            //Get all cells occupied by the buildings
            .SelectMany(GenAdj.CellsOccupiedBy)
            //Get all things at those cells
            .SelectMany(position => Find.CurrentMap.thingGrid.ThingsListAt(position))
            //Filter out things that are not items
            .Where(item => item.def.category == ThingCategory.Item)
            .ToList();
        return itemsOnTop;
    }

    /// <summary>
    /// Gets all gene packs in the gene banks of the buildings.
    /// </summary>
    /// <param name="buildings"> The buildings to search. </param>
    /// <returns></returns>
    private static IEnumerable<Genepack> FindGenePacks(IEnumerable<Building> buildings)
    {
        var genePacks = buildings
            //Filter out any buildings that are not building.HasComp<CompGenepackContainer>()
            .Where(building => building.HasComp<CompGenepackContainer>())
            //Get all things in the gene banks
            .SelectMany(building => building.TryGetComp<CompGenepackContainer>().ContainedGenepacks);
        return genePacks;
    }
}