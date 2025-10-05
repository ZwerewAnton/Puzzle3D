using Gameplay;
using Music;
using UnityEngine;
using Zenject;

namespace Level
{
    public class LevelSceneEntryPoint : MonoBehaviour
    {
        private LevelService _levelService;
        private MusicPlayer _musicPlayer;
        
        [Inject]
        private void Construct(LevelService levelService, MusicPlayer musicPlayer)
        {
            _levelService = levelService;
            _musicPlayer = musicPlayer;
        }

        private void Start()
        {
            _ = _levelService.InitializeLevel();
            _musicPlayer.Play(MusicType.Level);
        }
    }
}