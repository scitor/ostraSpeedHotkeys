#if UMM
using UnityEngine;
using UnityModManagerNet;

namespace SpeedHotkeys;

public class Settings : UnityModManager.ModSettings, IDrawable
{
    [Header("General")]
    [Draw("Require ALT + Key")]
    public bool requireAlt = false;

    [Draw("Require CTRL + Key")]
    public bool requireCtrl = false;

    [Draw("Toggle 1x when key repeats")]
    public bool toggleSame = false;

    [Draw("Enable Overspeed >16x (Experimental, probably gamebreaking)")]
    public bool lordHelmet = false;

    [Header("Key bindings (Unity KeyCode values, 0=disable)")]
    [Draw("1x KeyCode")]
    public int keyCode1x = (int)KeyCode.Alpha1;

    [Draw("2x KeyCode")]
    public int keyCode2x = (int)KeyCode.Alpha2;

    [Draw("4x KeyCode")]
    public int keyCode4x = (int)KeyCode.Alpha3;

    [Draw("8x KeyCode")]
    public int keyCode8x = (int)KeyCode.Alpha4;

    [Draw("16x KeyCode")]
    public int keyCode16x = (int)KeyCode.Alpha5;

    [Draw("32x KeyCode")]
    public int keyCode32x = (int)KeyCode.Alpha6;

    [Draw("64x KeyCode")]
    public int keyCode64x = (int)KeyCode.Alpha7;

    public override void Save(UnityModManager.ModEntry modEntry)
    {
        Save(this, modEntry);
    }

    public void OnChange()
    {
        Patch.requireAlt = requireAlt;
        Patch.requireCtrl = requireCtrl;
        Patch.toggleSame = toggleSame;
        Patch.overspeed = lordHelmet;

        Patch.keyMap[1] = (KeyCode)keyCode1x;
        Patch.keyMap[2] = (KeyCode)keyCode2x;
        Patch.keyMap[4] = (KeyCode)keyCode4x;
        Patch.keyMap[8] = (KeyCode)keyCode8x;
        Patch.keyMap[16] = (KeyCode)keyCode16x;
        Patch.keyMap[32] = (KeyCode)keyCode32x;
        Patch.keyMap[64] = (KeyCode)keyCode64x;
    }
}
#endif
