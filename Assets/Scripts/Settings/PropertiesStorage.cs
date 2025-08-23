namespace Settings
{
    public static class PropertiesStorage
    {
        private const string MusicPlayerPrefsKey = "music";
        private const string SoundPlayerPrefsKey = "sound";
        private const string MusicVolumeMixerProperty = "MusicVolume";
        private const string SoundVolumeMixerProperty = "SoundVolume";
        private const string Percent = "percentLevel";

        public static string GetMusicKey()
        {
            return MusicPlayerPrefsKey;
        }
        public static string GetSoundKey()
        {
            return SoundPlayerPrefsKey;
        }
        public static string GetMusicVolumeMixerParameter()
        {
            return MusicVolumeMixerProperty;
        }
        public static string GetSoundVolumeMixerParameter()
        {
            return SoundVolumeMixerProperty;
        }    
        public static string GetPercentKey()
        {
            return Percent;
        }
    }
}