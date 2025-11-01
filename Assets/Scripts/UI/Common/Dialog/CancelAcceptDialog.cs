using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Common.Dialog
{
    public class CancelAcceptDialog : BaseDialog
    {
        [SerializeField] private Button cancelButton;
        [SerializeField] private Button acceptButton;

        private void OnEnable()
        {
            cancelButton.onClick.AddListener(Cancel);
            acceptButton.onClick.AddListener(Accept);
        }

        private void OnDisable()
        {
            cancelButton.onClick.RemoveListener(Cancel);
            acceptButton.onClick.RemoveListener(Accept);
        }
    }
}