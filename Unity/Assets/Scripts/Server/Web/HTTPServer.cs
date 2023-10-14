using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
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
            Debug.Log(s);
            listener.Prefixes.Add(s);
        }

        listener.Start();
        Debug.LogFormat("Listening...");



        //Thread t = new Thread(SetWebServer);
        Thread t = new Thread(SimpleListenerExample);
        t.Start();
    }

    void OnDestroy()
    {
        //listener.Stop();
    }

    public void SetWebServer()
    {
        Debug.Log("SetWebServer");
        //建立监听器
        using (var httpListener = new HttpListener())
        {
            //监听的路径
            httpListener.Prefixes.Add("http://localhost:8000/");
            //设置匿名访问
            httpListener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            //开始监听
            httpListener.Start();
            while (true)
            {
                Thread.Sleep(0);

                //等待传入的请求接受到请求时返回，它将阻塞线程，直到请求到达
                var context = httpListener.GetContext();
                // 这里阻塞等待输入


                //取得请求的对象
                HttpListenerRequest request = context.Request;
                //Debug.LogFormat("{0} {1} HTTP/1.1", request.HttpMethod, request.RawUrl);
                //Debug.LogFormat("Accept: {0}", string.Join(",", request.AcceptTypes));
                //Debug.LogFormat("Accept-Language: {0}", string.Join(",", request.UserLanguages));
                //Debug.LogFormat("User-Agent: {0}", request.UserAgent);
                //Debug.LogFormat("Accept-Encoding: {0}", request.Headers["Accept-Encoding"]);
                //Debug.LogFormat("Connection: {0}", request.KeepAlive ? "Keep-Alive" : "close");
                //Debug.LogFormat("Host: {0}", request.UserHostName);
                //Debug.LogFormat("Pragma: {0}", request.Headers["Pragma"]);

                // 取得回应对象
                HttpListenerResponse response = context.Response;

                // 设置回应头部内容，长度，编码
                response.ContentEncoding = Encoding.UTF8;
                //response.ContentType = "text/plain;charset=utf-8";
                //response.ContentType = "text/html;charset=utf-8"; //空就是html
                Debug.Log($"ContentType: {response.ContentType}");

                var path = @"C:\Users\Hasee\Desktop\";
                //访问的文件名
                var fileName = request.Url.LocalPath;
                Debug.Log($"fileName: {fileName}");

                //读取文件内容
                var buff = File.ReadAllBytes(path + fileName);
                response.ContentLength64 = buff.Length;

                // 输出回应内容
                Stream output = response.OutputStream;
                output.Write(buff, 0, buff.Length);
                // 必须关闭输出流
                output.Close();
            }
        }
    }

    // 官方
    public static void SimpleListenerExample()
    {
        while (true)
        {
            // Note: The GetContext method blocks while waiting for a request.
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;

            Debug.Log($"{request.HttpMethod} {request.Url} HTTP/1.1");
            if (request.InputStream == null)
            {
                Debug.LogError("InputStream 是空数据");
            }

            // Obtain a response object.
            HttpListenerResponse response = context.Response;
            //Debug.Log($"RawUrl: {request.RawUrl}");
            //Debug.Log($"HttpMethod: {request.HttpMethod.Length}"); //0
            Debug.Log($"QueryString: {request.QueryString.Count}"); //处理GET请求URL中的参数

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
