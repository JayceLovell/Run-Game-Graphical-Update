using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    //To handle multiple sounds
    public enum SoundFX
    {
        EvilLaugh,
        LightLightGoesOut,
        FlickrLight,
        SeeThroughMode,
        ExitSeeThroughMode
    }
    public enum BgSound
    {
        Title,
        GamePlay        
    }
    public static void MasterVolumeChanged(float value)
    {
        try
        {
            GameObject.Find("BgSound").GetComponent<AudioSource>().volume = value;
        }
        catch
        {
            Debug.LogError("BG hasn't started yet");
        }
    }
    public static void StartBackground(BgSound bgSound)
    {
        GameObject BackgroundSound = new GameObject("BgSound");
        AudioSource audioSource = BackgroundSound.AddComponent<AudioSource>();
        audioSource.clip=GetBGAudio(bgSound);
        audioSource.loop = true;
        audioSource.volume = GameManager.Instance.BGMVolume;
        audioSource.Play();
        BackgroundSound.AddComponent<SoundBGVolume>();
    }
    /// <summary>
    /// Grabs Enum audio and plays once
    /// </summary>
    /// <param name="sound"></param>
    public static void PlaySound(SoundFX sound)
    {
        GameObject soundGameObject = new GameObject("SoundFX");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(GetAudio(sound));
        audioSource.volume = GameManager.Instance.SFXVolume;
        soundGameObject.AddComponent<SoundFXLife>().SoundLength = GetAudio(sound).length;
    }
    private static AudioClip GetBGAudio(BgSound sound)
    {
        foreach (SoundAssets.BgSounds Bgsound in SoundAssets.i.soundBGArray)
        {
            if (Bgsound.sound == sound)
            {
                return Bgsound.audioClip;
            }
        }
        Debug.LogError("Sound" + sound + "not found");
        return null;
    }
    /// <summary>
    /// Gets audio clip from sound assets
    /// </summary>
    /// <param name="sound"></param>
    /// <returns></returns>
    private static AudioClip GetAudio(SoundFX sound)
    {
        foreach (SoundAssets.SoundFXClip soundFxClip in SoundAssets.i.soundFXClipArray)
        {
            if (soundFxClip.sound == sound)
            {
                return soundFxClip.audioClip;
            }
        }
        Debug.LogError("Sound" + sound + "not found");
        return null;
    }
}
