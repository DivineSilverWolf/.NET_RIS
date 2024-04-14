using MassTransit;
using Md5_Selection;
using Md5_Selection.HashWorkers.InterfaceHashWorker;
using MessagesBetweenManagerAndWorker;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using WorkerHttp;
using WorkerHttp.Requests;
using WorkerHttp.TimeoutPerformers;

namespace WorkerHttpV2.Consumers
{
    public class Consumer (IExecutorTimeOut executor, IOptions<AlphabetSetting> appSettings, IHashWorker hashWorker, IRequest request) : IConsumer<HashCodeMessage>
    {
        private readonly IExecutorTimeOut _executorTime = executor;
        private readonly WorkerSearchForHash workerSearchForHash = new(appSettings.Value.Alphabet, hashWorker);
        private readonly IRequest _request = request;
        public Task Consume(ConsumeContext<HashCodeMessage> context)
        {
            HashCodeMessage hash = context.Message;
            Task.WaitAll(Task.Run(() =>
            {
                string word = _executorTime.GetWordForHash(workerSearchForHash, hash, out bool flag);
                _request.SendWordToManager(word, hash, flag);
                
            }));
            return Task.CompletedTask;
        }
    }
}
