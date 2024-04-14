using System.Collections.Concurrent;

namespace ManagerHttp
{
    public class ConcurrentHashSet<T>
    {
        private readonly ConcurrentDictionary<T, byte> _dictionary;

        public ConcurrentHashSet()
        {
            _dictionary = new ConcurrentDictionary<T, byte>();
        }

        public bool Add(T item)
        {
            return _dictionary.TryAdd(item, default);
        }

        public bool Remove(T item)
        {
            return _dictionary.TryRemove(item, out _);
        }

        public bool Contains(T item)
        {
            return _dictionary.ContainsKey(item);
        }

        public IEnumerable<T> GetAllItems()
        {
            return _dictionary.Keys;
        }
    }

}
