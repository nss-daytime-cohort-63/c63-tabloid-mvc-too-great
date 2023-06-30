using TabloidMVC.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace TabloidMVC.Repositories
{
    public class UserTypeRepository : BaseRepository, IUserTypeRepository
    {
        public UserTypeRepository(IConfiguration config) : base(config) { }

        public List<UserType> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand()) {
                    cmd.CommandText = @"Select * From UserType";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<UserType> types = new List<UserType>();
                        while (reader.Read())
                        {
                            UserType type = new UserType
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                            };
                            types.Add(type);
                        }
                        return types;
                    }
                }
            }
        }
    }
}
