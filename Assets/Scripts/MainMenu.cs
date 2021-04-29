using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject tapToPlayGO;
    public GameObject playButton;
    public GameObject scrollRect;
    public GameObject miniHouse;
    public UIMainMenuScrollRectController scrollController;
    private SceneLoader sceneLoader;
    
    private void Start()
    {
        sceneLoader = GameObject.FindObjectOfType<SceneLoader>();
    }
    public void FirstTap()
    {
        tapToPlayGO.SetActive(false);
        miniHouse.SetActive(false);
        playButton.SetActive(true);
        scrollRect.SetActive(true);
    }
    public void Play()
    {
        SaveLevel.levelID = scrollController.GetLevelID();
        sceneLoader.LoadScene();
    }


}
