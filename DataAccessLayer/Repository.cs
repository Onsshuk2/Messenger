using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Data_access_layer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class Repository
    {
        private readonly ClientChatContext _context;

        public Repository(ClientChatContext context)
        {
            _context = context;
        }

        // Метод для створення клієнта з перевіркою на унікальність імені
        public bool RegisterClient(string nickName, string password)
        {
            if (IsNicknameTaken(nickName) || !IsPasswordValid(password))
                return false;

            var client = new Client
            {
                NickName = nickName,
                Password = password
            };

            _context.Clients.Add(client);
            _context.SaveChanges();
            return true;
        }

        // Перевірка, чи зайняте ім'я користувача
        public bool IsNicknameTaken(string nickName)
        {
            return _context.Clients.Any(c => c.NickName == nickName);
        }

        // Перевірка на валідність пароля (мінімум 8 символів, букви і цифри, без пробілів)
        public bool IsPasswordValid(string password)
        {
            return password.Length >= 8 &&
                   Regex.IsMatch(password, @"[A-Za-z]") &&
                   Regex.IsMatch(password, @"[0-9]") &&
                   !password.Contains(" ");

        }

        // Метод для автентифікації користувача (логін)
        public bool AuthenticateClient(string nickName, string password)
        {
            return _context.Clients.Any(c => c.NickName == nickName && c.Password == password);
        }

        // Інші методи для управління клієнтами
        public List<Client> GetClients() => _context.Clients.ToList();
    }
}
