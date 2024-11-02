using System;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantBaseQualifier
namespace control_goo;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class ControlGoo : BaseUnityPlugin
{
    private static ManualLogSource _logger;
    private static Harmony _harmony;

    private static ConfigEntry<bool> Enable { get; set; }
    private static ConfigEntry<float> GooSpeed { get; set; }

    private static readonly Dictionary<DEN_DeathFloor, float> _floors = new Dictionary<DEN_DeathFloor, float>();

    private void Awake()
    {
        // Plugin startup logic
        _logger = base.Logger;

        Enable = Config.Bind("General", "Enable", true, "Enable mod");
        GooSpeed = Config.Bind("General", "GooSpeed", 0.0f,
            new ConfigDescription("Speed of goo", new AcceptableValueRange<float>(0.0f, 100.0f)));

        Config.Save();

        Enable.SettingChanged += OnEnableSettingChanged;

        _harmony = Harmony.CreateAndPatchAll(typeof(ControlGoo), MyPluginInfo.PLUGIN_GUID);
        _logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID}-v{MyPluginInfo.PLUGIN_VERSION} is loaded!");
    }

    private void OnDestroy()
    {
        _floors.Clear();
        _harmony.UnpatchSelf();
        _logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID}-v{MyPluginInfo.PLUGIN_VERSION} was unloaded!");
    }

    private static void OnEnableSettingChanged(object sender, EventArgs e)
    {
        if (Enable is { Value: false })
        {
            reset_floors_values();
        }
    }

    private static void reset_floors_values()
    {
        foreach (var floor in _floors)
        {
            floor.Key.speed = floor.Value;
        }
    }


    [HarmonyPatch(typeof(DEN_DeathFloor), "Start")]
    [HarmonyPostfix]
    private static void Patch_DEN_DeathFloor_Start(DEN_DeathFloor __instance)
    {
        _floors.Add(__instance, __instance.speed);
    }

    [HarmonyPatch(typeof(GameEntity), "OnDestroy")]
    [HarmonyPostfix]
    private static void Patch_DEN_DeathFloor_OnDestroy(GameEntity __instance)
    {
        var den_DeathFloor = (DEN_DeathFloor)__instance;
        if (den_DeathFloor != null)
        {
            _floors.Remove(den_DeathFloor);
        }
    }

    [HarmonyPatch(typeof(DEN_DeathFloor), "Update")]
    [HarmonyPostfix]
    private static void Patch_DEN_DeathFloor_Update(DEN_DeathFloor __instance)
    {
        if (Enable is not { Value: true }) return;

        foreach (var floor in _floors)
        {
            floor.Key.speed = GooSpeed.Value;
        }
    }
}