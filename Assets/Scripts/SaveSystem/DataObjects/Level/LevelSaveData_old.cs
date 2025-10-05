using System;
using System.Collections.Generic;

namespace SaveSystem.DataObjects.Level
{
    [Serializable]
    public class LevelSaveData
    {
        public List<DetailSaveData_old> details = new();
        public float percent;
    }
}