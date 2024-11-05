using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ClientChat
{
    public class MessageInfo
    {
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }

        public MessageInfo(string text, DateTime timestamp)
        {
            Text = text;
            Timestamp = timestamp;
        }
    

        public override string ToString()
        {
           
            return $"{Text}\t|{Timestamp.ToString("HH:mm")}";


        }
    }
}
