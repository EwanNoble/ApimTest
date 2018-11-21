using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApimTestClient.Models
{
    public class IndexViewModel
    {
        public IndexViewModel(string url)
        {
            Url = url;
        }

        public string Url { get; set; }
        public string Text { get; set; } = "";
    }
}
