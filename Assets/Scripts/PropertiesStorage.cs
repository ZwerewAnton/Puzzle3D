using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PropertiesStorage
{
    private static string _musicPlayerPrefsKey = "music";
    private static string _soundPlayerPrefsKey = "sound";
    private static string _musicVolumeMixerProperty = "MusicVolume";
    private static string _soundVolumeMixerProperty = "SoundVolume";
    private static string _percent = "percentLevel";

    public static string GetMusicKey()
    {
        return _musicPlayerPrefsKey;
    }
    public static string GetSoundKey()
    {
        return _soundPlayerPrefsKey;
    }
    public static string GetMusicVolumeMixerParameter()
    {
        return _musicVolumeMixerProperty;
    }
    public static string GetSoundVolumeMixerParameter()
    {
        return _soundVolumeMixerProperty;
    }    
    public static string GetPercentKey()
    {
        return _percent;
    }
}
