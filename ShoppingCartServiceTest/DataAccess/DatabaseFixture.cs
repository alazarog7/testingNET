using MongoDB.Driver;
using System;
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

        public void Dispose()
        {
            WslCommand("docker rm -f int_test");
        }
    }
}
