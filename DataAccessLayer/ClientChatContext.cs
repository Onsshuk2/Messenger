using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_access_layer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace DataAccessLayer
{
    public class ClientChatContext : DbContext
    {
        ClientChatContext(DbContextOptions<ClientChatContext> options) : base(options)
        {
        }
        public DbSet<Client> Clients { get; set; }
        private string connection = ConfigurationManager.ConnectionStrings["ClientChatConnection"].ConnectionString;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connection);
        }
      


    }
}