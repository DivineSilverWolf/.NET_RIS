using MessagesBetweenManagerAndWorker;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;

namespace ManagerHttp.Requests
{
    public class RequestToWorker(RequestToWorkerConfig config) : IRequest
    {
        private readonly string _url = config.HttpUrl;
        private readonly string _mediaType = config.MediaType;

        public (bool, string[]) Request(int maxLength, string hash, string id)
        {
            try
            {
                using var httpClient = new HttpClient();
                char[] chars = config.Alphabet.ToCharArray();
                string[] tasks = chars.Select((c, i) => new { Char = c, Index = i })
                         .GroupBy(x => x.Index / (int)Math.Ceiling((double)chars.Length / config.TaskCountForOneWord))
                         .Select(g => string.Concat(g.Select(x => x.Char)))
                         .ToArray();
                foreach (string task in tasks)
                {
                    var json = (JsonSerializer.Serialize(new HashCodeMessage(maxLength, hash, id, task.ToCharArray())));
                    HttpContent content = new StringContent(json, Encoding.UTF8, _mediaType);
                    using HttpResponseMessage response = httpClient.PostAsync(_url, content).Result;
                    response.EnsureSuccessStatusCode();
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                }
                return (true, tasks);
            }
            catch
            {
                return (false, null!);
            }
        }
    }

}
