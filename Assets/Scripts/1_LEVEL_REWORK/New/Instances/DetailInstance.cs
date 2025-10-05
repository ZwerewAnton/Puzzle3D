using System.Collections.Generic;
using System.Linq;
using _1_LEVEL_REWORK.New.Data;

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

        public DetailPrefab GetDetailPrefab()
        {
            return _data.Prefab;
        }

        public bool IsAnyInstalled()
        {
            return Points.Any(instance => instance.IsInstalled);
        }

        public void Reset()
        {
            RemainingCount = _data.Count;
            foreach (var p in Points) p.Uninstall();
        }
    }
}