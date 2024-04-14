using Md5_Selection;
using MessagesBetweenManagerAndWorker;
using WorkerHttp.TimeoutPerformers;

namespace WorkerHttp.Новая_папка
{
    public class ExecutorTimeoutforHashWorker(ExecutorTimeoutforHashWorkerConfig executorTimeoutforHashWorkerConfig): IExecutorTimeOut
    {
        public int TimeoutMilliseconds { get; set; } = executorTimeoutforHashWorkerConfig.TimeoutMilliseconds;

        public string GetWordForHash(WorkerSearchForHash workerSearchForHash, HashCodeMessage hash, out bool flag)
        {
            string word = "";
            flag = false;

            var task = Task.Run(() =>
            {
                return workerSearchForHash.StartFindWordForHash(hash.MaxLengthValue, hash.HashCodeValue);
            });

            if (!task.Wait(TimeSpan.FromMilliseconds(TimeoutMilliseconds)))
            {
                flag = true;
                workerSearchForHash.StopFlag = true;
                return word;
            }

            word = task.Result ?? "";

            return word;
        }
    }
}
