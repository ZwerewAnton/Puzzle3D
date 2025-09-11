using UnityEngine;

namespace Configs
{
    public class ApplicationConfigs : ScriptableObject
    {
        [Header("Audio")]
        public float audioOffValue = -80f;
        public float audioOnValue = -20f;
        public float musicFadeTime = 1f;
    }
}