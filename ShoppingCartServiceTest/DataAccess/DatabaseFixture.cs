using MongoDB.Bson;
using MongoDB.Driver;
using ShoppingCartService.Controllers.Models;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingCartServiceTest.DataAccess
{
    public class DatabaseFixture : IDisposable
    {
        public DatabaseFixture()
        {
            if (DockerStart())
            {
                WaitForMongoDbConnection("mongodb://127.0.0.1:1111", "ShoppingCartDb");
                FillDatabase();
            }
        }

        private bool WslCommand(string command)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "wsl",
                    Arguments = command,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };

            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                return false;
            }

            return true;
        }

        private bool DockerStart()
        {
            return WslCommand("docker run -d -p 1111:27017 --name int_test mongo");
        }

        private bool WaitForMongoDbConnection(string connectionString, string dbName)
        {
            var probeTask = Task.Run(() =>
            {
                var isAlive = false;
                var client = new MongoClient(connectionString);

                for (var i = 0; i < 3000; i++)
                {
                    client.GetDatabase(dbName);
                    var server = client.Cluster.Description.Servers.FirstOrDefault();
                    isAlive = server != null &&
                             server.HeartbeatException == null &&
                             server.State == MongoDB.Driver.Core.Servers.ServerState.Connected;

                    if (isAlive)
                    {
                        break;
                    }

                    Thread.Sleep(100);
                }

                return isAlive;
            });

            probeTask.Wait();

            return probeTask.Result;
        }


        public void FillDatabase()
        {
            var client = new MongoClient("mongodb://127.0.0.1:1111");
            var database = client.GetDatabase("ShoppingCartDb");
            var carts = database.GetCollection<Cart>("ShoppingCart");

            var cartList = new List<Cart>
            {
                new Cart()
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    CustomerId = Guid.NewGuid().ToString(),
                    CustomerType = CustomerType.Standard,
                    ShippingMethod = ShippingMethod.Standard,
                    ShippingAddress = new Address()
                    {
                        Country = "Bolivia",
                        City = "La Paz",
                        Street = "Calle 1"
                    },
                    Items = new List<Item>()
                    {
                        new Item()
                        {
                            ProductId = Guid.NewGuid().ToString(),
                            ProductName = "Test2",
                            Price = 10,
                            Quantity = 1,
                        }
                    }
                },
                 new Cart()
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    CustomerId = Guid.NewGuid().ToString(),
                    CustomerType = CustomerType.Standard,
                    ShippingMethod = ShippingMethod.Standard,
                    ShippingAddress = new Address()
                    {
                        Country = "Bolivia",
                        City = "La Paz",
                        Street = "Calle 1"
                    },
                    Items = new List<Item>()
                    {
                        new Item()
                        {
                            ProductId = Guid.NewGuid().ToString(),
                            ProductName = "Test3",
                            Price = 10,
                            Quantity = 1,
                        }
                    }
                }
            };

            carts.InsertMany(cartList);
        }

        public void Dispose()
        {
            WslCommand("docker rm -f int_test");
        }
    }
}
