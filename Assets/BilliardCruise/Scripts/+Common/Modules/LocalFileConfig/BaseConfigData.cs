using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public enum Platform
{
    GooglePlay,
    Apple,
    PC,
}


[Serializable]
public class BaseConfigData
{
    // Version of actual application
    public string version = "0.0.0";
    public Dictionary<Platform, string> updateLinks = new Dictionary<Platform, string>();

    // Major Version of Actual Application
    // NOTE: If local and global major version differs -> need to update application
    [JsonIgnore]
    public int MajorVersion => Version.Parse(version).Major;




    [JsonIgnore]
    public Version Version => Version.Parse(version);

    // Minor Version of Actual Application
    // NOTE: If local and global minor version differs -> need to offer update application (optional)
    [JsonIgnore]
    public int MinorVersion => Version.Parse(version).Minor;

    public static ConfigData CreateDefault()
    {
        ConfigData result = new ConfigData();
        result.version = Application.version;
        return result;
    }
}
