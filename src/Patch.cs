using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace SpeedHotkeys;

[HarmonyPatch]
class Patch
{
    public static Dictionary<int, KeyCode> keyMap = new() {
        { 1, KeyCode.Alpha1 },
        { 2, KeyCode.Alpha2 },
        { 4, KeyCode.Alpha3 },
        { 8, KeyCode.Alpha4 },
        { 16, KeyCode.Alpha5 },
        { 32, KeyCode.Alpha6 },
        { 64, KeyCode.Alpha7 },
    };

    public static bool requireAlt = false;
    public static bool requireCtrl = false;
    public static bool toggleSame = false;
    public static bool overspeed = false;

    static bool ModifierKeysActive()
    {
        if (requireAlt && (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))) {
            return true;
        }
        if (requireCtrl && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))) {
            return true;
        }
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CrewSim), "KeyHandler")]
    static void CrewSim_KeyHandler__Prefix()
    {
        if (requireAlt != (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))) {
            return;
        }
        if (requireCtrl != (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))) {
            return;
        }
        MethodInfo reflSetTimeScale = typeof(CrewSim).GetMethod("SetTimeScale", BindingFlags.NonPublic | BindingFlags.Static);
        foreach (KeyValuePair<int, KeyCode> entry in keyMap) {
            if (entry.Value == KeyCode.None || !Input.GetKeyDown(entry.Value) || (entry.Key > 16f && !overspeed)) {
                continue;
            }
            if ((int)Time.timeScale != entry.Key) {
                reflSetTimeScale.Invoke(null, new object[] { (float)entry.Key });
            } else if (toggleSame) {
                reflSetTimeScale.Invoke(null, new object[] { 1f });
            }
            AudioManager.am.PlayAudioEmitter("UIPauseBtnOut", false);
            return;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CrewSim), "SetTimeScale")]
    static void  CrewSim_SetTimeScale__Postfix(float fTimeScale, ref TMPro.TMP_Text ___txtRate)
    {
        if (fTimeScale < 32f || !overspeed)
            return;

        Time.timeScale = MathUtils.Clamp(fTimeScale, 32f, 64f);
        if (fTimeScale > 32f)
            ___txtRate.text = "Ludicrous\nSpeed x64";
        else
            ___txtRate.text = "Ridiculous\nSpeed x32";
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(GUIQuickBar), "OnButtonClicked")]
    static bool GUIQuickBar_OnButtonClicked__Prefix(GUIQuickActionButton qab)
    {
        return !ModifierKeysActive();
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(GUIQuickBar), "PageDown")]
    static bool GUIQuickBar_PageDown__Prefix()
    {
        return !ModifierKeysActive();
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(GUIQuickBar), "Refresh")]
    static bool GUIQuickBar_Refresh__Prefix()
    {
        return !ModifierKeysActive();
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(GUIQuickBar), "ExpandCollapse")]
    static bool GUIQuickBar_ExpandCollapse__Prefix()
    {
        return !ModifierKeysActive();
    }
}
