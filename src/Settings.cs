#if UMM
using UnityModManagerNet;

namespace SpeedHotkeys;

public class Settings : UnityModManager.ModSettings, IDrawable
{
    [Draw("Require ALT + Key")]
    public bool requireAlt = false;

    [Draw("Require CTRL + Key")]
    public bool requireCtrl = false;

    public override void Save(UnityModManager.ModEntry modEntry)
    {
        Save(this, modEntry);
    }

    public void OnChange()
    {
        Patch.requireAlt = requireAlt;
        Patch.requireCtrl = requireCtrl;
    }
}
#endif
