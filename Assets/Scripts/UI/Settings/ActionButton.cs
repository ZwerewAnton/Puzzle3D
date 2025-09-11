using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Settings
{
    [RequireComponent(typeof(Button))]
    public class ActionButton : MonoBehaviour
    {
        public event Action Clicked;
        
        private Button _button;

        protected virtual void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }

        protected virtual void OnDestroy()
        {
            _button.onClick.RemoveListener(OnClick);
        }
        
        private void OnClick()
        {
            Clicked?.Invoke();
        }
    }
}