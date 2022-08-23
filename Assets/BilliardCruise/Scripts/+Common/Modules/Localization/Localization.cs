using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using System;
using TMPro;
using Newtonsoft.Json;
using UniRx;

[Serializable]
public class LocalizationSettings : ModuleData
{
    public string currentLanguage;
}

[ExecuteInEditMode]
public class Localization : GameModuleConfigured<LocalizationSettings, LocalizationConfig>
{
    protected static string language;
    static TextAsset[] langFiles;
    static Dictionary<string, TextAsset[]> langDictionary = new Dictionary<string, TextAsset[]>();

    public override IEnumerable<Type> GetDependencies()
    {
        return new List<Type>() { typeof(LocalConfig), typeof(LocalProfile) };
    }

    public string this[string key] => Translate(key);

    public override IObservable<bool> Initialize()
    {
        ResetDataLink();
        RefreshLanguages();
        return Observable.Return(true);
    }

    [SerializeField]
    public static string currentLanguage
    {
        get
        {
            return language;
        }
        set
        {
            language = value;
            Data.currentLanguage = value;
            Core.Rx.Publish(new SwitchLanguageEvent(language));
        }
    }


    public List<string> languages => new List<string>(translations.Keys);

    [HideInInspector]
    public static string lastUpdate;

    public static string languagesDesc;

    [HideInInspector]
    public static int bundles;

    public static Dictionary<string, Dictionary<string, string>> translations = new Dictionary<string, Dictionary<string, string>>();


    void OnValidate()
    {
        language = currentLanguage;
        RefreshLanguages();
    }

    public static string Translate(string key, bool restrict = false)
    {
        string lang = "en-US";

        if (string.IsNullOrEmpty(language) == false)
        {
            lang = language;
        }

        return TranslateFromLanguage(key, lang, restrict);
    }

    public static string TranslateFromLanguage(string key, string strictLanguage, bool restrict = false)
    {
        Dictionary<string, string> translation;

        string lang = "en-US";

        if (string.IsNullOrEmpty(strictLanguage) == false)
        {
            lang = strictLanguage;
        }

        if (Localization.translations.TryGetValue(lang, out translation))
        {
            string result;
            if (translation != null && translation.TryGetValue(key.ToLower(), out result))
            {
                return result;
            }
            else
            {
                return restrict ? "" : key;
            }
        }
        else
        {
            //Debug.LogError("Can't find translations file for " + currentLanguage);
        }
        return key;
    }

    #if UNITY_EDITOR

    public static void AddTranslation(string key, string language, string text)
    {
        if(translations == null || !translations.ContainsKey(language))
        {
            RefreshLanguages();
        }
        if(translations.ContainsKey(language))
        {
            translations[language][key] = text;
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings
            {
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                TypeNameHandling = TypeNameHandling.Auto
            };
            string json = JsonConvert.SerializeObject(translations[language], serializerSettings);
            FileUtils.SaveTextFileToResources("Localization/"+language+".json", json);
            AssetDatabase.SaveAssets();
        }
        else
        {
            Log.Error("No language: " + language);
        }
    }

    #endif

    public static string Translate(string key, Dictionary<string,string> values)
    {
        string result = Translate(key);
        foreach (KeyValuePair<string,string> kv in values)
        {
            result = result.Replace("{" + kv.Key + "}", kv.Value);
        }
        return result;
    }
    public static string Translate(string key, params string[] values)
    {
        string result = Translate(key);
        for (int i = 0; i < values.Length; i++)
        {
            result = result.Replace("{" + (i + 1) + "}", values[i]);
        }
        return result;
    }

    public static void TryToSetSystemLanguage()
    {

        if (Config != null && Config.languages.ContainsKey(Application.systemLanguage))
        {
            Data.currentLanguage = Config.languages[Application.systemLanguage];
        }
        else if (Config != null)
        {
            Data.currentLanguage = Config.languages[SystemLanguage.English];
        }
        language = Data.currentLanguage;

    }

    public static void RefreshLanguages()
    {
        if ((new LocalizationConfig()).languages.ContainsKey(Application.systemLanguage))
        {
            language = (new LocalizationConfig()).languages[Application.systemLanguage];
        }

        if (LocalConfig.Data == null && LocalProfile.Data == null) {
            TryToSetSystemLanguage();
        } else 
        { 
            if (!string.IsNullOrEmpty(Data.currentLanguage)) {
                currentLanguage = Data.currentLanguage;
            } else
            {
                TryToSetSystemLanguage();
            }
        }
            
        JsonSerializerSettings serializerSettings = new JsonSerializerSettings
        {
            ObjectCreationHandling = ObjectCreationHandling.Replace,
            TypeNameHandling = TypeNameHandling.Auto
        };

        langFiles = Resources.LoadAll<TextAsset>("Localization");
        bundles = 0;
        List<string> langs = new List<string>();
        foreach (TextAsset lang in langFiles)
        {
            Dictionary<string, string> fromFile = (Dictionary<string, string>)JsonConvert.DeserializeObject(lang.text, typeof(Dictionary<string, string>), serializerSettings);
            int cur = 0;
            translations[lang.name] = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> pair in fromFile)
            {
                cur++;
                translations[lang.name].Add(pair.Key.ToLower(), pair.Value);
            }
            bundles = Math.Max(bundles, cur);
        }
        lastUpdate = "" + DateTime.Now;
        languagesDesc = String.Join(", ", langs.ToArray());

            
    }

}
#if UNITY_EDITOR
[CustomEditor(typeof(Localization))]
public class LocalizationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //EditorGUILayout.HelpBox("Here is languages", MessageType.None);
        base.OnInspectorGUI();
        Localization.RefreshLanguages();

        EditorGUILayout.LabelField("Languages", Localization.languagesDesc);

        EditorGUILayout.LabelField("Last Update", Localization.lastUpdate);
        EditorGUILayout.LabelField("Bundles in each language", "" + Localization.bundles);
    }
}
#endif

