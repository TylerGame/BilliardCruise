using UnityEngine;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizedText : DataBehaviour
{
    public string key;
    public string prefix = "";
    public string suffix = "";
    TextMeshProUGUI tmp;

    protected void Awake()
    {
        Subscribe<SwitchLanguageEvent>(lang => {
            UpdateLocalizedText();
        });
    }


    public void UpdateLocalizedText()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        tmp.text = prefix + Localization.Translate(key) + suffix;
    }

    protected void OnEnable()
    {
        UpdateLocalizedText();
    }

    public void OnValidate()
    {
        Localization.RefreshLanguages();
        UpdateLocalizedText();
    }

}
#if UNITY_EDITOR

[CustomEditor(typeof(LocalizedText))]
public class LocalizedTextEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
#endif