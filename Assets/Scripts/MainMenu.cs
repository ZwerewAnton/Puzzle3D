using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class MainMenu : MonoBehaviour
{
    public GameObject tapToPlayGO;
    public GameObject playButton;
    public GameObject scrollRect;
    public GameObject miniHouse;
    public GameObject settingsPanel;
    public UIMainMenuScrollRectController scrollController;
    public AudioSource audioSource;
    public AudioClip tapToPlayClip;
    public AudioClip playClip;
    private SceneLoader sceneLoader;
    [SerializeField] private UnityEvent _onFirstTap;
    private MusicPlayer musicPlayer;
    
    private void Start()
    {
        sceneLoader = GameObject.FindObjectOfType<SceneLoader>();
        musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();
        if(sceneLoader.IsSecondLauch())
        {
            HideStartScreen();
        }
    }
    public void FirstTap()
    {
        HideStartScreen();
        PlayTapToPlayClip();
        musicPlayer.Play();
    }
    public void Play()
    {
        SaveLevel.levelID = scrollController.GetLevelID();
        PlayStartGameClip();
        sceneLoader.LoadNextScene();
    }
    private void HideStartScreen()
    {
        _onFirstTap.Invoke();
        tapToPlayGO.SetActive(false);
        miniHouse.SetActive(false);
        playButton.SetActive(true);
        scrollRect.SetActive(true);
        settingsPanel.SetActive(true);
    }
    private void PlayTapToPlayClip(){
        audioSource.PlayOneShot(tapToPlayClip);
    }
    private void PlayStartGameClip(){
        audioSource.PlayOneShot(playClip);
    }


}
