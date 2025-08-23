using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class UILevelMenu : MonoBehaviour
    {
        [FormerlySerializedAs("_endClip")] [SerializeField] private AudioClip endClip;
        [FormerlySerializedAs("_homeButton")] [SerializeField] private Button homeButton;
        [FormerlySerializedAs("_scrollRect")] [SerializeField] private ScrollRect scrollRect;
        [FormerlySerializedAs("_audioSource")] [SerializeField] private AudioSource audioSource;

        public void PlayEndClip()
        {
            audioSource.PlayOneShot(endClip);
        }
        public void ShowHomeButton()
        {
            homeButton.gameObject.SetActive(true);
            scrollRect.gameObject.SetActive(false);
        }
    }
}