using Md5_Selection;
using MessagesBetweenManagerAndWorker;

namespace WorkerHttp.TimeoutPerformers
{
    public class ExecutorTimeoutforHashWorker(ExecutorTimeoutforHashWorkerConfig executorTimeoutforHashWorkerConfig): IExecutorTimeOut
    {
        public int TimeoutMilliseconds { get; set; } = executorTimeoutforHashWorkerConfig.TimeoutMilliseconds;

        public string GetWordForHash(WorkerSearchForHash workerSearchForHash, HashCodeMessage hash, out bool flag)
        {
            workerSearchForHash.LetterCheckArray = hash.LetterCheckArray;
            string word = "";
            flag = false;

            var task = Task.Run(() =>
            {
                return workerSearchForHash.StartFindWordForHash(hash.MaxLengthValue, hash.HashCodeValue);
            });

            if (TimeoutMilliseconds < 0)
            {
                task.Wait();
                word = task.Result ?? "";
                return word;
            }
            else
            {
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
}
