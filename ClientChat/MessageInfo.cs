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
            public ImageSource ImageSource { get; set; } 

     
            public MessageInfo(string text, DateTime timestamp)
            {
                Text = text;
                Timestamp = timestamp;
                ImageSource = null;
            }

      
            public MessageInfo(DateTime timestamp, ImageSource imageSource)
            {
                Text = null;
                Timestamp = timestamp;
                ImageSource = imageSource;
            }
        

        public override string ToString()
        {
           
            return $"{Text}\t|{Timestamp.ToString("HH:mm")}";


        }
    }
}
