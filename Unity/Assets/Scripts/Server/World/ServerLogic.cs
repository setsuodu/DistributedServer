using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;

public class ServerLogic : MonoBehaviour, INetEventListener
{
    NetManager server;

    void Start()
    {
        server = new NetManager(this);
        server.Start(10515);
    }

    void Update()
    {
        server.PollEvents();
    }

    void OnDestroy()
    {
        server.Stop();
    }

    public void OnConnectionRequest(ConnectionRequest request)
    {
        if (server.ConnectedPeersCount < 10 /* max connections */)
            request.AcceptIfKey("ExampleGame");
        else
            request.Reject();
    }

    public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
    {
        Debug.LogError($"{endPoint}: {socketError.ToString()}");
    }

    public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {
        Debug.Log($"{peer.EndPoint}: {latency}");
    }

    public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
    {
        throw new NotImplementedException();
    }

    public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {
        throw new NotImplementedException();
    }

    public void OnPeerConnected(NetPeer peer)
    {
        Debug.Log($"We got connection: {peer.EndPoint}"); // Show peer ip
        NetDataWriter writer = new NetDataWriter();                 // Create writer class
        writer.Put("Hello client!");                                // Put some string
        peer.Send(writer, DeliveryMethod.ReliableOrdered);
    }

    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        throw new NotImplementedException();
    }
}