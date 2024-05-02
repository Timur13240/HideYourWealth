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
    /// <summary>
    /// The settings for the mod.
    /// </summary>
    public static HYWSettings? Settings;
    
    /// <summary>
    /// A cache of all hidden storage building defs.
    /// </summary>
    private static List<ThingDef> _hiddenStorageBuildingDefs = new List<ThingDef>();
    
    /// <summary>
    /// A cache of all deep storage building defs.
    /// </summary>
    private static List<ThingDef> _deepStorageBuildingDefs = new List<ThingDef>();
    
    /// <summary>
    /// Default constructor for the mod.
    /// Ran on game start.
    /// Sets up the mod settings and patches the game.
    /// </summary>
    /// <param name="content"> The content pack to load the mod from. </param>
    public HywMod(ModContentPack content) : base(content)
    {
        Settings = GetSettings<HYWSettings>();
        var harmony = new Harmony("vnull.HideYourWealth");
        harmony.PatchAll();
    }
    
    public override string SettingsCategory() => "Hide Your Wealth";
    
    /// <summary>
    /// Draws the settings window for the mod.
    /// </summary>
    /// <param name="inRect"> The rectangle to draw the settings window in. </param>
    public override void DoSettingsWindowContents(Rect inRect)
    {
        Listing_Standard listingStandard = new Listing_Standard();
        listingStandard.Begin(inRect);
        listingStandard.CheckboxLabeled("Hide Wealth inside Deep Storage Containers", ref Settings.DeepStorageHidesContents);
        listingStandard.End();
        Settings.Write();
    }
    
    /// <summary>
    /// Gets all hidden storage building defs.
    /// </summary>
    /// <returns> A list of all hidden storage building defs. </returns>
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