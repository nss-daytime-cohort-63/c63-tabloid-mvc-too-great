using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TabloidMVC.Models;
namespace TabloidMVC.Repositories
{
    public class PostReactionRepository : BaseRepository, IPostReactionRepository
    {
        public PostReactionRepository(IConfiguration configuration) : base(configuration) { }

        public List<PostReaction> GetAllByPost(int id) {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"Select * From PostReaction where PostId = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<PostReaction> reactions = new List<PostReaction>();
                        while (reader.Read())
                        {
                            PostReaction reaction = new PostReaction()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                                ReactionId = reader.GetInt32(reader.GetOrdinal("ReactionId")),
                                UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),


                            };
                            reactions.Add(reaction);
                        }
                        return reactions;
                    }
                }
            }

            
        }

        public void Add(PostReaction postReaction)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    Insert Into PostReaction (PostId,ReactionId,UserProfileId)
                    OUTPUT INSERTED.ID
                    VALUES (@postId,@reactionId,@userProfileId);";
                    cmd.Parameters.AddWithValue("@postId", postReaction.PostId);
                    cmd.Parameters.AddWithValue("@reactionId",postReaction.ReactionId);
                    cmd.Parameters.AddWithValue("@userProfileId", postReaction.PostId);
                    int id = (int)cmd.ExecuteScalar();
                    postReaction.Id = id;
                }
            }
        }
    }
}
