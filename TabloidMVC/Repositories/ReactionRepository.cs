using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public class ReactionRepository : BaseRepository, IReactionRepository
    {
        public ReactionRepository(IConfiguration configuration) : base(configuration) { }

        public List<Reaction> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"Select * From Reaction";
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Reaction> reactions = new List<Reaction>();
                        while (reader.Read())
                        {
                            Reaction reaction = new Reaction()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("name")),
                               
                            };
                            if (!reader.IsDBNull(reader.GetOrdinal("ImageLocation")))
                            {
                                reaction.ImageLocation = reader.GetString(reader.GetOrdinal("ImageLocation"));

                            }
                            reactions.Add(reaction);
                        }
                        return reactions;
                    }
                }
            }
        }
    }
}
