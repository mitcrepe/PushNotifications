using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebClient.Areas.Api.Models
{
    public class PushSubscriptionModel
    {
        public string Endpoint { get; set; }
        public PushSubscriptionKeys Keys { get; set; }
    }

    public class PushSubscriptionKeys
    {
        public string Auth { get; set; }
        public string P256DH { get; set; }
    }
}
