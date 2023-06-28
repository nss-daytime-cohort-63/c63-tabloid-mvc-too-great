using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using TabloidMVC.Models;
using TabloidMVC.Utils;

namespace TabloidMVC.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(IConfiguration config) : base(config) { }

       public List<UserProfile> GetAll()
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"select userProfile.id as Id,FirstName,LastName,
                                        DisplayName,Email,CreateDateTime,
                                        UserTypeId,UserType.Name as Type 
                                        from UserProfile join UserType 
                                        on UserTypeId = userType.Id 
                                        where isActive = 1
                                        order by DisplayName
                                       ";
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<UserProfile> Users = new List<UserProfile>();
                        while (reader.Read())
                        {
                            UserProfile User = new UserProfile
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                DisplayName=reader.GetString(reader.GetOrdinal("DisplayName")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                CreateDateTime =reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                                UserType = new UserType
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                    Name=reader.GetString(reader.GetOrdinal("Type"))
                                }

                            };
                            Users.Add(User);
                        }
                        return Users;
                    }
                }
            }
        }

        public UserProfile GetById(int id)
        {
            using(SqlConnection conn = Connection) { 
            
            conn.Open();

                using (SqlCommand cmd =conn.CreateCommand())
                {
                    cmd.CommandText = @"select userProfile.id as Id,FirstName,LastName,
                                        DisplayName,Email,CreateDateTime,isActive,
                                        UserTypeId,UserType.Name as Type,ImageLocation 
                                        from UserProfile join UserType 
                                        on UserTypeId = userType.Id Where userProfile.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            UserProfile User = new UserProfile()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                                UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("isActive")),
                            UserType = new UserType
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                    Name = reader.GetString(reader.GetOrdinal("Type"))
                                }
                            };

                            if (!reader.IsDBNull(reader.GetOrdinal("ImageLocation")))
                           {
                                User.ImageLocation = reader.GetString(reader.GetOrdinal("ImageLocation"));
                                
                            }
                            return User;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
        public UserProfile GetByEmail(string email)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT u.id,u.isActive, u.FirstName, u.LastName, u.DisplayName, u.Email,
                              u.CreateDateTime, u.ImageLocation, u.UserTypeId,
                              ut.[Name] AS UserTypeName
                         FROM UserProfile u
                              LEFT JOIN UserType ut ON u.UserTypeId = ut.id
                        WHERE email = @email";
                    cmd.Parameters.AddWithValue("@email", email);

                    UserProfile userProfile = null;
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        userProfile = new UserProfile()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            ImageLocation = DbUtils.GetNullableString(reader, "ImageLocation"),
                            UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("isActive")),
                            UserType = new UserType()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                            },
                        };
                    }

                    reader.Close();

                    return userProfile;
                }
            }
        }

        public void DeactivateUser(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"Update UserProfile
                                        Set isActive = 0
                                        Where Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
