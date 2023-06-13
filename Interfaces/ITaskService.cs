
using System.Collections.Generic;

namespace Task.Interfaces
{
    using Task.Models;
    public interface ITaskService
    {
        List<Task>? GetAll(int userId);
        Task Get(int id, int userId);
        void Post(Task t);
        void Delete(int id, int userId);
        bool Update(Task t);
        int Count { get; }
    }
}
