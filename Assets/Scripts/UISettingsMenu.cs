using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using DG.Tweening;

public class UISettingsMenu : MonoBehaviour
{
    [Header ("space between menu items")]
    [SerializeField] 
    private Vector2 _spacing;
    [SerializeField] 
    private Vector2 _offset;
    private const float AUDIOOFFVALUE = -80f;
    private const float AUDIOONVALUE = 0f;

    [Space]
    [Header ("Setting button rotation")]
    [SerializeField] private float rotationDuration;
    [SerializeField] private Ease rotationEase;

    [Space]
    [Header ("Animation")]
    [SerializeField] private float expandDuration;
    [SerializeField] private float collapseDuration;
    [SerializeField] private Ease expandeEase;
    [SerializeField] private Ease collapseEase;

    [Space]
    [Header ("Fading")]
    [SerializeField] private float expandFadeDuration;
    [SerializeField] private float collapseFadeDuration;

    public AudioSource gearSound;

    [SerializeField] private Button settingButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button soundButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Sprite musicOnSprite;
    [SerializeField] private Sprite musicOffSprite;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;
    private SceneLoader sceneLoader;
    public UISettingsMenuItem[] menuItems;
    private bool _isExpanded;

    private Vector2 settingButtonPosition;
    private int itemsCount;
    private RectTransform _musicButtonTransform;
    private RectTransform _soundButtonTransform;
    private RectTransform _backButtonTransform;
    private string[] playerPrefsKey = new string[2] {"music", "sound"};
    private string[] mixerVolumeParameters = new string[2] {"MusicVolume", "SoundVolume"};
    private bool[] isAudioOn = new bool[2] {true, true};
    [SerializeField] private AudioMixer audioMixer;
    
    private void Awake()
    {
        _musicButtonTransform = musicButton.gameObject.GetComponent<RectTransform>();
        _soundButtonTransform = musicButton.gameObject.GetComponent<RectTransform>();
        _backButtonTransform = musicButton.gameObject.GetComponent<RectTransform>();

        for (int i = 0; i < playerPrefsKey.Length; i++)
        {
            if (!PlayerPrefs.HasKey(playerPrefsKey[i]))
            {
                PlayerPrefs.SetFloat(playerPrefsKey[i], AUDIOONVALUE);
                audioMixer.SetFloat(mixerVolumeParameters[i], AUDIOONVALUE);
                isAudioOn[i] = true;
                PlayerPrefs.Save ();
            }
            else
            {
                if (PlayerPrefs.GetFloat (playerPrefsKey[i]) == AUDIOOFFVALUE)
                {
                    audioMixer.SetFloat(mixerVolumeParameters[i], AUDIOOFFVALUE);
                    menuItems[i].Click();
                    isAudioOn[i] = false;
                }
                else
                {
                    audioMixer.SetFloat(mixerVolumeParameters[i], AUDIOONVALUE);
                    isAudioOn[i] = true;
                }
            }
        }
    }

    private void Start()
    {
        sceneLoader = GameObject.FindObjectOfType<SceneLoader>();
        itemsCount = transform.childCount - 1;
        settingButton = transform.GetChild(0).GetComponent<Button>();
        settingButton.transform.SetAsLastSibling();
        settingButtonPosition = settingButton.GetComponent<RectTransform>().localPosition;
        ResetPosition();
    }
    
    private void ResetPosition(){
        for(int i = 0; i < itemsCount; i++){
            menuItems[i].trans.localPosition =  settingButtonPosition;
        }
    }

    public void ToggleMenu()
    {
        gearSound.Play();
        if(_isExpanded)
        {
            for(int i = 0; i < itemsCount; i++)
            {
                menuItems[i].trans.DOLocalMove(settingButtonPosition, collapseDuration).SetEase(collapseEase);   
                menuItems[i].img.DOFade(0f, collapseFadeDuration);
            }
        }
        else
        {
            for(int i = 0; i < itemsCount; i++)
            {
                menuItems[i].trans.DOLocalMove(settingButtonPosition + _spacing * (i+1), expandDuration).SetEase(expandeEase);
                menuItems[i].img.DOFade(1f, expandFadeDuration).From(0f);
            }
        }
        settingButton.transform
            .DOLocalRotate(Vector3.forward * 180f, rotationDuration)
            .From(Vector3.zero)
            .SetEase(rotationEase);

        _isExpanded = !_isExpanded;
    }

    public void ToggleMusic()
    {
        ToggleAudio(0);
    }

    public void ToggleSound()
    {
        ToggleAudio(1);
    }    
    public void ToggleAudio(int i)
    {
        if (isAudioOn[i])
        {
            PlayerPrefs.SetFloat (playerPrefsKey[i], AUDIOOFFVALUE);
            audioMixer.SetFloat(mixerVolumeParameters[i], AUDIOOFFVALUE);
            isAudioOn[i] = false;
        }
        else
        {
            PlayerPrefs.SetFloat (playerPrefsKey[i], AUDIOONVALUE);
            audioMixer.SetFloat(mixerVolumeParameters[i], AUDIOONVALUE);
            isAudioOn[i] = true;
        }
        PlayerPrefs.Save ();
    }

    public void Home()
    {
        sceneLoader.LoadNextScene();
    }
}
