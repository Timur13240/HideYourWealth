using HarmonyLib;
using Verse;
namespace NoWealthStorageZone;

/// <summary>
/// The main class for the Hide Your Wealth mod.
/// </summary>
public class HywMod : Mod
{
    public HywMod(ModContentPack content) : base(content)
    {
        var harmony = new Harmony("vnull.HideYourWealth");
        harmony.PatchAll();
    }
}