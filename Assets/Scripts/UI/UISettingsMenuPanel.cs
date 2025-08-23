using DG.Tweening;
using SceneManagement;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class UISettingsMenuPanel : MonoBehaviour
    {
        [Header ("Buttons")]
        [SerializeField] private Button settingButton;
        [SerializeField] private Button[] menuButtons;
        
        [Space]
        [Header ("Space between menu items")]
        [FormerlySerializedAs("_spacing")] [SerializeField] 
        private Vector2 spacing;
        [FormerlySerializedAs("_offset")] [SerializeField] 
        private Vector2 offset;

        [Space]
        [Header ("Setting button rotation")]
        [SerializeField] private float rotationDuration;
        [SerializeField] private Ease rotationEase;

        [Space]
        [Header ("Animation")]
        [SerializeField] private float expandDuration;
        [SerializeField] private float collapseDuration;
        [FormerlySerializedAs("expandeEase")] [SerializeField] private Ease expandEase;
        [SerializeField] private Ease collapseEase;

        [Space]
        [Header ("Fading")]
        [SerializeField] private float expandFadeDuration;
        [SerializeField] private float collapseFadeDuration;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip toggleMenuClip;
        [SerializeField] private AudioClip toggleButtonClip;
        [SerializeField] private AudioMixer audioMixer;
        
        private SceneLoader _sceneLoader;
        private RectTransform[] _buttonsTransform;
        private Image[] _buttonsImage;
        private bool _isExpanded;
        private Vector2 _settingButtonPosition;
        private int _itemsCount;
    
        private void Awake()
        {
            _buttonsTransform = new RectTransform[menuButtons.Length];
            _buttonsImage = new Image[menuButtons.Length];

            for (var i = 0; i < menuButtons.Length; i++)
            {
                _buttonsTransform[i] = menuButtons[i].gameObject.GetComponent<RectTransform>();
                _buttonsImage[i] = menuButtons[i].gameObject.GetComponent<Image>();
            }
        }

        private void Start()
        {
            _sceneLoader = FindObjectOfType<SceneLoader>();
            _itemsCount = transform.childCount - 1;
            settingButton = transform.GetChild(0).GetComponent<Button>();
            settingButton.transform.SetAsLastSibling();

            _settingButtonPosition = settingButton.GetComponent<RectTransform>().localPosition;
        
            ResetPosition();
        }
    
        private void ResetPosition()
        {
            for (var i = 0; i < _itemsCount; i++)
            {
                _buttonsTransform[i].transform.localPosition = _settingButtonPosition;
            }
        }

        public void ToggleMenu()
        {
            PlayToggleMenuClip();
            if(_isExpanded)
            {
                for (var i = 0; i < _itemsCount; i++)
                {
                    _buttonsTransform[i].transform.DOLocalMove(_settingButtonPosition, collapseDuration).SetEase(collapseEase);   
                    _buttonsImage[i].DOFade(0f, collapseFadeDuration);
                }
            }
            else
            {
                for(var i = 0; i < _itemsCount; i++)
                {
                    _buttonsTransform[i].transform.DOLocalMove(_settingButtonPosition + spacing * (i + 1), expandDuration).SetEase(expandEase);
                    _buttonsImage[i].DOFade(1f, expandFadeDuration).From(0f);
                }
            }
            settingButton.transform
                .DOLocalRotate(Vector3.forward * 180f, rotationDuration)
                .From(Vector3.zero)
                .SetEase(rotationEase);

            _isExpanded = !_isExpanded;
        }

        public void Home()
        {
            PlayButtonClip();
            _sceneLoader.LoadNextScene();
        }
        
        private void PlayToggleMenuClip()
        {
            audioSource.PlayOneShot(toggleMenuClip);
        }
        
        public void PlayButtonClip()
        {
            audioSource.PlayOneShot(toggleButtonClip);
        }
    }
}