using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WorldTests.BLL.Interfaces;
using WorldTests.BLL.Services;
using WorldTests.BLL.Utilities;
using WorldTests.DAL;
using WorldTests.DAL.Entities;
using WorldTests.DAL.Interfaces;
using WorldTests.DAL.Repository;
using WorldTests.Primitive.Models;
using System.Text.Json;
using WorldTests.Primitive;

namespace WorldTests.Server
{
    public class Program
    {
        private const long BUFFERSIZE = 1024;

        private static TcpListener _server;
        private static IUserService _userService;
        private static ITestService _testService;

        static void Main(string[] args)
        {
            var initializerServices = new ServicesInitializer();
            initializerServices.InitializeUserService(out _userService);
            initializerServices.InitializeTestService(out _testService);

            var address = ConfigurationManager.AppSettings["ServerIPAddress"];
            var port = ConfigurationManager.AppSettings["ServerPort"];
            if (int.TryParse(port, out var portInt) == false)
            {
                portInt = 3100;
            }

            ConfigureServer(address, portInt);

            try
            {
                _server.Start();
                Console.WriteLine($"Server is running");

                Console.WriteLine("Server -> waiting for clients");
                while (true)
                {
                    try
                    {
                        var tcpClient = _server.AcceptTcpClient();
                        var client = new Client(tcpClient, BUFFERSIZE);
                        Task.Run(() => WorkWithClient(client));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Server -> client connect error: {e.Message}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Server running error:: {e.Message}");
            }
            finally
            {
                _server.Stop();
                Console.WriteLine("Server stopped");
            }
        }

        static void ConfigureServer(string address, int port)
        {
            try
            {
                var ip = IPAddress.Parse(address);
                _server = new TcpListener(ip, port);
            }
            catch (Exception e)
            {
                Console.WriteLine($"IP Error: {e.Message}");
            }
        }

        static void WorkWithClient(Client client)
        {
            while (client.TcpClient.Connected)
            {
                try
                {
                    int countOfBytes = 0;
                    string clientRequest = "";
                    do
                    {
                        countOfBytes = client.NetworkStream.Read(client.Buffer);
                        clientRequest += Encoding.UTF8.GetString(client.Buffer, 0, countOfBytes);
                    } while (countOfBytes == 1024);

                    var communication = JsonSerializer.Deserialize<СommunicationServerClient>(clientRequest);
                    var serverCommunication = new ServerCommunication(_userService, _testService);
                    switch (communication.Command)
                    {
                        case Command.Login:
                            serverCommunication.ClientLoginAnswer(client, communication);
                            break;
                        case Command.Register:
                            serverCommunication.ClientRegisterAnswer(client, communication);
                            break;
                        case Command.GetTests:
                            serverCommunication.ClientGetTestsAnswer(client, communication);
                            break;
                        case Command.SaveTest:
                            serverCommunication.SaveTest(client, communication);
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Server -> work with client error: {e.Message}");
                    client.Stop();
                }
            }
        }
    }
}
