namespace ManagerHttp.ResponseBodies
{
    public class UniqueIdentifierForUser
    {
        public string RequestId { get; set; }

        public UniqueIdentifierForUser()
        {
            this.RequestId = Guid.NewGuid().ToString();
        }
    }
}
