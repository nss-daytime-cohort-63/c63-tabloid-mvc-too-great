using System.Collections.Generic;
using System.Data.SqlTypes;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        public CategoryRepository(IConfiguration config) : base(config) { }
        public List<Category> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "select * from Category order by name";
                    var reader = cmd.ExecuteReader();

                    var categories = new List<Category>();

                    while (reader.Read())
                    {
                        categories.Add(new Category()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                        });
                    }

                    reader.Close();

                    return categories;
                }
            }
        }
        public Category GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "Select * From Category Where Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            Category category = new Category()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("name")),
                            };
                            return category;
                        }
                        else {
                            return null;
                        }
                    }
                }
            }
        }

        public void Update(Category category)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    Update Category
                                    Set [Name] = @name
                                    Where Id = @id";
                    cmd.Parameters.AddWithValue("@name", category.Name);
                    cmd.Parameters.AddWithValue("@id", category.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Add(Category category) {


            using (SqlConnection connection = Connection)
            {
                connection.Open();

                using (SqlCommand  cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"
                    Insert Into Category ([Name])
                    OUTPUT INSERTED.ID
                    VALUES (@name);";
                    cmd.Parameters.AddWithValue("@name", category.Name);
                    int id = (int)cmd.ExecuteScalar();
                    category.Id = id;
                }
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection connection = Connection)
            {
                connection.Open();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"Delete From Category Where Id = @id";
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
