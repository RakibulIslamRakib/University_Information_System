using University_Information_System.Models;

namespace University_Information_System.Services.ServiceInterfaces
{
    public interface IUserService
    {
        string GetUserId();
        bool IsAuthenticated();
    }
}