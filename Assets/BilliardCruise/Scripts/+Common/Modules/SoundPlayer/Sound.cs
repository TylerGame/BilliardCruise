using UnityEngine;
using System.Collections.Generic;
using System;
using UniRx;

public class LoopPlayer
{
    public AudioSource auso;
    public float targetVolume;
    public float time;

    public void Update()
    {
        if (time > 0.1f)
        {
            targetVolume = 0f;
        } else { 
            time += Time.deltaTime;
        }
        auso.volume = Mathf.Lerp(auso.volume, targetVolume, 10 * Time.deltaTime);
    }

    public void Play()
    {
        time = 0;
        targetVolume = 1f;
    }

    public void Poke()
    {
        time = 0;
        targetVolume = Sound.Data.volume;
    }
}

public class SoundSettingsGameObject : DataBehaviour
{
    public void Start()
    {
        Subscribe<ChangeSoundVolumeEvent>(_ =>
        {
            GetComponent<AudioSource>().volume = Sound.Data.volume;
        });
    }

    public void Update()
    {
        foreach(KeyValuePair<string, LoopPlayer> kv in Sound.loops)
        {
            kv.Value.Update();
        }
    }
}

public class SoundSettings : ModuleData
{
    public float volume = 1f;
}

public class Sound : GameModuleProfiled<SoundSettings>
{
    public static AudioSource soundSource;

    public const string SoundChangeVolume = nameof(SoundChangeVolume);


    private static Dictionary<string, float> throttle = new Dictionary<string, float>();

    public static Dictionary<string, LoopPlayer> loops = new Dictionary<string, LoopPlayer>();

    public static void ForcePlayLoop(string sound)
    {
        LoopPlayer lp = null;
        loops.TryGetValue(sound, out lp);
        if(lp == null)
        {
            GameObject go = Core.CreateGameObject();
            lp = new LoopPlayer();
            AudioSource auso = go.AddComponent<AudioSource>();
            lp.auso = auso;
            lp.auso.volume = 0f;
            lp.auso.clip = AssetDB.LoadSound("Sounds/"+sound);
            lp.targetVolume = 1f;
            loops.Add(sound, lp);
            lp.auso.loop = true;
            lp.auso.Play();
        }
        lp.Play();
    }

    public static void Play(List<string> sounds, float multiply = 1f)
    {
        if (sounds == null || sounds.Count == 0) return;
        Play(RandomUtils.RandomValue(sounds), multiply);
    }

    public static void Play(string prefix, int min, int max, float multiply = 1f)
    {
        Play(prefix+UnityEngine.Random.Range(min,max+1), multiply);
    }

    public static void Play(AudioClip clip, float multiply = 1f)
    {
        if (clip == null) return;
        soundSource.PlayOneShot(clip, (LocalProfile.Data != null ? Sound.Data.volume : 1) * multiply);
    }


    public static void Play(string sound, float multiply = 1f)
    {
        if (soundSource != null)
        {
            float time;
            if (throttle.TryGetValue(sound, out time))
            {
                if (Time.time - time < 0.1f)
                {
                    return;
                }
            }
            throttle[sound] = Time.time;
            soundSource.PlayOneShot(AssetDB.LoadSound("Sounds\\"+sound), (LocalProfile.Data != null ? Sound.Data.volume : 1) * multiply);
        }
    }

    public static void SetVolume(float volume)
    {
        Sound.Data.volume = volume;
        soundSource.volume = Sound.Data.volume;
    }

    public override IObservable<bool> Initialize()
    {
        GameObject go = Core.CreateGameObject(this);
        soundSource = go.AddComponent<AudioSource>();
        go.AddComponent<SoundSettingsGameObject>();

        Core.OnInitialized(typeof(LocalProfile)).Subscribe(initialized =>
        {
            if (initialized)
            {
                soundSource.volume = Sound.Data.volume;
            }
        });

        return Observable.Return(true);
    }

    public override IEnumerable<Type> GetDependencies()
    {
        return new List<Type>();
    }
}

