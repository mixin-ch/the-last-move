using Mixin.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace Mixin.TheLastMove.Utils
{
    public static class IEnumerableExtensions
    {
        public static MixinDictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<IKeyAndValueLocked<TKey, TValue>> list)
        {
            MixinDictionary<TKey, TValue> TileDict = new MixinDictionary<TKey, TValue>();

            foreach (IKeyAndValueLocked<TKey, TValue> keyAndValueLocked in list)
                TileDict.Add(keyAndValueLocked.GetKey(), keyAndValueLocked.GetValue());

            return TileDict;
        }
    }
}
