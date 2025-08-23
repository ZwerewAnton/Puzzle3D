using System;
using System.Collections.Generic;

namespace SaveSystem.DataObjects
{
    [Serializable]
    public class LevelSaveData
    {
        public List<DetailSaveData> details = new();
        public float percent;
    }
}