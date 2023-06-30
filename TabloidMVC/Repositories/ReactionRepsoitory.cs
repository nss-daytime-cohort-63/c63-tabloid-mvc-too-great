using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public class ReactionRepsoitory : BaseRepository, IReactionRepository
    {
        public ReactionRepsoitory(IConfiguration configuration) : base(configuration) { }

        public List<Reaction> GetAll()
        {
            return null;
        }
    }
}
