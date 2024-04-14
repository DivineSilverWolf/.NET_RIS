using MassTransit;
using MessagesBetweenManagerAndWorker;
using WorkerHttp.Requests;
using Microsoft.Extensions.Options;

namespace WorkerHttpV2.Requests
{
    public class RequestToManagerForConsumer(IOptions<RequestToManagerConfig> config, IBus bus) : IRequest
    {
        private readonly IBus _bus = bus;
        private readonly string _url = config.Value.QueueUrl;
        public void SendWordToManager(string word, HashCodeMessage hash, bool flag)
        {
            try
            {
                ISendEndpoint endpoint = _bus.GetSendEndpoint(new Uri(_url)).Result;
                endpoint.Send(new MessageForDecryptedWord(word, hash.IdValue!, hash.LetterCheckArray, flag));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
