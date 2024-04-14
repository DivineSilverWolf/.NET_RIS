using MessagesBetweenManagerAndWorker;
using System.Text.Json;
using System.Text;

namespace ManagerHttp.Requests
{
    public class RequestToWorker(RequestToWorkerConfig config) : IRequest
    {
        private readonly string _url = config.Url;
        private readonly string _mediaType = config.MediaType;

        public bool Request(int maxLength, string hash, string id)
        {
            try
            {
                using var httpClient = new HttpClient();
                var json = JsonSerializer.Serialize(new HashCodeMessage(maxLength, hash, id));
                HttpContent content = new StringContent(json, Encoding.UTF8, _mediaType);
                using HttpResponseMessage response = httpClient.PostAsync(_url, content).Result;
                response.EnsureSuccessStatusCode();
                string responseBody = response.Content.ReadAsStringAsync().Result;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

}
