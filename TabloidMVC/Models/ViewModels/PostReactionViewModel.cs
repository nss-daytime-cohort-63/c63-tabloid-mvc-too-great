using System.Collections.Generic;

namespace TabloidMVC.Models.ViewModels
{
    public class PostReactionViewModel
    {
       public Post Post { get; set; }

        public List<Reaction> Reactions { get; set; }

        public List<PostReaction> PostReactions { get; set; }

        public int ReactionId { get; set; }
    }
}
