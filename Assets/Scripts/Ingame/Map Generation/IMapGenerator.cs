using Mixin.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Mixin.TheLastMove
{
    public interface IMapGenerator
    {
        public MapPlan Tick(float blockChunkSize);
    }
}
