using Data_access_layer.Entities;
using System.Collections.Generic;

namespace Data_access_layer
{
    public class Repository
    {
        private ClientChatContext context = new ClientChatContext();
        public void CreateClient(string nickName,string passsword, string domain)
        {
            
            Client client = new Client
            {
                NickName = nickName,
                Password = passsword,
                Domain = domain
            };
            context.Clients.Add(client);
            context.SaveChanges();
        }
        // for commit
        public List<Client> GetClients() 
        {
            List < Client > clientList= new List < Client >();
            foreach ( Client client in context.Clients )
            {
                clientList.Add( client );
            }
            return clientList;
        }
        public void UpdateClient( Client client ) 
        {
            foreach(var current_client in context.Clients) 
            {
                if (client.Domain == current_client.Domain)
                {
                    context.Clients.Remove(current_client);
                }
            }
            context.Clients.Add(client);
            context.SaveChanges();
        }
        public void DeleteClient(string nickName)
        {
            foreach (var client in context.Clients) 
            {
                if (client.NickName == nickName)
                {
                    context.Clients.Remove(client);
                }
            }
        }
    }
}
