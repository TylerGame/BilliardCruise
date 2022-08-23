using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

[Serializable]
public class LocalizationConfig  : ModuleConfig
{
    public Dictionary<SystemLanguage, string> languages = new Dictionary<SystemLanguage, string>()
    {
        { SystemLanguage.Russian, "ru-RU" },
        { SystemLanguage.English, "en-US" }
    };
}