using System.Collections.Generic;
using TabloidMVC.Models;
namespace TabloidMVC.Repositories
{
    public interface IPostReactionRepository
    {
        List<PostReaction> GetAllByPost(int id);
        void Add(PostReaction postReaction);
    }
}
