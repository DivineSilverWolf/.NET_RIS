using ManagerHttp.ResponseBodies;
using MongoDB.Bson.Serialization.Attributes;
namespace ManagerHttpV2
{
    [BsonIgnoreExtraElements]
    public class ProjectionTask(string userId, char[] LetterCheckArray)
    {
        public char[] LetterCheckArray { get; set; } = LetterCheckArray;

        public string UserId { get; set; } = userId;

        public DateTime SendTime { get; set; } = DateTime.UtcNow;
        public string? Word { get; set; }
        public STATUS Status { get; set; } = STATUS.IN_PROGRESS;
    }
}
