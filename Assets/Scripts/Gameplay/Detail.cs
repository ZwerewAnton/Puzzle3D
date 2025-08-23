using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
    [Serializable]
    public class Detail : MonoBehaviour
    {
#if UNITY_EDITOR
        public List<bool> showPointList = new();  
        public List<bool> showParentsList = new();  
#endif
        public int count = 1;
        [FormerlySerializedAs("_prefab")] public GameObject prefab;
        public Sprite icon;
        public bool isRoot;

        private int _currentCount = 0;
        private bool _installed;

        public int CurrentCount
        {
            get => _currentCount;
            set => _currentCount = value;
        }

        public List<PointParentConnector> points = new();

        public List<PointParentConnector> GetPoints => points;

        public List<Point> GetAvailablePoints()
        {
            var list = new List<Point>();
            foreach (var pointParentConnector in points)
            {
                if(pointParentConnector.IsReady() && !pointParentConnector.IsInstalled)
                {
                    list.Add(pointParentConnector.point);
                }
            }
            return list;

        }
        public void Reset()
        {
            _currentCount = count;
            foreach (var pointParConn in points)
            {
                pointParConn.Uninstall();
            }
        }

        public bool IsLastDetail()
        {
            if (_currentCount == 0)
            {
                _currentCount = count;
            }
            if (_currentCount == 1)
            {
                _currentCount -= 1;
                return true;
            }

            _currentCount -= 1;
            return false;
        }
    }
}