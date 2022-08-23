using UnityEngine;
using UniRx;
using System;
using System.Collections.Generic;

public class MusicSettings : ModuleData
{
    public float volume = 1f;
}

public class Music : GameModuleProfiled<MusicSettings>
{
    public static AudioSource musicSource;

    static string startMusic;

    static GameObject go;

    public static void SetVolume(float volume)
    {
        Music.Data.volume = volume;
        musicSource.volume = Music.Data.volume;
    }
        
    public static void Play(string music, bool loop = true)
    {
        if (musicSource != null)
        {
            AudioClip ac = AssetDB.LoadSound("Music\\"+music);
            if (musicSource.clip == ac) return;

            musicSource.Stop();
            musicSource.loop = loop;
            musicSource.clip = ac;
            musicSource.Play();
            Log.Message("Playing: " + music);
        } else
        {
            startMusic = music;
        }
    }
    public static void Stop() { 
        musicSource.Stop();
    }

    public override IObservable<bool> Initialize()
    {
        go = Core.CreateGameObject(this);
        musicSource = go.AddComponent<AudioSource>();
        if(startMusic != null)
        {
            Play(startMusic);
        }

        Core.OnInitialized(typeof(LocalProfile)).Subscribe(initialized =>
        {
            if (initialized)
            {
                musicSource.volume = Music.Data.volume;
            }
        });

        return Observable.Return(true);
    }

    public override IEnumerable<Type> GetDependencies()
    {
        return new List<Type>();
    }
}

