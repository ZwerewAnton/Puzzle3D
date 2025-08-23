using Music;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class UISettingsAudioButton : MonoBehaviour
    {
        [FormerlySerializedAs("img")] [HideInInspector] public Image buttonImage;
        [FormerlySerializedAs("offImg")] public Sprite offImage;
        [FormerlySerializedAs("onImg")] public Sprite onImage;
        [FormerlySerializedAs("_dropDown")] [SerializeField] private PlayerPrefsKey dropDown = PlayerPrefsKey.Music;
        
        private bool _isOn;
        private MusicPlayer _audioPlayer;
        private enum PlayerPrefsKey
        {
            Music,
            Sound
        }

        private void Awake()
        {
            _audioPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();
            buttonImage = GetComponent<Image>();
        }
        
        private void Start()
        {
            _isOn = dropDown == PlayerPrefsKey.Music ? _audioPlayer.IsMusicOn() : _audioPlayer.IsSoundOn();
            ChangeIcon(!_isOn);
        }
        public void ToggleAudio()
        {
            ChangeIcon(_isOn);
            if (dropDown == PlayerPrefsKey.Music)
            {
                _audioPlayer.ToggleMusic();
            }
            else
            {
                _audioPlayer.ToggleSound();
            }
        }

        private void ChangeIcon(bool isOn)
        {
            buttonImage.sprite = isOn ? offImage : onImage;
            _isOn = !isOn;
        }
    }
}