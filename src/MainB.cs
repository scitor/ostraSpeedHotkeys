#if !UMM
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;

namespace SpeedHotkeys
{
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    public class Main : BaseUnityPlugin
    {
        public const string pluginGuid = "space.scitor.Ostranauts.SpeedHotkeys";
        public const string pluginName = "SpeedHotkeys";
        public const string pluginVersion = "1.2.0";

        ConfigEntry<bool> requireAlt = null!;
        ConfigEntry<bool> requireCtrl = null!;

        private void Awake()
        {
            requireAlt = Config.Bind("General", "requireAlt", false, "Require ALT + Key");
            requireAlt.SettingChanged += OnSettingChanged;

            requireCtrl = Config.Bind("General", "requireCtrl", false, "Require CTRL + Key");
            requireCtrl.SettingChanged += OnSettingChanged;

            Harmony harmony = new Harmony(pluginGuid);
            harmony.PatchAll(typeof(Patch).Assembly);

            Logger.LogInfo(pluginName + " v" + pluginVersion + " Ready");
        }

        private void OnSettingChanged(object sender, EventArgs e)
        {
            Patch.requireAlt = requireAlt.Value;
            Patch.requireCtrl = requireCtrl.Value;
        }
    }
}
#endif
