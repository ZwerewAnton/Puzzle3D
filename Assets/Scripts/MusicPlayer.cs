using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Audio;

public class MusicPlayer : MonoBehaviour
{
    private const float MAXVOLUME = 1f;
    private const float MINVOLUME = 0f;
    private const float AUDIOOFFVALUE = -80f;
    private const float AUDIOONVALUE = 0f;
    [SerializeField] private AudioMixer _audioMixer;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _menuMusicClip;
    [SerializeField] private AudioClip _gameMusicClip;
    [SerializeField] private float _fadeTime = 1f;
    private float _volume = 0f;
    private bool _isSoundOn, _isMusicOn;
    private string musicKey, soundKey, musicMixerParameter, soundMixerParameter;
    private int sceneIndex = 0;


    private void Awake()
    {
        musicKey = PropertiesStorage.GetMusicKey();
        soundKey = PropertiesStorage.GetSoundKey();
        musicMixerParameter = PropertiesStorage.GetMusicVolumeMixerParameter();
        soundMixerParameter = PropertiesStorage.GetSoundVolumeMixerParameter();

        DontDestroyOnLoad(transform.gameObject);
        _audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        _isSoundOn = GetPlayerPrefsVolumeState(soundKey, soundMixerParameter);
        _isMusicOn = GetPlayerPrefsVolumeState(musicKey, musicMixerParameter);

        _audioSource.clip = _menuMusicClip;
    }

    public void Play()
    {
        if(_isMusicOn)
        {
            if(sceneIndex == 0)
            {
                _audioSource.clip = _menuMusicClip;
                FadeIn();
            }
            else if(sceneIndex == 1)
            {
                _audioSource.clip = _gameMusicClip;
                FadeIn();
            }
            else
            {
                _audioSource.Stop();
            }
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
            PlayerPrefs.SetFloat(soundKey, AUDIOOFFVALUE);
            _isSoundOn = false;
            _audioMixer.SetFloat(soundMixerParameter, AUDIOOFFVALUE);
        }
        else
        {
            PlayerPrefs.SetFloat(soundKey, AUDIOONVALUE);
            _isSoundOn = true;
            _audioMixer.SetFloat(soundMixerParameter, AUDIOONVALUE);
        }
        PlayerPrefs.Save();
    }   

    public void ToggleMusic()
    {
        if(_isMusicOn)
        {
            PlayerPrefs.SetFloat (PropertiesStorage.GetMusicKey(), AUDIOOFFVALUE);
            _isMusicOn = false;
            Pause();
        }
        else
        {
            PlayerPrefs.SetFloat (PropertiesStorage.GetMusicKey(), AUDIOONVALUE);
            _isMusicOn = true;
            FadeIn();
        }
        PlayerPrefs.Save();
    }

    private bool GetPlayerPrefsVolumeState(string key, string mixerParameter)
    {
        if(PlayerPrefs.HasKey(key))
        {
            if(PlayerPrefs.GetFloat(key) == AUDIOOFFVALUE)
            {
                _audioMixer.SetFloat(mixerParameter, AUDIOOFFVALUE);
                return false;
            }
            else
            {
                _audioMixer.SetFloat(mixerParameter, AUDIOONVALUE);
                return true;
            }
        }
        else
        {
            PlayerPrefs.SetFloat(key, AUDIOONVALUE);
            PlayerPrefs.Save();
            return true;
        }
    }

    public float volume
    {
        get
        {
            return _volume;
        }
        set
        {
            _volume = value;
            //_audioSource.volume = _volume;
            SetMusicMixerVolume(_volume);
            //_audioMixer.SetFloat(soundMixerParameter, _volume);
        }
    }
    
    private void FadeIn()
    {
        //_audioSource.volume = MINVOLUME;
        SetMusicMixerVolume(AUDIOOFFVALUE);
        _audioSource.Play();
        DOTween
            .To(() => volume, x => volume = x, AUDIOONVALUE, _fadeTime);
    }

    public void Stop()
    {
        DOTween
            .To(() => volume, x => volume = x, AUDIOOFFVALUE, _fadeTime)
            .OnComplete(_audioSource.Stop);
    }    
    private void Pause()
    {
        DOTween
            .To(() => volume, x => volume = x, AUDIOOFFVALUE, _fadeTime)
            .OnComplete(_audioSource.Pause);
    }
    private void SetMusicMixerVolume(float volume)
    {
        _audioMixer.SetFloat(musicMixerParameter, volume);
    }
    
}
