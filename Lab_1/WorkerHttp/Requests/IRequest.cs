using MessagesBetweenManagerAndWorker;

namespace WorkerHttp.Requests
{
    public interface IRequest
    {
        public void SendWordToManager(string word, HashCodeMessage hash, bool flag);
    }
}
