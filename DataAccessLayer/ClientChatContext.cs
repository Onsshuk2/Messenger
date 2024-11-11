using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Data_access_layer.Entities;

namespace DataAccessLayer
{
    public class ClientChatContext : DbContext
    {
        public ClientChatContext() { }

        public ClientChatContext(DbContextOptions<ClientChatContext> options) : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }

        private readonly string connection = "workstation id=GordoMessanger.mssql.somee.com;packet size=4096;user id=Grinchik_SQLLogin_1;pwd=9bfpecft4c;data source=GordoMessanger.mssql.somee.com;persist security info=False;initial catalog=GordoMessanger;TrustServerCertificate=True";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Friends)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "ClientFriends",
                    j => j.HasOne<Client>().WithMany().HasForeignKey("FriendId"),
                    j => j.HasOne<Client>().WithMany().HasForeignKey("ClientId"));
        }
    }
}