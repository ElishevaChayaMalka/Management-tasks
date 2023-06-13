namespace User.Services
{
    using System.Text.Json;
    using Task.Services;
    using User.Interfaces;
    using User.Models;
    public class UserService : IUserService
    {
        List<User>? users { get; }

        static int nextId = 100;

        private IWebHostEnvironment webHost;
        private string filePath;
        public UserService(IWebHostEnvironment webHost)
        {
            this.webHost = webHost;
            this.filePath = Path.Combine(webHost.ContentRootPath, "Data", "User.json");
            using (var jsonFile = File.OpenText(filePath))
            {
                users = JsonSerializer.Deserialize<List<User>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }
        private void saveToFile()
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(users));
        }

        public List<User> GetAll() => users;

        public User? Get(int Id) => users?.FirstOrDefault(t => t.Id == Id);

        public void Post(User u)
        {
            u.Id = users[users.Count()-1].Id+1;
            users.Add(u);
            saveToFile();
        }

        public void Delete(int id)
        {
            var user = Get(id);
            if (user is null)
                return;
            users.Remove(user);
            saveToFile();
        }
    }

}
