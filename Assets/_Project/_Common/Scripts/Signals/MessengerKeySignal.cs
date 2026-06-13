namespace _Project._Common.Scripts.Signals
{
    public struct MessengerKeySignal<TKey, TSignal>
    {
        private readonly TKey _key;
        private TSignal _signal;
        
        public MessengerKeySignal(TKey key, TSignal signal)
        {
            _key = key;
            _signal = signal;
        }
        
        public bool CompareChanel(TKey key)
            => _key.Equals(key);
    }
}