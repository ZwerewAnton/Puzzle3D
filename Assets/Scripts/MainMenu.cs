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
    private SceneLoader sceneLoader;
    [SerializeField] private UnityEvent _onFirstTap;
    private MusicPlayer musicPlayer;
    
    private void Start()
    {
        sceneLoader = GameObject.FindObjectOfType<SceneLoader>();
        musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();
        if(sceneLoader.IsSecondLauch())
        {
            FirstTap();
        }
    }
    public void FirstTap()
    {
        _onFirstTap.Invoke();
        musicPlayer.Play();
        tapToPlayGO.SetActive(false);
        miniHouse.SetActive(false);
        playButton.SetActive(true);
        scrollRect.SetActive(true);
        settingsPanel.SetActive(true);
    }
    public void Play()
    {
        SaveLevel.levelID = scrollController.GetLevelID();
        sceneLoader.LoadNextScene();
    }


}
