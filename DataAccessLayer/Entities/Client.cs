﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_access_layer.Entities
{
    public class Client
    {
        public int ClientId { get; set; }
        public string NickName { get; set; }
        public string Password { get; set; }
        //for commit
        //for commit

        public ICollection<Client> Friends { get; set; } = new List<Client>();
    }
}
