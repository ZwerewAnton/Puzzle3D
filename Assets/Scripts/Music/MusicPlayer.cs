using DG.Tweening;
using Settings;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Music
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicPlayer : MonoBehaviour
    {
        private const float AudioOffValue = -80f;
        private const float AudioOnValue = -20f;
        
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private AudioClip menuMusicClip;
        [SerializeField] private AudioClip gameMusicClip;
        [SerializeField] private float fadeTime = 1f;

        private SettingsService _settingsService;
        private AudioSource _audioSource;
        private float _volume;
        private Tween _fadeTween;

        private float Volume
        {
            get => _volume;
            set
            {
                _volume = value;
                SetMusicMixerVolume(_volume);
            }
        }

        [Inject]
        private void Construct(SettingsService settingsService)
        {
            _settingsService = settingsService;
            _settingsService.MusicChanged += ApplyMusicState;
        }
    
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }
    
        private void Start()
        {
            _audioSource.clip = menuMusicClip;
            Volume = AudioOffValue;
        }

        private void OnDestroy()
        {
            _settingsService.MusicChanged -= ApplyMusicState;
        }

        public void Play(MusicType type)
        {
            if (!_settingsService.IsMusicOn) 
                return;
            
            switch (type)
            {
                case MusicType.MainMenu:
                    _audioSource.clip = menuMusicClip;
                    break;
                case MusicType.Level:
                    _audioSource.clip = gameMusicClip;
                    break;
                default:
                    _audioSource.Stop();
                    return;
            }
            
            FadeIn();
        }
        
        public void Stop()
        {
            FadeOut();
        }
        
        private void FadeIn()
        {
            _fadeTween?.Kill();
            if(!_audioSource.isPlaying) 
                _audioSource.Play();
            
            _fadeTween = DOTween
                .To(() => Volume, x => Volume = x, AudioOnValue, fadeTime)
                .OnComplete(() =>_fadeTween = null);
        }
        
        private void FadeOut()
        {
            _fadeTween?.Kill();
            _fadeTween = DOTween
                .To(() => Volume, x => Volume = x, AudioOffValue, fadeTime)
                .OnComplete(() =>
                {
                    _audioSource.Stop();
                    _fadeTween = null;
                });
        }

        private void ApplyMusicState(bool isOn)
        {
            if (isOn)
            {
                FadeIn();
            }
            else
            {
                FadeOut();
            }
        }
        
        private void SetMusicMixerVolume(float volume)
        {
            audioMixer.SetFloat(PropertiesStorage.MusicVolumeMixerProperty, volume);
        }
    }
}