namespace ManagerHttp.Requests
{
    public interface IRequest
    {
        public bool Request(int maxLength, string hash, string id);
    }
}
