using UnityEngine;
using System.Collections.Generic;
using System;
using UniRx;

public class AssetDB : IModule
{
    private static Dictionary<string, AudioClip> sounds = new Dictionary<string, AudioClip>();
    private static Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();
    private static Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
    private static Dictionary<string, Texture> textures = new Dictionary<string, Texture>();
    private static Dictionary<string, Material> materials = new Dictionary<string, Material>();

    public IObservable<bool> Initialize()
    {
        return Observable.Return(true);
    }

    public IEnumerable<Type> GetDependencies()
    {
        return new List<Type>();
    }

    public static AudioClip LoadSound(string sound)
    {
        if (sounds.ContainsKey(sound))
        {
            return sounds[sound];
        }

        AudioClip ac = (AudioClip)Resources.Load( sound, typeof(AudioClip));

        if (ac == null)
        {
            Log.Error("Can't load sound: " +  sound);
        }

        sounds[sound] = ac;
        return ac;
    }

    public static GameObject LoadPrefab(string name)
    {
        if (prefabs.ContainsKey(name))
        {
            return prefabs[name];
        }

        GameObject ac = (GameObject)Resources.Load(name, typeof(GameObject));

        if (ac == null)
        {
            Log.Error("Can't load prefab: " + name);
        }

        prefabs[name] = ac;
        return ac;
    }

    public static Material LoadMaterial(string name)
    {
        if (materials.ContainsKey(name))
        {
            return materials[name];
        }

        Material ac = (Material)Resources.Load(name, typeof(Material));

        if (ac == null)
        {
            Log.Error("Can't load material: " + name);
        }

        materials[name] = ac;
        return ac;
    }


    public static List<GameObject> LoadPrefabs<T>(string path) where T : MonoBehaviour
    {
        List<GameObject> result = new List<GameObject>();
        foreach(T t in Resources.LoadAll<T>(path))
        {
            result.Add(t.gameObject);
        }
        return result;
    }

    public static Sprite LoadSprite(string path)
    {
        if (sprites.ContainsKey(path))
        {
            return sprites[path];
        }

        Sprite ac = (Sprite)Resources.Load(path, typeof(Sprite));

        if (ac == null)
        {
            Log.Error("Can't load sprite: " + path);
            return null;
        }

        sprites[path] = ac;
        return ac;
    }

    public static Texture LoadTexture(string path)
    {
        if (textures.ContainsKey(path))
        {
            return textures[path];
        }

        Texture ac = (Texture)Resources.Load(path, typeof(Texture));

        if (ac == null)
        {
            Log.Error("Can't load texture: " + path);
        }

        textures[path] = ac;
        return ac;
    }

}

