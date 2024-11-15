using Data_access_layer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer
{
    public class Repository
    {
        private readonly ClientChatContext _context;

        public Repository(ClientChatContext context)
        {
            _context = context;
        }
        // Create client method

        public void CreateClient(string nickName, string password)
        {
            Client client = new Client
            {
                NickName = nickName,
                Password = password,
            };
            _context.Clients.Add(client);
            _context.SaveChanges();
        }

        public List<Client> GetClients()
        {
            return _context.Clients.ToList();
        }
        // Update client method
        public void UpdateClient(Client client)
        {
            var existingClient = _context.Clients.Find(client.ClientId);
            if (existingClient != null)
            {
                existingClient.NickName = client.NickName;
                existingClient.Password = client.Password;
                _context.SaveChanges();
            }
        }
        // Delete client method
        public void DeleteClient(string nickName)
        {
            var client = _context.Clients.FirstOrDefault(c => c.NickName == nickName);
            if (client != null)
            {
                _context.Clients.Remove(client);
                _context.SaveChanges();
            }
        }

        // Add friend method
        public void AddFriend(int clientId, int friendId)
        {
            var client = _context.Clients.Include(c => c.Friends).FirstOrDefault(c => c.ClientId == clientId);
            var friend = _context.Clients.Find(friendId);

            if (client != null && friend != null && !client.Friends.Contains(friend))
            {
                client.Friends.Add(friend);
                _context.SaveChanges();
            }
        }

        // Delette friend method
        public void DeleteFriend(int clientId, int friendId)
        {
            var client = _context.Clients.Include(c => c.Friends).FirstOrDefault(c => c.ClientId == clientId);
            var friend = _context.Clients.Find(friendId);

            if (client != null && friend != null && client.Friends.Contains(friend))
            {
                client.Friends.Remove(friend);
                _context.SaveChanges();
            }
        }

        // Method to get all friend of a specific client
        public List<Client> GetAllFriends(int clientId)
        {
            var client = _context.Clients
                .Include(c => c.Friends)
                .FirstOrDefault(c => c.ClientId == clientId);

            return client?.Friends.ToList() ?? new List<Client>();
        }
        // Get id method
        public int GetId(string nickName)
        {
            int id=0;
            List<Client> clients=GetClients();
            foreach (Client client in clients) 
            {
                if (client.NickName == nickName) 
                {
                    id = client.ClientId; break;
                }
            }

            return id;
        }
    }
}