using UnityEngine;

namespace Mixin.TheLastMove.Utils
{
    [System.Serializable]
    public class KeyAndValueLocked<TKey, TValue> : IKeyAndValueLocked<TKey, TValue>
    {
        [SerializeField]
        private TKey _key;
        [SerializeField]
        private TValue _value;

        public TKey Key { get => _key; }
        public TValue Value { get => _value; }

        public TKey GetKey() { return Key; }
        public TValue GetValue() { return Value; }
    }
}
