using MessagesBetweenManagerAndWorker;
using System.Text.Json;
using System.Text;

namespace WorkerHttp.Requests
{
    public class RequestToManager(RequestToManagerConfig config) : IRequest
    {
        private readonly string _url = config.Url;
        private readonly string _mediaType = config.MediaType;
        public void SendWordToManager(string word, HashCodeMessage hash, bool flag)
        {
            try
            {
                HttpClientHandler clientHandler = new()
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                };
                HttpClient httpClient = new HttpClient(clientHandler);
                var json = JsonSerializer.Serialize(new MessageForDecryptedWord(word, hash.IdValue!, flag));
                HttpContent content = new StringContent(json, Encoding.UTF8, _mediaType);
                Console.WriteLine(json);
                using HttpResponseMessage response = httpClient.PatchAsync(_url, content).Result;
                response.EnsureSuccessStatusCode();
                string responseBody = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
