
namespace User.Interfaces
{
    using User.Models;
    public interface IUserService
    {
        List<User>? GetAll();
        User Get(int id);
        void Post(User t);
        void Delete(int id);
    }
}
