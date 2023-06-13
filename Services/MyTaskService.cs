using Task.Interfaces;
using System.Text.Json;

namespace Task.Services
{
    using Task.Models;
    public class TaskService : ITaskService
    {
        List<Task> tasks { get; }

        private IWebHostEnvironment webHost;
        private string filePath;
        public TaskService(IWebHostEnvironment webHost)
        {
            this.webHost = webHost;
            this.filePath = Path.Combine(webHost.ContentRootPath, "Data", "task.json");
            using (var jsonFile = File.OpenText(filePath))
            {
                tasks = JsonSerializer.Deserialize<List<Task>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }

        private void saveToFile()
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(tasks));
        }

        public List<Task> GetAll(int UserId) => tasks.Where(u => u.UserId == UserId).ToList();


        public Task? Get(int Id, int userId) => tasks.FirstOrDefault(t => t.Id == Id && t.UserId == userId);

        public void Post(Task t)
        {
            t.Id = tasks[tasks.Count()-1].Id + 1;
            tasks.Add(t);
            saveToFile();
        }

        public void Delete(int id, int userId)
        {

            var task = Get(id, userId);
            tasks.Remove(task);
            saveToFile();
        }

        public bool Update(Task t)
        {
            var item = tasks.Find(task => t.Id == task.Id);
            var index = tasks.FindIndex(task => task.Id == t.Id);
            t.UserId = item.UserId;
            if (index == -1)
                return false;
            tasks[index] = t;
            saveToFile();
            return true;
        }

        public int Count => tasks.Count();



    }

}