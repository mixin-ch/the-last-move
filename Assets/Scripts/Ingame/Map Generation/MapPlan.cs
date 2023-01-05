using Mixin.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Mixin.TheLastMove
{
    [System.Serializable]
    public class MapPlan
    {
        private List<BlockPlan> _blockPlanList;
        private List<ObstaclePlan> _obstaclePlanList;

        public IReadOnlyCollection<BlockPlan> BlockPlanList { get => _blockPlanList.AsReadOnly(); }
        public IReadOnlyCollection<ObstaclePlan> ObstaclePlanList { get => _obstaclePlanList.AsReadOnly(); }

        public MapPlan(List<BlockPlan> blockPlanList, List<ObstaclePlan> obstaclePlanList)
        {
            _blockPlanList = blockPlanList;
            _obstaclePlanList = obstaclePlanList;
        }
    }
}
