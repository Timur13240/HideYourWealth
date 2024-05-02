using Verse;

namespace NoWealthStorageZone;

public class HYWSettings : ModSettings
{
    public bool DeepStorageHidesContents = true;

    public override void ExposeData()
    {
        Scribe_Values.Look(ref DeepStorageHidesContents, "DeepStorageHidesContents", true);
        base.ExposeData();
    }
}