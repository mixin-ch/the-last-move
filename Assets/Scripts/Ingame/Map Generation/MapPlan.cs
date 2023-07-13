using System.Collections.Generic;

namespace Mixin.TheLastMove
{
    [System.Serializable]
    public class MapPlan
    {
        private List<BlockPlan> _blockPlanList;
        private List<ObstaclePlan> _obstaclePlanList;
        private List<CollectablePlan> _collectablePlanList;

        public IReadOnlyCollection<BlockPlan> BlockPlanList { get => _blockPlanList.AsReadOnly(); }
        public IReadOnlyCollection<ObstaclePlan> ObstaclePlanList { get => _obstaclePlanList.AsReadOnly(); }
        public IReadOnlyCollection<CollectablePlan> CollectablePlanList { get => _collectablePlanList.AsReadOnly(); }

        public MapPlan(List<BlockPlan> blockPlanList, List<ObstaclePlan> obstaclePlanList, List<CollectablePlan> collectablePlanList)
        {
            _blockPlanList = blockPlanList;
            _obstaclePlanList = obstaclePlanList;
            _collectablePlanList = collectablePlanList;
        }
    }
}
