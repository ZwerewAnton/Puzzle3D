using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevelMenu : MonoBehaviour
{
    [SerializeField] private AudioClip _endClip;
    [SerializeField] private Button _homeButton;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private AudioSource _audioSource;

    public void PlayEndClip()
    {
        _audioSource.PlayOneShot(_endClip);
    }
    public void ShowHomeButton()
    {
        _homeButton.gameObject.SetActive(true);
        _scrollRect.gameObject.SetActive(false);
    }
}
