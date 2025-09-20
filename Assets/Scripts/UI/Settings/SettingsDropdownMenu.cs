using DG.Tweening;
using Infrastructure.SceneManagement;
using Music;
using Settings;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace UI.Settings
{
    public class SettingsDropdownMenu : MonoBehaviour
    {
        [Header ("Buttons")]
        [SerializeField] private Button settingButton;
        [SerializeField] private Button[] menuButtons;
        
        [Space]
        [Header ("Spacing between menu items")]
        [FormerlySerializedAs("_spacing")] [SerializeField] 
        private Vector2 spacing;
        [FormerlySerializedAs("_offset")] [SerializeField] 
        private Vector2 offset;

        [Space]
        [Header ("Main button rotation")]
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
        
        //[SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip toggleMenuClip;
        [SerializeField] private AudioClip toggleButtonClip;
        
        private SceneSwitcher _sceneSwitcher;
        
        private Vector2 _settingButtonPosition;
        private RectTransform[] _buttonsTransform;
        private Image[] _buttonsImage;
        private int _itemsCount;
        private bool _isExpanded;

        [Inject]
        private void Construct(SceneSwitcher sceneSwitcher)
        {
            _sceneSwitcher = sceneSwitcher;
        }
    
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
            settingButton = transform.GetChild(0).GetComponent<Button>();
            settingButton.transform.SetAsLastSibling();
            _settingButtonPosition = settingButton.GetComponent<RectTransform>().localPosition;
            _itemsCount = transform.childCount - 1;
            
            ResetButtonsPosition();
        }

        public void ToggleMenu()
        {
            AnimateButtons();
            _isExpanded = !_isExpanded;
        }

        public void HomeButtonClick()
        {
            _sceneSwitcher.LoadNextScene();
        }

        private void AnimateButtons()
        {
            if (_isExpanded)
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
        }
    
        private void ResetButtonsPosition()
        {
            for (var i = 0; i < _itemsCount; i++)
            {
                _buttonsTransform[i].transform.localPosition = _settingButtonPosition;
            }
        }
    }
}