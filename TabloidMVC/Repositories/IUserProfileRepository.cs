using TabloidMVC.Models;
using System.Collections.Generic;

namespace TabloidMVC.Repositories
{
    public interface IUserProfileRepository
    {
        UserProfile GetByEmail(string email);

        List<UserProfile> GetAll();

        List<UserProfile> AllAdmins();

        List<UserProfile> GetDeactive();

        UserProfile GetById(int id);
        void Register(UserProfile userProfile);

        void DeactivateUser(int id);

        void ReactivateUser(int id);

        void Edit(UserProfile profile);
    }
}