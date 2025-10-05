using System;
using System.Collections.Generic;

namespace SaveSystem.DataObjects.Level
{
    [Serializable]
    public class DetailSaveData_old
    {
        public string detailName;
        public int currentCount;
        public List<PointParentConnectorSaveData_old> parentList = new();
    }
}