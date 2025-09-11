/*
using DG.Tweening;
using SceneManagement;
using Settings;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

namespace Music
{
    public class MusicPlayer2 : MonoBehaviour
    {
        [FormerlySerializedAs("_audioMixer")] [SerializeField] private AudioMixer audioMixer;
        [FormerlySerializedAs("_menuMusicClip")] [SerializeField] private AudioClip menuMusicClip;
        [FormerlySerializedAs("_gameMusicClip")] [SerializeField] private AudioClip gameMusicClip;
        [FormerlySerializedAs("_fadeTime")] [SerializeField] private float fadeTime = 1f;
        
        private static MusicPlayer2 _musicPlayer;

        private const float AudioOffValue = -80f;
        private const float AudioOnValue = -20f;
        private AudioSource _audioSource;
        private float _volume;
        private bool _isSoundOn, _isMusicOn;
        private string _musicKey, _soundKey, _musicMixerParameter, _soundMixerParameter;
        private int _sceneIndex;

        private float Volume
        {
            get => _volume;
            set
            {
                _volume = value;
                SetMusicMixerVolume(_volume);
            }
        }
    
        private void Awake()
        {
            if (_musicPlayer == null) 
            {
                _musicPlayer = this;
            } 
            else 
            {
                Destroy(gameObject);
            }

            _musicKey = PropertiesStorage.GetMusicKey();
            _soundKey = PropertiesStorage.GetSoundKey();
            _musicMixerParameter = PropertiesStorage.GetMusicVolumeMixerParameter();
            _soundMixerParameter = PropertiesStorage.GetSoundVolumeMixerParameter();

            DontDestroyOnLoad(transform.gameObject);
            _audioSource = GetComponent<AudioSource>();
        }
    
        private void Start()
        {
            _isSoundOn = GetPlayerPrefsVolumeState(_soundKey, _soundMixerParameter);
            _isMusicOn = GetPlayerPrefsVolumeState(_musicKey, _musicMixerParameter);
            Play();

            _audioSource.clip = menuMusicClip;
        }

        public void Play()
        {
            _sceneIndex = SceneLoader.GetSceneIndex();
            if (!_isMusicOn || _audioSource.isPlaying) 
                return;
            switch (_sceneIndex)
            {
                case 0:
                    _audioSource.clip = menuMusicClip;
                    FadeIn();
                    break;
                case 1:
                    _audioSource.clip = gameMusicClip;
                    FadeIn();
                    break;
                default:
                    _audioSource.Stop();
                    break;
            }
        }    

        public bool IsMusicOn()
        {
            return _isMusicOn;
        }
        
        public bool IsSoundOn()
        {
            return _isSoundOn;
        }

        public void ToggleSound()
        {
            if (_isSoundOn)
            {
                PlayerPrefs.SetFloat(_soundKey, AudioOffValue);
                _isSoundOn = false;
                audioMixer.SetFloat(_soundMixerParameter, AudioOffValue);
            }
            else
            {
                PlayerPrefs.SetFloat(_soundKey, AudioOnValue);
                _isSoundOn = true;
                audioMixer.SetFloat(_soundMixerParameter, AudioOnValue);
            }
            PlayerPrefs.Save();
        }

        public void ToggleMusic()
        {
            if (_isMusicOn)
            {
                PlayerPrefs.SetFloat(PropertiesStorage.GetMusicKey(), AudioOffValue);
                _isMusicOn = false;
                Pause();
            }
            else
            {
                PlayerPrefs.SetFloat(PropertiesStorage.GetMusicKey(), AudioOnValue);
                _isMusicOn = true;
                FadeIn();
            }
            PlayerPrefs.Save();
        }
        
        private bool GetPlayerPrefsVolumeState(string key, string mixerParameter)
        {
            if (PlayerPrefs.HasKey(key))
            {
                if (Mathf.Approximately(PlayerPrefs.GetFloat(key), AudioOffValue))
                {
                    audioMixer.SetFloat(mixerParameter, AudioOffValue);
                    return false;
                }

                audioMixer.SetFloat(mixerParameter, AudioOnValue);
                return true;
            }

            PlayerPrefs.SetFloat(key, AudioOnValue);
            PlayerPrefs.Save();
            return true;
        }
        
        private void FadeIn()
        {
            Volume = AudioOffValue;
            _audioSource.Play();
            DOTween
                .To(() => Volume, x => Volume = x, AudioOnValue, fadeTime);
        }
        
        public void Stop()
        {
            DOTween
                .To(() => Volume, x => Volume = x, AudioOffValue, fadeTime)
                .OnComplete(_audioSource.Stop);
        }
        
        private void Pause()
        {
            DOTween
                .To(() => Volume, x => Volume = x, AudioOffValue, fadeTime)
                .OnComplete(_audioSource.Pause);
        }
        
        private void SetMusicMixerVolume(float volume)
        {
            audioMixer.SetFloat(_musicMixerParameter, volume);
        }
    }
}
*/