using System;
using System.Collections.Generic;

namespace SaveSystem.DataObjects
{
    [Serializable]
    public class DetailSaveData
    {
        public string detailName;
        public int currentCount;
        public List<PointParentConnectorSaveData> parentList = new();
    }
}