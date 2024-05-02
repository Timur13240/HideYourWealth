using RimWorld;
using Verse;

namespace HideYourWealth;

public static class WealthCalculator
{
        /// <summary>
    /// Calculates the total market value of all hidden items.
    /// </summary>
    /// <returns> The total market value of all hidden items. </returns>
    public static float HiddenItemWealth()
    {
        //Get all hidden storage building defs
        var buildingDefs = HywMod.GetHiddenStorageBuildingDefs();
        if (buildingDefs.Count == 0) return 0;
        
        //Convert the building defs to a list of buildings
        var buildings = Find.CurrentMap.listerBuildings.allBuildingsColonist
            .Where(building => buildingDefs.Contains(building.def)).ToList();
        if (buildings.Count == 0) return 0;
        
        //Get items on top of buildings and gene packs in gene banks
        List<Thing> wealthRemovedItems = new();
        wealthRemovedItems.AddRange(FindItemsOnBuildings(buildings));
        wealthRemovedItems.AddRange(FindGenePacks(buildings));
        return wealthRemovedItems.Sum(item => item.MarketValue * item.stackCount);
    }

    
    /// <summary>
    /// Gets all items on top of the buildings.
    /// </summary>
    /// <returns> A list of all items on top of the buildings. </returns>
    private static IEnumerable<Thing> FindItemsOnBuildings(IEnumerable<Building> buildings)
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
    /// Gets all gene packs in any gene bank buildings.
    /// </summary>
    /// <param name="buildings"> The buildings to search. </param>
    /// <returns> A list of all gene packs in any gene bank buildings. </returns>
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