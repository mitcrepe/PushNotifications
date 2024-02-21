namespace Notifications
{
    public class Subscription
    {
        public string Username { get; set; }

        // Address of push service with user identification as GET parameter.
        public string Endpoint { get; set; }
        

        // Change to Dictionary to support multiple INotifier types. "Keys" collection for example.

        // ECDH key, used for message encryption
        // Key pair is generated at user agent, private key is stored locally and public key is sent to server.
        // Use that key to encrypt messages so only user agent can read them.
        public string SharedSecret { get; set; }

        // auth - Authenticates messages to push service
        public string AuthenticationKey { get; set; }
    }
}
