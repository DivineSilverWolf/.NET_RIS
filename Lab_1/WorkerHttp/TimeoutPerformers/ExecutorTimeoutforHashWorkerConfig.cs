namespace WorkerHttp.TimeoutPerformers
{
    public class ExecutorTimeoutforHashWorkerConfig(int TimeoutMilliseconds)
    {
         public int TimeoutMilliseconds { get; set; } = TimeoutMilliseconds;
    }
}
