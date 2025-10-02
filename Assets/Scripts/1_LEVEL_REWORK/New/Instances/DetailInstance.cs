using System.Collections.Generic;
using System.Linq;
using _1_LEVEL_REWORK.New.Data;

namespace _1_LEVEL_REWORK.New.Instances
{
    public class DetailInstance
    {
        private readonly DetailData _data;
        private readonly List<PointInstance> _points;
        
        public IReadOnlyList<PointInstance> Points => _points;
        public int RemainingCount { get; private set; }
        
        public DetailInstance(DetailData data)
        {
            _data = data;
            _points = data.points.Select(_ => new PointInstance()).ToList();
        }

        public void Reset()
        {
            RemainingCount = _data.Count;
            foreach (var p in _points) p.Uninstall();
        }

        public bool IsLastDetail()
        {
            if (RemainingCount == 0)
                RemainingCount = _data.Count;

            RemainingCount--;
            return RemainingCount == 0;
        }
    }
}