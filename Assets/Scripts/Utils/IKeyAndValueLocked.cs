namespace Mixin.TheLastMove.Utils
{
    public interface IKeyAndValueLocked<TKey, TValue>
    {
        public TKey GetKey();
        public TValue GetValue();
    }
}
