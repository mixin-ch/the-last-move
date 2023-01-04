using Mixin.Save;
using System;

namespace Mixin.TheLastMove.Save
{
    [Serializable]
    public class IngameData : DataFile
    {
        public int Highscore;
    }
}
