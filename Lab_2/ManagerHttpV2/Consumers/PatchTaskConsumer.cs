using ManagerHttp;
using ManagerHttpV2.Handlers;
using MassTransit;
using MessagesBetweenManagerAndWorker;

namespace ManagerHttpV2.Consumers
{
    public class PatchTaskConsumer(ProjectionTaskRepository singletonDictonaryIdToWord) : IConsumer<MessageForDecryptedWord>
    {
        private readonly ProjectionTaskRepository _singletonDictonaryIdToWord = singletonDictonaryIdToWord;

        public Task Consume(ConsumeContext<MessageForDecryptedWord> context)
        {

          WordHandler.Handle(_singletonDictonaryIdToWord, context.Message);
          return Task.CompletedTask;

                
        }
    }
}
