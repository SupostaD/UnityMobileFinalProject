using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")] 
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Mixer")] 
    public AudioMixer audioMixer;

    [Header("Clips")] 
    public AudioClip defaultBGM;
    public List<AudioClip> sfxClips;

    private Dictionary<string, AudioClip> sfxDict = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            foreach (var clip in sfxClips)
            {
                if (clip != null)
                {
                    sfxDict[clip.name] = clip;
                }
            }

            if (defaultBGM != null)
                PlayBGM(defaultBGM);
        }
        else Destroy(gameObject);
    }
    
    // BGM
    public void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;

        if (bgmSource.clip != clip)
        {
            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }
    
    // SFX
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    public void PlaySFX(string clipName)
    {
        if (sfxDict.TryGetValue(clipName, out var clip))
            PlaySFX(clip);
    }
    
    // Volume
    public void SetMasterVolume(float sliderValue)
    {
        SetMixerVolume("MasterVolume", sliderValue);
    }

    public void SetBGMVolume(float sliderValue)
    {
        SetMixerVolume("BGMVolume", sliderValue);
    }

    public void SetSFXVolume(float sliderValue)
    {
        SetMixerVolume("SFXVolume", sliderValue);
    }

    public void MuteMaster(bool isMuted, float sliderValue)
    {
        audioMixer.SetFloat("MasterVolume", isMuted ? -80f : VolumeToDecibels(sliderValue));
    }

    public void MuteBGM(bool isMuted, float sliderValue)
    {
        audioMixer.SetFloat("BGMVolume", isMuted ? -80f : VolumeToDecibels(sliderValue));
    }

    public void MuteSFX(bool isMuted, float sliderValue)
    {
        audioMixer.SetFloat("SFXVolume", isMuted ? -80f : VolumeToDecibels(sliderValue));
    }
    
    private void SetMixerVolume(string parameter, float volume)
    {
        audioMixer.SetFloat(parameter, VolumeToDecibels(volume));
    }

    private float VolumeToDecibels(float volume)
    {
        return Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
    }
}
