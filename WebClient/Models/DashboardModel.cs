using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebClient.Models
{
    public class DashboardModel
    {
        public List<string> AvailableUsers { get; set; }
        public string PublicKey { get; set; }
    }
}
