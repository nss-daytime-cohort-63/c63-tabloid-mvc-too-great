using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TabloidMVC.Models;
namespace TabloidMVC.Repositories
{
    public class PostReactionRepository : BaseRepository, IPostReactionRepository
    {
        public PostReactionRepository(IConfiguration configuration) : base(configuration) { }

        public List<PostReaction> GetAllByPost() {
            return null;
        }
    }
}
