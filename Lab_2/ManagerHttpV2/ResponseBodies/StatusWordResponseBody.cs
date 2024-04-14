using System.ComponentModel;

namespace ManagerHttp.ResponseBodies
{
    public enum STATUS
    {
        ERROR,
        IN_PROGRESS,
        READY
    }
    public class StatusWordResponseBody(STATUS? Status, string? Data)
    {
        public string? Status { get; } = GetEnumDescription(Status);
        public string? Data { get; } = Data;
        private static string GetEnumDescription(Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length == 0 ? value.ToString() : ((DescriptionAttribute)attributes[0]).Description;
        }
    }
}
