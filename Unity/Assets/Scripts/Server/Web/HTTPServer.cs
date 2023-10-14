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
        //����������
        using (var httpListener = new HttpListener())
        {
            //������·��
            httpListener.Prefixes.Add("http://localhost:8000/");
            //������������
            httpListener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            //��ʼ����
            httpListener.Start();
            while (true)
            {
                Thread.Sleep(0);

                //�ȴ������������ܵ�����ʱ���أ����������̣߳�ֱ�����󵽴�
                var context = httpListener.GetContext();
                // ���������ȴ�����


                //ȡ������Ķ���
                HttpListenerRequest request = context.Request;
                //Debug.LogFormat("{0} {1} HTTP/1.1", request.HttpMethod, request.RawUrl);
                //Debug.LogFormat("Accept: {0}", string.Join(",", request.AcceptTypes));
                //Debug.LogFormat("Accept-Language: {0}", string.Join(",", request.UserLanguages));
                //Debug.LogFormat("User-Agent: {0}", request.UserAgent);
                //Debug.LogFormat("Accept-Encoding: {0}", request.Headers["Accept-Encoding"]);
                //Debug.LogFormat("Connection: {0}", request.KeepAlive ? "Keep-Alive" : "close");
                //Debug.LogFormat("Host: {0}", request.UserHostName);
                //Debug.LogFormat("Pragma: {0}", request.Headers["Pragma"]);

                // ȡ�û�Ӧ����
                HttpListenerResponse response = context.Response;

                // ���û�Ӧͷ�����ݣ����ȣ�����
                response.ContentEncoding = Encoding.UTF8;
                //response.ContentType = "text/plain;charset=utf-8";
                //response.ContentType = "text/html;charset=utf-8"; //�վ���html
                Debug.Log($"ContentType: {response.ContentType}");

                var path = @"C:\Users\Hasee\Desktop\";
                //���ʵ��ļ���
                var fileName = request.Url.LocalPath;
                Debug.Log($"fileName: {fileName}");

                //��ȡ�ļ�����
                var buff = File.ReadAllBytes(path + fileName);
                response.ContentLength64 = buff.Length;

                // �����Ӧ����
                Stream output = response.OutputStream;
                output.Write(buff, 0, buff.Length);
                // ����ر������
                output.Close();
            }
        }
    }

    // �ٷ�
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
                Debug.LogError("InputStream �ǿ�����");
            }

            // Obtain a response object.
            HttpListenerResponse response = context.Response;
            //Debug.Log($"RawUrl: {request.RawUrl}");
            //Debug.Log($"HttpMethod: {request.HttpMethod.Length}"); //0
            Debug.Log($"QueryString: {request.QueryString.Count}"); //����GET����URL�еĲ���

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
