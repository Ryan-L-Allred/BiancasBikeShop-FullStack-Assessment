using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using BiancasBikeShop.Models;

namespace BiancasBikeShop.Repositories
{
    public class BikeRepository : IBikeRepository
    {
        private SqlConnection Connection
        {
            get
            {
                return new SqlConnection("server=localhost\\SQLExpress;database=BiancasBikeShop;integrated security=true;TrustServerCertificate=true");
            }
        }

        public List<Bike> GetAllBikes()
        {
            using (var conn = Connection)
            {
                conn.Open();
                  using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT b.Id, b.Brand, b.Color, b.OwnerId, b.BikeTypeId,
                           o.Name, o.Address, o.Email, o.Telephone,
                           bt.Name as BikeType
                      FROM Bike b
                           JOIN Owner o ON b.OwnerId = o.Id
                           JOIN BikeType bt ON b.BikeTypeId = bt.Id";
                           
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        var bikes = new List<Bike>();
                        // implement code here... 
                        while (reader.Read())
                        {
                            var bike = new Bike()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Brand = reader.GetString(reader.GetOrdinal("Brand")),
                                Color = reader.GetString(reader.GetOrdinal("Color")),
                                BikeType = new BikeType()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("BikeTypeId")),
                                    Name = reader.GetString(reader.GetOrdinal("BikeType")),
                                },
                                Owner = new Owner()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Address = reader.GetString(reader.GetOrdinal("Address")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    Telephone = reader.GetString(reader.GetOrdinal("Telephone"))
                                },
                            };

                            bikes.Add(bike);
                        }
                        return bikes;
                    }
                }
            }
        }

        public Bike GetBikeById(int id)
        {
           using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT b.Id, b.Brand, b.Color, b.OwnerId, b.BikeTypeId,

                              o.Name, o.Address, o.Email, o.Telephone,
                              bt.Name as BikeTypeName,
                              wo.Id as WorkOrderID, wo.DateInitiated, wo.Description, wo.DateCompleted
                        FROM Bike b
                             JOIN Owner o ON b.OwnerId = o.Id
                             JOIN BikeType bt ON b.BikeTypeId = bt.Id
                              LEFT JOIN WorkOrder wo ON wo.BikeId = b.Id
                              WHERE b.Id = @Id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Bike bike = null;
                        while (reader.Read())
                        {
                          if (bike == null) 
                           { 
                            bike = new Bike()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Brand = reader.GetString(reader.GetOrdinal("Brand")),
                                Color = reader.GetString(reader.GetOrdinal("Color")),
                                Owner = new Owner()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Address = reader.GetString(reader.GetOrdinal("Address")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    Telephone = reader.GetString(reader.GetOrdinal("Telephone"))
                                },
                                BikeType = new BikeType()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("BikeTypeId")),
                                    Name = reader.GetString(reader.GetOrdinal("BikeTypeName")),
                                },
                                WorkOrders = new List<WorkOrder>()
         
                                };
                            }

                            WorkOrder temp = new WorkOrder()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("WorkOrderId")),
                                DateInitiated = reader.GetDateTime(reader.GetOrdinal("DateInitiated")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                DateCompleted = !reader.IsDBNull(reader.GetOrdinal("DateCompleted")) ? reader.GetDateTime(reader.GetOrdinal("DateCompleted")) : null
                            };
                            bike.WorkOrders.Add(temp);

                        }

                        return bike;
                    }
                }
            }
        }

        public int GetBikesInShopCount()
        {
            int count = 0;
            // implement code here... 
            return count;
        }
    }
}
