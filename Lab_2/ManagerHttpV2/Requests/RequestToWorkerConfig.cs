namespace ManagerHttp.Requests
{
    public class RequestToWorkerConfig
    {
        public string HttpUrl { get; set; }
        public string MediaType { get; set; }
        public string QueueUrl { get; set; }

        public string Alphabet { get; set; }
        public int TaskCountForOneWord { get; set; }
    }
}
