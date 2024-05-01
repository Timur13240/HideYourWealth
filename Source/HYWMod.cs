using HarmonyLib;
using Verse;
namespace NoWealthStorageZone;

public class HywMod : Mod
{
    
    public HywMod(ModContentPack content) : base(content)
    {
        var harmony = new Harmony("vnull.HideYourWealth");
        harmony.PatchAll();
    }
}