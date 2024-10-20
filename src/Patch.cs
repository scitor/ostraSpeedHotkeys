﻿using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace SpeedHotkeys;

[HarmonyPatch]
class Patch
{
    static readonly Dictionary<KeyCode, float> keyMap = new() {
        { KeyCode.Alpha1, 1f },
        { KeyCode.Alpha2, 2f },
        { KeyCode.Alpha3, 4f },
        { KeyCode.Alpha4, 8f },
        { KeyCode.Alpha5, 16f }
    };

    static bool ModifierKeysActive()
    {
        if (Main.Settings.requireAlt && (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))) {
            return true;
        }
        if (Main.Settings.requireCtrl && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))) {
            return true;
        }
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CrewSim), "KeyHandler")]
    static bool CrewSim_KeyHandler__Prefix(CrewSim __instance)
    {
        if (Main.Settings.requireAlt && !Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightAlt)) {
            return true;
        }
        if (Main.Settings.requireCtrl && !Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl)) {
            return true;
        }
        MethodInfo refSetTimeScale = typeof(CrewSim).GetMethod("SetTimeScale", BindingFlags.NonPublic | BindingFlags.Static);
        foreach (KeyValuePair<KeyCode, float> entry in keyMap) {
            if (Input.GetKeyDown(entry.Key)) {
                refSetTimeScale.Invoke(null, new object[] { entry.Value });
                return false;
            }
        }
        return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(GUIQuickBar), "OnButtonClicked")]
    static bool GUIQuickBar_OnButtonClicked__Prefix(GUIQuickActionButton qab)
    {
        // prevent quickactions with active modifier keys
        return !ModifierKeysActive();
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(GUIQuickBar), "PageDown")]
    static bool GUIQuickBar_PageDown__Prefix()
    {
        // prevent quickactions with active modifier keys
        return !ModifierKeysActive();
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(GUIQuickBar), "Refresh")]
    static bool GUIQuickBar_Refresh__Prefix()
    {
        // prevent quickactions with active modifier keys
        return !ModifierKeysActive();
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(GUIQuickBar), "ExpandCollapse")]
    static bool GUIQuickBar_ExpandCollapse__Prefix()
    {
        // prevent quickactions with active modifier keys
        return !ModifierKeysActive();
    }
}
