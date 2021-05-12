using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using DG.Tweening;

public class UISettingsMenuPanel : MonoBehaviour
{
    [Header ("space between menu items")]
    [SerializeField] 
    private Vector2 _spacing;
    [SerializeField] 
    private Vector2 _offset;

    [Space]
    [Header ("Setting button rotation")]
    [SerializeField] float rotationDuration;
    [SerializeField] Ease rotationEase;

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

    public AudioSource audioSource;
    public AudioClip toggleMenuClip;
    public AudioClip toggleButtonClip;
    //public AudioMixerGroup uiMixerGroup;
    //public AudioMixerGroup musicMixerGroup;


    [SerializeField] private Button settingButton;
    private SceneLoader sceneLoader;
    //public UISettingsMenuItem[] menuItems;
    public Button[] menuButtons;
    private RectTransform[] _buttonsTransform;
    private Image[] _buttonsImage;

    private bool _isExpanded;

    private Vector2 settingButtonPosition;
    private int itemsCount;
    private string[] playerPrefsKey = new string[2] {"music", "sound"};
    private string[] mixerVolumeParameters = new string[2] {"MusicVolume", "SoundVolume"};
    private bool[] isAudioOn = new bool[2] {true, true};
    [SerializeField]
    private AudioMixer audioMixer;
    
    private void Awake()
    {
        _buttonsTransform = new RectTransform[menuButtons.Length];
        _buttonsImage = new Image[menuButtons.Length];

        for (int i = 0; i < menuButtons.Length; i++)
        {
            _buttonsTransform[i] = menuButtons[i].gameObject.GetComponent<RectTransform>();
            _buttonsImage[i] = menuButtons[i].gameObject.GetComponent<Image>();
        }

        //PlayerPrefs.DeleteAll();
/*         for (int i = 0; i < playerPrefsKey.Length; i++)
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
        } */
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
    
    private void ResetPosition()
    {
        for(int i = 0; i < itemsCount; i++)
        {
            _buttonsTransform[i].transform.localPosition =  settingButtonPosition;
        }
    }

    public void ToggleMenu()
    {
        PlayToggleMenuClip();
        if(_isExpanded)
        {
            for(int i = 0; i < itemsCount; i++)
            {
                _buttonsTransform[i].transform.DOLocalMove(settingButtonPosition, collapseDuration).SetEase(collapseEase);   
                _buttonsImage[i].DOFade(0f, collapseFadeDuration);
            }
        }
        else
        {
            for(int i = 0; i < itemsCount; i++)
            {
                _buttonsTransform[i].transform.DOLocalMove(settingButtonPosition + _spacing * (i+1), expandDuration).SetEase(expandeEase);
                _buttonsImage[i].DOFade(1f, expandFadeDuration).From(0f);
            }
        }
        settingButton.transform
            .DOLocalRotate(Vector3.forward * 180f, rotationDuration)
            .From(Vector3.zero)
            .SetEase(rotationEase);

        _isExpanded = !_isExpanded;
    }

/*     public void ToggleMusic()
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
    } */

    public void Home()
    {
        PlayButtonClip();
        sceneLoader.LoadNextScene();
    }
    private void PlayToggleMenuClip(){
        audioSource.PlayOneShot(toggleMenuClip);
    }
    public void PlayButtonClip(){
        audioSource.PlayOneShot(toggleButtonClip);
    }
}
