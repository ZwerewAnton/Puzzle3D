using Settings;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Music
{
    [RequireComponent(typeof(AudioSource))]
    public class SfxPlayer : MonoBehaviour
    {
        private const float AudioOffValue = -80f;
        private const float AudioOnValue = -20f;
        
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private AudioClip tapToPlayClip;
        [SerializeField] private AudioClip playClip;
        [SerializeField] private AudioClip menuButtonClip;
        [SerializeField] private AudioClip settingButtonClip;
        
        private SettingsService _settingsService;
        private AudioSource _audioSource;

        [Inject]
        private void Construct(SettingsService settingsService)
        {
            _settingsService = settingsService;
            _settingsService.SoundChanged += ApplySoundState;
        }
        
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            ApplySoundState(_settingsService.IsSoundOn);
        }

        private void OnDestroy()
        {
            _settingsService.SoundChanged -= ApplySoundState;
        }

        public void PlayTapToPlayClip()
        {
            _audioSource.PlayOneShot(tapToPlayClip);
        }
        
        public void PlayStartGameClip()
        {
            _audioSource.PlayOneShot(playClip);
        }
        
        public void PlayMenuButtonClip()
        {
            _audioSource.PlayOneShot(menuButtonClip);
        }
        
        public void PlaySettingButtonClip()
        {
            _audioSource.PlayOneShot(settingButtonClip);
        }

        private void ApplySoundState(bool isOn)
        {
            SetMusicMixerVolume(isOn ? AudioOnValue : AudioOffValue);
        }
        
        private void SetMusicMixerVolume(float volume)
        {
            audioMixer.SetFloat(PropertiesStorage.SoundVolumeMixerProperty, volume);
        }
    }
}