using System.Collections.Generic;

namespace DiscreteApproach
{
    public class IndexDictionary<K, V>
    {
        private readonly Dictionary<K, List<V>> _indexData = new Dictionary<K, List<V>>();

        public void Add(K key, V value)
        {
            if (!_indexData.ContainsKey(key))
            {
                _indexData.Add(key, new List<V>());
            }

            _indexData[key].Add(value);
        }

        public List<V> GetValue(K key)
        {
            return _indexData.ContainsKey(key) ? _indexData[key] : new List<V>();
        }
    }
}