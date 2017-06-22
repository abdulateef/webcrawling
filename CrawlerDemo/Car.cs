using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerDemo
{
    [DebuggerDisplay("{Model}, {Price}")]
    class Car
    {

        public string Model { get; set; }
        public string Price { get; set; }
        public string Link { get; set; }
        public string ImageUrl { get; set; }

        
    }
}
