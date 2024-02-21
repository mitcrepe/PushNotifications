using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications
{

	[Serializable]
	public class SubscriptionNotFoundException : Exception
	{
		public SubscriptionNotFoundException() { }
		public SubscriptionNotFoundException(string message) : base(message) { }
		public SubscriptionNotFoundException(string message, Exception inner) : base(message, inner) { }
		protected SubscriptionNotFoundException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
