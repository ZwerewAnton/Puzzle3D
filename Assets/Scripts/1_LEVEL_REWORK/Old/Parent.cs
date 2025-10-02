using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Gameplay
{
    [Serializable]
    public class Parent
    {
#if UNITY_EDITOR
        public List<bool> showParentPointList = new();  
#endif
        [FormerlySerializedAs("checkToogleList")] public List<bool> checkToggleList = new();
        public Detail parentDetail;
        public List<Point> parentPointList = new(); 

        public List<PointParentConnector> GetAllPpc()
        {
            var pointParentConnectors = new List<PointParentConnector>();
            for (var i = 0; i < parentDetail.points.Count; i++)
            {
                if (checkToggleList[i])
                {
                    pointParentConnectors.Add(parentDetail.points[i]);
                }
            }
            return pointParentConnectors;
        }

        public bool IsParentInstall()
        {
            var list = GetAllPpc();
            foreach (var ppc in list)
            {
                if (!ppc.IsInstalled)
                {
                    return false;
                }
            }
            return true;
        }
    }
}