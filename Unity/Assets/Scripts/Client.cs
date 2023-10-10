using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;

public class Client : MonoBehaviour
{
    EventBasedNetListener listener;
    NetManager client;

    void Start()
    {
        listener = new EventBasedNetListener();
        client = new NetManager(listener);
        client.Start();
        client.Connect("localhost", 10515, "ExampleGame");
        //listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod, channel) =>
        //{
        //    //Console.WriteLine("We got: {0}", dataReader.GetString(100));
        //    //Debug.Log("We got: {0}", dataReader.GetString(100));
        //    dataReader.Recycle();
        //};

        //while (!Console.KeyAvailable)
        //{
        //    client.PollEvents();
        //    Thread.Sleep(15);
        //}

        //client.Stop();
    }

    void Update()
    {
        client.PollEvents();
    }

    void OnDestroy()
    {
        client.Stop();
    }
}
