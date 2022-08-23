using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

public static class FileUtils
{
    public static string LoadTextFile(string filename)
    {
        string raw = File.ReadAllText(Application.persistentDataPath + "/" + filename);
        return raw;
    }

    public static void SaveTextFile(string filename, string text)
    {
        File.WriteAllText(Application.persistentDataPath + "/" + filename, text);
    }

    public static string LoadJsonFromResources(string path)
    {
        string filePath = path.Replace(".json", "");
        TextAsset targetFile = Resources.Load<TextAsset>(filePath);
        return targetFile.text;
    }


    public static void SaveTextFileToResources(string filename, string text)
    {
#if UNITY_EDITOR
        File.WriteAllText(Application.dataPath + "\\Resources\\" + filename, text);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
#endif
    }

}
