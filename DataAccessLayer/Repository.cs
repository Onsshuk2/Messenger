using Data_access_layer.Entities;
using System.Collections.Generic;

namespace DataAccessLayer
{
    public class Repository
    {
        private readonly ClientChatContext _context;
        public Repository(ClientChatContext context) 
        {
            _context = context;
        }

        public void CreateClient(string nickName, string password, string domain)
        {
           
            Client client = new Client
            {
                NickName = nickName,
                Password = password,
                Domain = domain
            };
            _context.Clients.Add(client);
            _context.SaveChanges();
        }
        // for commit
        public List<Client> GetClients()
        {
            List<Client> clientList = new List<Client>();
            foreach (Client client in _context.Clients)
            {
                clientList.Add(client);
            }
            return clientList;
        }
        public void UpdateClient(Client client)
        {
            foreach (var current_client in _context.Clients)
            {
                if (client.Domain == current_client.Domain)
                {
                    _context.Clients.Remove(current_client);
                }
            }
            _context.Clients.Add(client);
            _context.SaveChanges();
        }
        public void DeleteClient(string nickName)
        {
            foreach (var client in _context.Clients)
            {
                if (client.NickName == nickName)
                {
                    _context.Clients.Remove(client);
                    
                }
            }
            _context.SaveChanges();
        }
    }
}