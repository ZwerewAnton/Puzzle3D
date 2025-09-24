using Music;
using UI.Common;
using UI.MainMenu;
using UnityEngine;
using Zenject;

namespace UI.Mediators
{
    public class MainMenuMediator : MonoBehaviour
    {
        [SerializeField] private TapToPlayPanel tapToPlayPanel;
        [SerializeField] private SimplePanel menuPanel;
        [SerializeField] private SimplePanel miniHousePanel;
        [SerializeField] private MainMenu.MainMenu mainMenu;

        private SfxPlayer _sfxPlayer;

        [Inject]
        private void Construct(SfxPlayer sfxPlayer)
        {
            _sfxPlayer = sfxPlayer;
        }

        private void Awake()
        {
            tapToPlayPanel.Clicked += mainMenu.FirstTap;
            tapToPlayPanel.Clicked += _sfxPlayer.PlayTapToPlayClip;
        }

        private void OnDestroy()
        {
            tapToPlayPanel.Clicked -= mainMenu.FirstTap;
            tapToPlayPanel.Clicked -= _sfxPlayer.PlayTapToPlayClip;
        }

        public void ShowTapToPlayPanel()
        {
            tapToPlayPanel.Show();
            miniHousePanel.Show();
            menuPanel.Hide();
        }
        
        public void HideTapToPlayPanel()
        {
            tapToPlayPanel.Hide();
            miniHousePanel.Hide();
            menuPanel.Show();
        }
    }
}