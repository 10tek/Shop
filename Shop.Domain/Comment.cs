using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Domain
{
    public class Comment : Entity
    {
        public User User { get; set; }
        public Item Item { get; set; }
        public int Rating { get; set; }
        public string Text { get; set; }
    }
}
