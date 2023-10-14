using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;

public class HTTPServer : MonoBehaviour
{
    static HttpListener listener = new HttpListener();
    static string[] prefixes = new string[]
    {
        "http://localhost:8080/",
    };

    void Start()
    {
        if (!HttpListener.IsSupported)
        {
            Debug.LogFormat("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
            return;
        }
        // URI prefixes are required,
        // for example "http://contoso.com:8080/index/".
        if (prefixes == null || prefixes.Length == 0)
            throw new ArgumentException("prefixes");

        // Create a listener.
        //HttpListener listener = new HttpListener();
        // Add the prefixes.
        foreach (string s in prefixes)
        {
            listener.Prefixes.Add(s);
            Debug.Log($"Listening on: {s}");
        }

        listener.Start();


        Thread t = new Thread(SimpleListenerExample);
        t.Start();
    }

    // 官方
    public static void SimpleListenerExample()
    {
        while (true)
        {
            // Note: The GetContext method blocks while waiting for a request.
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            //Debug.Log($"{request.HttpMethod} {request.Url}");
            //Debug.Log($"RawUrl: {request.RawUrl}");


            // 处理Json
            if (request.ContentType != null)
            {
                Debug.Log($"Client data content type {request.ContentType}");
                //application/x-www-form-urlencoded（WWWForm）
                //multipart/form-data
                //application/json
            }
            string jsonStr;
            using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
            {
                jsonStr = reader.ReadToEnd();

                // 处理
                switch (request.HttpMethod)
                {
                    case "GET":
                        // 检查URL种的参数，无法获取formdata和json二进制
                        //Debug.Log($"QueryString: {request.QueryString.Count}");
                        //foreach (var item in request.QueryString)
                        //{
                        //    string key = item.ToString();
                        //    string value = request.QueryString[key];
                        //    Debug.Log($"{key} : {value}");
                        //}
                        break;
                    case "POST":
                        switch (request.ContentType)
                        {
                            case "application/x-www-form-urlencoded":
                                break;
                            case "application/json":
                                break;
                        }
                        break;
                }
            }
            Debug.Log($"{request.HttpMethod} : {jsonStr}");


            // Obtain a response object.
            HttpListenerResponse response = context.Response;

            // Construct a response.
            string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            // You must close the output stream.
            output.Close();
            //listener.Stop();
        }
        listener.Stop();
    }
}
