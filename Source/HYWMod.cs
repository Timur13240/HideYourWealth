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
    
    public static List<ThingDef> GetHiddenStorageBuildings()
    {
        if (HiddenStorageBuildings.Count == 0)
        {
            try
            {
                foreach (var def in DefDatabase<ThingDef>.AllDefsListForReading)
                {
                    if (def.HasModExtension<HiddenItemStorageBuilding>())
                    {
                        HiddenStorageBuildings.Add(def);
                    }
                }
            } catch (Exception)
            {
                //I'm lazy, and don't want to deal with exceptions, since they seem to only happen at the beginning of the game :P
            }
        }
        return HiddenStorageBuildings;
    }
}