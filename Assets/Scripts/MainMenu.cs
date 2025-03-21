using UnityEngine;
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

    public GameObject disassembleWindow;
    public GameObject disassembleButton;

    private SceneLoader _sceneLoader;
    private MusicPlayer _musicPlayer;
    [SerializeField] private UnityEvent _onFirstTap;
    
    private void Start()
    {
        _sceneLoader = FindObjectOfType<SceneLoader>();
        _musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();
        if(_sceneLoader.IsSecondLaunch())
        {
            HideStartScreen();
        }
    }
    
    public void FirstTap()
    {
        HideStartScreen();
        PlayTapToPlayClip();
        _musicPlayer.Play();
    }
    
    public void Play()
    {
        SaveLevel.levelID = scrollController.GetLevelID();
        PlayStartGameClip();
        _sceneLoader.LoadNextScene();
    }
    
    public void ShowDisassembleWindow()
    {
        disassembleWindow.SetActive(true);
    }
    
    public void CloseDisassembleWindow()
    {
        disassembleWindow.SetActive(false);
    }
    
    public void DisassembleDetail()
    {
        var levelID = scrollController.GetLevelID();
        SaveLevel.levelID = levelID;
        LevelContainer.currentLevelContainer.ResetLevel(levelID);
        scrollController.UpdatePercents();
        disassembleWindow.SetActive(false);
    }

    private void HideStartScreen()
    {
        _onFirstTap.Invoke();
        tapToPlayGO.SetActive(false);
        miniHouse.SetActive(false);
        playButton.SetActive(true);
        disassembleButton.SetActive(true);
        scrollRect.SetActive(true);
        settingsPanel.SetActive(true);
    }
    
    private void PlayTapToPlayClip()
    {
        audioSource.PlayOneShot(tapToPlayClip);
    }
    
    private void PlayStartGameClip()
    {
        audioSource.PlayOneShot(playClip);
    }
}
