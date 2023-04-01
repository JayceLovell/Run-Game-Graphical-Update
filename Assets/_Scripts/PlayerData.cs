using UnityEngine;
using static GameManager;

public class PlayerData
{

    public static float BGMVolume
    {
        get { return PlayerPrefs.GetFloat("BGMVolume", 0.5f); }
        set { PlayerPrefs.SetFloat("BGMVolume", value); }
    }   

    public static float SFXVolume
    {
        get { return PlayerPrefs.GetFloat("SFXVolume", 0.5f); }
        set { PlayerPrefs.SetFloat("SFXVolume", value); }
    }

    public static string UserName
    {
        get { return PlayerPrefs.GetString("UserName", "Runner"); }
        set { PlayerPrefs.SetString("UserName", value); }
    }    

    public static void Save()
    {
        PlayerPrefs.Save();
    }
}
