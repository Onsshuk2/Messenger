using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _ClientChat
{
    public class ClientChatDBContext : DbContext
    {
        private const string connectionString = @"Data Source=DESKTOP-A1447MR\SQLEXPRESS;
                                          Initial Catalog=ClientChat;
                                          Integrated Security=True;
                                          TrustServerCertificate=True";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
