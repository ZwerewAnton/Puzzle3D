using System.Collections.Generic;
using System.Linq;
using _1_LEVEL_REWORK.New.Data;
using UnityEngine;

namespace _1_LEVEL_REWORK.New.Instances
{
    public class DetailInstance
    {
        private readonly DetailData _data;
        private bool _isGround;

        public List<PointInstance> Points { get; }
        public int RemainingCount { get; set; }
        
        public DetailInstance(DetailData data, bool isGround = false)
        {
            _data = data;
            RemainingCount = data.Count;
            Points = new List<PointInstance>();
            _isGround = isGround;
        }

        public Sprite GetDetailIcon()
        {
            return _data.Icon;
        }

        public DetailPrefab GetDetailPrefab()
        {
            return _data.Prefab;
        }

        public Mesh GetDetailMesh()
        {
            return _data.Mesh;
        }

        public Material GetDetailMaterial()
        {
            return _data.Material;
        }

        public bool IsAnyInstalled()
        {
            return Points.Any(instance => instance.IsInstalled);
        }

        public bool IsAllInstalled()
        {
            return Points.All(instance => instance.IsInstalled);
        }

        public void Reset()
        {
            RemainingCount = _data.Count;
            foreach (var p in Points) p.Uninstall();
        }
    }
}