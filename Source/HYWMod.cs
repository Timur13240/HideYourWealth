using HarmonyLib;
using RimWorld;
using Verse;
namespace NoWealthStorageZone;

/// <summary>
/// The main class for the Hide Your Wealth mod.
/// </summary>
public class HywMod : Mod
{
    private static List<ThingDef> HiddenStorageBuildings {get; set;} = new();
    public HywMod(ModContentPack content) : base(content)
    {
        var harmony = new Harmony("vnull.HideYourWealth");
        harmony.PatchAll();
    }
    
    public static List<ThingDef> GetCachedHiddenStorageBuildingDefs()
    {
            if (HiddenStorageBuildings.Count != 0) return HiddenStorageBuildings;
            HiddenStorageBuildings = DefDatabase<ThingDef>.AllDefsListForReading
                .Where(def => def.HasModExtension<HiddenItemStorageBuilding>())
                .ToList();
            return HiddenStorageBuildings;
    }
}