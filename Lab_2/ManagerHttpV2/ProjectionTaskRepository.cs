using ManagerHttp.Requests;
using ManagerHttp.ResponseBodies;
using ManagerHttpV2;
using ManagerHttpV2.DBConfig;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Text.Json;

namespace ManagerHttp
{
    public class ProjectionTaskRepository
    {
         private readonly IMongoCollection<ProjectionTask> ProjectionTasks;

        public ProjectionTaskRepository(IOptions<DBConfig> config)
        {
            var client = new MongoClient(config.Value.Url);
            var database = client.GetDatabase(config.Value.DBName);
            ProjectionTasks = database.GetCollection<ProjectionTask>("projectionTasks");
            var filter = Builders<ProjectionTask>.Filter.Empty; // пустой фильтр, чтобы выбрать все документы
       //     ProjectionTasks.DeleteMany(filter);
            var documents = ProjectionTasks.Find(filter).ToList();

            foreach (var document in documents)
            {
                Console.WriteLine(JsonSerializer.Serialize(document));
            }
        }
        public List<ProjectionTask> SearchFromUserId(string userID)
        {
            var filter = Builders<ProjectionTask>.Filter.Eq("UserId", userID);
            return ProjectionTasks.Find(filter).ToList();
        }
        public ProjectionTask SearchFromLetterCheckArray(List<ProjectionTask> projectionTasksFromUserId, char[] LetterCheckArray)
        {
            return projectionTasksFromUserId.Where(task => task.LetterCheckArray.ToString() == LetterCheckArray.ToString()).FirstOrDefault();
        }
        public void UpdateTaskStatus(string userId, char[] LetterCheckArray, STATUS newStatus)
        {
            var filter = Builders<ProjectionTask>.Filter.Eq("UserId", userId) & Builders<ProjectionTask>.Filter.Eq("LetterCheckArray", LetterCheckArray);
            var update = Builders<ProjectionTask>.Update.Set("Status", newStatus);
            ProjectionTasks.UpdateOne(filter, update);
        }
        public void UpdateTaskWord(string userId, char[] LetterCheckArray, string word)
        {
            var filter = Builders<ProjectionTask>.Filter.Eq("UserId", userId) & Builders<ProjectionTask>.Filter.Eq("LetterCheckArray", LetterCheckArray);
            var update = Builders<ProjectionTask>.Update.Set("Word", word);
            ProjectionTasks.UpdateOne(filter, update);
        }
        public void UpdateTaskWordAndStatus(string userId, char[] LetterCheckArray, STATUS newStatus, string word)
        {
            var filter = Builders<ProjectionTask>.Filter.Eq("UserId", userId) & Builders<ProjectionTask>.Filter.Eq("LetterCheckArray", LetterCheckArray);
            var update = Builders<ProjectionTask>.Update.Set("Status", newStatus).Set("Word", word);
            ProjectionTasks.UpdateOne(filter, update);
        }
        public void AddProjectionTask(ProjectionTask task)
        {
            ProjectionTasks.InsertOne(task);
        }
        public ProjectionTask SearchFromLetterCheckArray(string userID, char[] LetterCheckArray) {
            List<ProjectionTask> list = SearchFromUserId(userID);
            return SearchFromLetterCheckArray(list, LetterCheckArray);
        }
    }
}
