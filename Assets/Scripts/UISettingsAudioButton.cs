using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettingsAudioButton : MonoBehaviour
{
    [HideInInspector]
    public Image img;
    public Sprite offImg;
    public Sprite onImg;
    private bool _isOn;
    private MusicPlayer _audioPlayer;

    private enum PLAYERPREFSKEY
    {
        Music,
        Sound
    }
    [SerializeField] private PLAYERPREFSKEY _dropDown = PLAYERPREFSKEY.Music;

    private void Awake()
    {
        _audioPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();
        img = GetComponent<Image>();
    }
    private void Start()
    {
        if(_dropDown == PLAYERPREFSKEY.Music)
        {
            _isOn = _audioPlayer.IsMusicOn();
        }
        else
        {
            _isOn = _audioPlayer.IsSoundOn();
        }
        ChangeIcon(!_isOn);
    }
    public void ToggleAudio()
    {
        ChangeIcon(_isOn);
        
        if(_dropDown == PLAYERPREFSKEY.Music)
        {
            _audioPlayer.ToggleMusic();
        }
        else
        {
            _audioPlayer.ToggleSound();
        }
        
    }
    public void ChangeIcon(bool isOn)
    {
        if(isOn)
        {
            img.sprite = offImg;
        }
        else
        {
            img.sprite = onImg;
        }
        _isOn = !isOn;
    }
}
