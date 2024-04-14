using Md5_Selection;
using MessagesBetweenManagerAndWorker;

namespace WorkerHttp.TimeoutPerformers
{
    public interface IExecutorTimeOut
    {
        public string GetWordForHash(WorkerSearchForHash workerSearchForHash, HashCodeMessage hash, out bool flag);
    }
}
