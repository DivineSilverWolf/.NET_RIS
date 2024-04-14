using System.Collections.Concurrent;

namespace ManagerHttp
{
    public class SingletonDictonaryIdToWord
    {
        public ConcurrentDictionary<string, string> IdtoWord { get; set; } = [];
        public ConcurrentHashSet<string> IdtoException { get; set; } = new ConcurrentHashSet<string>();

    }
}
