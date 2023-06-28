using TabloidMVC.Models;
using System.Collections.Generic;

namespace TabloidMVC.Repositories
{
    public interface IUserProfileRepository
    {
        UserProfile GetByEmail(string email);

        List<UserProfile> GetAll();

        UserProfile GetById(int id);

        void DeactivateUser(int id);
    }
}