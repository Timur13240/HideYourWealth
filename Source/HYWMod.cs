using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
namespace HideYourWealth;

/// <summary>
/// The main class for the Hide Your Wealth mod.
/// </summary>
public class HywMod : Mod
{
    public static HYWSettings Settings;
    public static List<ThingDef> HiddenStorageBuildingDefs = new List<ThingDef>();
    public static List<ThingDef> DeepStorageBuildingDefs = new List<ThingDef>();
    public HywMod(ModContentPack content) : base(content)
    {
        Settings = GetSettings<HYWSettings>();
        var harmony = new Harmony("vnull.HideYourWealth");
        harmony.PatchAll();
    }
    
    public override string SettingsCategory() => "Hide Your Wealth";
    
    public override void DoSettingsWindowContents(Rect inRect)
    {
        Listing_Standard listingStandard = new Listing_Standard();
        listingStandard.Begin(inRect);
        listingStandard.CheckboxLabeled("Hide Wealth inside Deep Storage Containers", ref Settings.DeepStorageHidesContents);
        listingStandard.End();
        Settings.Write();
    }
    
    public static List<ThingDef> GetHiddenStorageBuildingDefs()
    {
        if (HiddenStorageBuildingDefs.Count == 0)
        {
            HiddenStorageBuildingDefs = DefDatabase<ThingDef>.AllDefsListForReading
                .Where(def => def.HasModExtension<HiddenItemStorageBuilding>())
                .ToList();
        }
        if (DeepStorageBuildingDefs.Count == 0)
        {
            DeepStorageBuildingDefs = DefDatabase<ThingDef>.AllDefsListForReading
                .Where(def => def.HasModExtension<DeepStorageHiddenItemStorageBuilding>())
                .ToList();
        }
        
        var output = new List<ThingDef>(HiddenStorageBuildingDefs);
        if (Settings.DeepStorageHidesContents)
        {
            output.AddRange(DeepStorageBuildingDefs);
        }
        return output;
    }
}