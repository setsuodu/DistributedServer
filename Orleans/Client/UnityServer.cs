using LiteNetLib.Utils;
using LiteNetLib;

namespace Client
{
    public class UnityServer
    {
        public void Start()
        {
            EventBasedNetListener listener = new EventBasedNetListener();
            NetManager server = new NetManager(listener);
            server.Start(10515);

            listener.ConnectionRequestEvent += request =>
            {
                if (server.ConnectedPeersCount < 10 /* max connections */)
                    request.AcceptIfKey("ExampleGame");
                else
                    request.Reject();
            };

            listener.PeerConnectedEvent += peer =>
            {
                Console.WriteLine("We got connection: {0}", peer.EndPoint); // Show peer ip
                NetDataWriter writer = new NetDataWriter();                 // Create writer class
                writer.Put("Hello client!");                                // Put some string
                peer.Send(writer, DeliveryMethod.ReliableOrdered);             // Send with reliability
            };

            while (!Console.KeyAvailable)
            {
                server.PollEvents();
                Thread.Sleep(15);
            }
            server.Stop();
        }
    }
}
