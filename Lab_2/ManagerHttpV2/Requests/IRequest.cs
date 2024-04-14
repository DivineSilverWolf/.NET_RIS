namespace ManagerHttp.Requests
{
    public interface IRequest
    {
        public (bool, string[]) Request(int maxLength, string hash, string id);
    }
}
