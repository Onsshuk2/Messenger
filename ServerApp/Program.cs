using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Data_access_layer.Entities;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GordoChatServer
{
    public class ChatServer
    {
       
        private TcpListener _listener;
        private List<TcpClient> _clients;

        public ChatServer(int port)
        {
            _listener = new TcpListener(IPAddress.Any, port);
            _clients = new List<TcpClient>();
        }

        public async Task StartAsync()
        {
            _listener.Start();
            Console.WriteLine("Server started...");
            while (true)
            {
                var client = await _listener.AcceptTcpClientAsync();
                _clients.Add(client);
                Console.WriteLine("Client connected.");
                _ = Task.Run(() => HandleClientAsync(client));

            }
            
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            var stream = client.GetStream();
            var buffer = new byte[1024];
            while (true)
            {
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;
                var message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Received: {message}");
                await BroadcastAsync(message, client);
            }
            client.Close();
        }

        private async Task BroadcastAsync(string message, TcpClient sender)
        {
            foreach (var client in _clients)
            {
                if (client != sender)
                {
                    var stream = client.GetStream();
                    var data = Encoding.UTF8.GetBytes(message);
                    await stream.WriteAsync(data, 0, data.Length);
                }
            }
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {

            int port = 4040;
            var server = new ChatServer(port);
            await server.StartAsync();
            
        }
    }
}
