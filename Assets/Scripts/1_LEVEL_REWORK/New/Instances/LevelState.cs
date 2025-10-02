using System.Collections.Generic;
using _1_LEVEL_REWORK.New.Data;

namespace _1_LEVEL_REWORK.New.Instances
{
    public class LevelState
    {
        private readonly Dictionary<string, DetailInstance> _detailInstances = new();
        private readonly DependencyGraph _dependencyGraph = new();
        
        public DependencyGraph DependencyGraph => _dependencyGraph;
        public IReadOnlyDictionary<string, DetailInstance> Details => _detailInstances;
        
        public LevelState(LevelData data)
        {
            CreateDetailInstances(data.Details);
            CreateDependencyGraph(data.Details);
        }

        private void CreateDetailInstances(List<DetailData> detailDataList)
        {
            foreach (var detailData in detailDataList)
            {
                var instance = new DetailInstance(detailData);
                _detailInstances[detailData.Id] = instance;
            }
        }

        private void CreateDependencyGraph(List<DetailData> detailDataList)
        {
            foreach (var detailData in detailDataList)
            {
                var detailInstance = _detailInstances[detailData.Id];
                for (var i = 0; i < detailData.points.Count; i++)
                {
                    var pointData = detailData.points[i];
                    var pointInstance = detailInstance.Points[i];

                    foreach (var parent in pointData.constraints)
                    {
                        var parentDetail = _detailInstances[parent.ParentDetail.Id];
                        foreach (var idx in parent.ParentPointIndexes)
                        {
                            var parentPoint = parentDetail.Points[idx];
                            _dependencyGraph.AddDependency(pointInstance, parentPoint);
                        }
                    }
                }
            }
        }
    }
}