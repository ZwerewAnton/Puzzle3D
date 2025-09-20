using UnityEngine;
using UnityEngine.Serialization;

namespace Configs
{
    [CreateAssetMenu(fileName = "ApplicationConfigs", menuName = "My Assets/Configs/ApplicationConfigs")]
    public class ApplicationConfigs : ScriptableObject
    {
        [Header("Scene Loading")]
        public float minTimeToShowLoadingScreen = 1f;
        [FormerlySerializedAs("loadingScreenFadeTIme")] public float loadingScreenFadeTime = 0.5f;
        
        [Header("Audio")]
        public float audioOffValue = -80f;
        public float audioOnValue = -20f;
        public float musicFadeTime = 1f;
    }
}