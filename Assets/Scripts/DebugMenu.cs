using System;
using System.Collections.Generic;
using Configs;
using UnityEngine;

public class DebugMenu : MonoBehaviour
{
    private static List<ConfigField> ConfigFields => new();

    /// <summary>
    /// Add a new value to the list with all it's properties.
    /// </summary>
    /// <param name="valuePrefsName"></param>
    /// <param name="inputFieldGoName"></param>
    /// <param name="initialValue"></param>
    /// <param name="changeAction"></param>
    private void AddConfigField(string valuePrefsName, string inputFieldGoName, float initialValue, Action<float> changeAction)
    {
        var configField = new ConfigField(valuePrefsName, inputFieldGoName, initialValue, changeAction);

        ConfigFields.Add(configField);
        configField.Initialize();
    }

    /// <summary>
    /// Standard Unity Start function.
    /// Creates all the values needed to change them.
    /// </summary>
    private void Start()
    {
        AddConfigField("minZoom2D", "MinZoom2D", Config.Config2D.MinZoom, value => Config.Config2D.MinZoom = value);
        AddConfigField("maxZoom2D", "MaxZoom2D", Config.Config2D.MaxZoom, value => Config.Config2D.MaxZoom = value);
        AddConfigField("speedDamp2D", "ZoomDampening2D", Config.Config2D.SpeedDampening, value => Config.Config2D.SpeedDampening = value);
       
        AddConfigField("distance3D", "Distance3D", Config.Config3D.Distance, value => Config.Config3D.Distance = value);
        AddConfigField("minDistance3D", "MinDistance3D", Config.Config3D.MinDistance, value => Config.Config3D.MinDistance = value);
        AddConfigField("maxDistance3D", "MaxDistance3D", Config.Config3D.MaxDistance, value => Config.Config3D.MaxDistance = value);
        AddConfigField("xSpeed3D", "xSpeed3D", Config.Config3D.XSpeed, value => Config.Config3D.XSpeed = value);
        AddConfigField("ySpeed3D", "ySpeed3D", Config.Config3D.YSpeed, value => Config.Config3D.YSpeed = value);
        AddConfigField("speedRotate3D", "SpeedRotate3D", Config.Config3D.SpeedRotate, value => Config.Config3D.SpeedRotate = value);
        AddConfigField("speedDampening3D", "SpeedDampening3D", Config.Config3D.SpeedDampening, value => Config.Config3D.SpeedDampening = value);
        AddConfigField("speedZoom3D", "SpeedZoom3D", Config.Config3D.SpeedZoom, value => Config.Config3D.SpeedZoom = value);
        AddConfigField("speedPan3D", "SpeedPan3D", Config.Config3D.SpeedPan, value => Config.Config3D.SpeedPan = value);
        
        AddConfigField("speedAR", "SpeedAR", Config.ConfigAR.Speed, value => Config.ConfigAR.Speed = value);
        AddConfigField("speedRotateAR", "SpeedRotateAR", Config.ConfigAR.SpeedRotate, value => Config.ConfigAR.SpeedRotate = value);
        AddConfigField("speedDampeningAR", "SpeedDampeningAR", Config.ConfigAR.SpeedDampening, value => Config.ConfigAR.SpeedDampening = value);
    }
}