using Data_access_layer.Entities;

namespace Data_access_layer
{
    public class Database
    {
        public void AddClient(string nickName,string passsword, string domain)
        {
            ClientChatContext context = new ClientChatContext();
            Client client = new Client
            {
                NickName = nickName,
                Password = passsword,
                Domain = domain
            };
            context.Clients.Add(client);
            context.SaveChanges();
        }
    }
}
