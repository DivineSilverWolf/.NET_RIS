using ManagerHttp.Requests;
using MassTransit;
using MessagesBetweenManagerAndWorker;
using Microsoft.Extensions.Options;

namespace ManagerHttpV2.Requests
{
    public class RequestToWorkerForConsumer(IOptions<RequestToWorkerConfig> config, IBus bus) : IRequest
    {
        private readonly string _url = config.Value.QueueUrl;
        private readonly IBus _bus = bus;

        public (bool, string[]) Request(int maxLength, string hash, string id)
        {
            try
            {
                ISendEndpoint endpoint = _bus.GetSendEndpoint(new Uri(_url)).Result;
                char[] chars = config.Value.Alphabet.ToCharArray();
                int chunkSize = (int) Math.Ceiling((double) config.Value.Alphabet.Length / config.Value.TaskCountForOneWord);
                string[] tasks = chars.Select((c, i) => new { Char = c, Index = i })
                    .GroupBy(x => x.Index / (int)Math.Ceiling((double)config.Value.Alphabet.Length / config.Value.TaskCountForOneWord))  // Группировка индексов
                    .Select(g => string.Concat(g.Select(x => x.Char)))  // Объединение символов внутри каждой группы
                    .ToArray();

                foreach (string task in tasks)
                {
                    endpoint.Send(new HashCodeMessage(maxLength, hash, id, task.ToCharArray()));
                }
                return (true, tasks);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return (false, null!);
            }
        }
    }
}
