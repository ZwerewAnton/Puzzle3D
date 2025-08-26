using System;
using UnityEngine;

namespace SaveSystem.DataObjects.Progress
{
    [Serializable]
    public class ProgressLevelSaveData
    {
        [SerializeField] public int levelId;
        [SerializeField] public int progress;

        public ProgressLevelSaveData(int levelId, int progress)
        {
            this.levelId = levelId;
            this.progress = progress;
        }
    }
}