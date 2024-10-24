#if !UMM
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedHotkeys
{
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    public class Main : BaseUnityPlugin
    {
        public const string pluginGuid = "space.scitor.Ostranauts.SpeedHotkeys";
        public const string pluginName = "SpeedHotkeys";
        public const string pluginVersion = "1.3.0";

        ConfigEntry<bool> requireAlt = null!;
        ConfigEntry<bool> requireCtrl = null!;
        ConfigEntry<bool> toggleSame = null!;
        ConfigEntry<bool> overspeed = null!;
        Dictionary<int, KeyConfigEntry> keyConfigEntries = null!;

        private void Awake()
        {
            requireAlt = Config.Bind("General", "requireAlt", false, "Require ALT + Key");
            requireAlt.SettingChanged += OnSettingChanged;

            requireCtrl = Config.Bind("General", "requireCtrl", false, "Require CTRL + Key");
            requireCtrl.SettingChanged += OnSettingChanged;

            toggleSame = Config.Bind("General", "toggleSame", false, "Toggle 1x when key repeats");
            toggleSame.SettingChanged += OnSettingChanged;

            overspeed = Config.Bind("General", "lordHelmet", false, "Enable Overspeed >16x (Experimental, probably gamebreaking)");
            overspeed.SettingChanged += OnSettingChanged;

            keyConfigEntries = new Dictionary<int, KeyConfigEntry>() {
                { 1, new(){ Handle="keyCode1x", DefaultKeyCode=KeyCode.Alpha1 } },
                { 2, new(){ Handle="keyCode2x", DefaultKeyCode=KeyCode.Alpha2 } },
                { 4, new(){ Handle="keyCode4x", DefaultKeyCode=KeyCode.Alpha3 } },
                { 8, new(){ Handle="keyCode8x", DefaultKeyCode=KeyCode.Alpha4 } },
                { 16, new(){ Handle="keyCode16x", DefaultKeyCode=KeyCode.Alpha5 } },
                { 32, new(){ Handle="keyCode32x", DefaultKeyCode=KeyCode.Alpha6 } },
                { 64, new(){ Handle="keyCode64x", DefaultKeyCode=KeyCode.Alpha7 } },
            };
            foreach (KeyValuePair<int, KeyConfigEntry> entry in keyConfigEntries) {
                entry.Value.ConfigEntry = Config.Bind("KeyBindings", entry.Value.Handle, (int)entry.Value.DefaultKeyCode, "Unity KeyCode values, 0=disabled");
                entry.Value.ConfigEntry.SettingChanged += OnSettingChanged;
            }

            OnSettingChanged(null!, null!);
            Harmony harmony = new Harmony(pluginGuid);
            harmony.PatchAll(typeof(Patch).Assembly);

            Logger.LogInfo(pluginName + " v" + pluginVersion + " Ready");
        }

        private void OnSettingChanged(object sender, EventArgs e)
        {
            Patch.requireAlt = requireAlt.Value;
            Patch.requireCtrl = requireCtrl.Value;
            Patch.toggleSame = toggleSame.Value;
            Patch.overspeed = overspeed.Value;

            foreach (KeyValuePair<int, KeyConfigEntry> entry in keyConfigEntries) {
                if (entry.Value.ConfigEntry != null) {
                    Patch.keyMap[entry.Key] = (KeyCode)entry.Value.ConfigEntry.Value;
                }
            }
        }
    }

    internal class KeyConfigEntry
    {
        public ConfigEntry<int>? ConfigEntry { get; set; }
        public KeyCode DefaultKeyCode { get; set; } = KeyCode.None;
        public string Handle { get; set; } = string.Empty;
    }
}
#endif
