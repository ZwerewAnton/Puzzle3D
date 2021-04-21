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
    public void FirstTap()
    {
        tapToPlayGO.SetActive(false);
        miniHouse.SetActive(false);
        playButton.SetActive(true);
        scrollRect.SetActive(true);
    }
    public void Play(){
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SaveLevel.levelID = scrollController.GetLevelID();
        StartCoroutine(LoadAsync());
    }

    IEnumerator LoadAsync(){
        AsyncOperation asyncLoadOperation  = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        //asyncLoadOperation.allowSceneActivation = false;
        while(!asyncLoadOperation.isDone){
            yield return null;
        }
        
    }
    public void LoadAnimation()
    {

    }


}
