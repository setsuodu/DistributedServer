using System;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using Newtonsoft.Json;

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

    // 官方案例
    public void SimpleListenerExample()
    {
        string text = string.Empty;
        string responseString = string.Empty;

        while (true)
        {
            // 同步阻塞
            Thread.Sleep(1);

            // Note: The GetContext method blocks while waiting for a request.
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            //Debug.Log($"{request.HttpMethod} {request.Url}");


            // 处理数据
            //if (!request.HasEntityBody)
            //{
            //    Debug.Log("No client data was sent with the request."); //GET中没有body
            //    return;
            //}
            using (Stream body = request.InputStream) //使用using关闭流
            {
                using (var reader = new StreamReader(body, request.ContentEncoding))
                {
                    if (request.ContentType != null)
                    {
                        // GET请求没有Content，就没有ContentType
                        Debug.Log($"Client data content type {request.ContentType}");
                        //application/x-www-form-urlencoded（WWWForm）
                        //multipart/form-data
                        //application/json
                    }

                    // 一、提取数据
                    //Debug.Log("Start of client data:");
                    text = reader.ReadToEnd();
                    //Debug.Log("End of client data:");
                    //body.Close();
                    //reader.Close();



                    // URL函数路由
                    // http://localhost:8080/Person/3333?name="test"
                    //Debug.Log($"Url: {request.Url}");
                    //Debug.Log($"Length: {request.Url.Segments.Length}"); //URI分段，3，会隐去参数
                    ///*
                    string className = string.Empty, methodName = string.Empty;
                    int methodPos = request.Url.Segments.Length - 1;
                    int classPos = methodPos - 1;
                    //Debug.Log($"main={classPos}, sub={methodPos}");
                    if (methodPos >= 0)
                    {
                        methodName = request.Url.Segments[methodPos].Replace("/", "");
                        //Debug.Log($"methodName={methodName}");
                    }
                    if (classPos >= 0)
                    {
                        className = request.Url.Segments[classPos].Replace("/", "");
                        //Debug.Log($"className={className}");
                    }
                    Debug.Log($"拼接: {className} / {methodName}");

                    // 读取class和method
                    string[] strParams = request.Url.Segments.Skip(2).Select(s => s.Replace("/", "")).ToArray();
                    for (int i = 0; i < strParams.Length; i++)
                    {
                        Debug.Log($"[{i}] : {strParams[i]}");
                    }

                    //string methodName = request.Url.Segments[1].Replace("/", ""); //Person
                    MethodInfo[] array = this.GetType().GetMethods();
                    var lst = array.Where(mi => mi.GetCustomAttributes(true).Any(attr => attr is Mapping && ((Mapping)attr).Map == methodName));
                    //Debug.Log($"method: {methodName}, count={lst.ToArray().Length}");
                    //>>>>

                    List<object> paramsCache = new List<object>();

                    // 二、解析数据
                    switch (request.HttpMethod)
                    {
                        case "GET":
                            // 解析API
                            // http://localhost:8080/api/relationship/toFriend?token=5e6f2335ac1f858a3a7865392b2192e7&rid=3

                            NameValueCollection qscoll = request.QueryString;

                            // 检查URL种的参数，无法获取formdata和json二进制
                            //Debug.Log($"QueryString: {request.QueryString.Count}");
                            if (qscoll.Count == 0)
                            {
                                // 认为是访问网页
                                // 从本地wwwroot文件夹读取.html文件
                                responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
                            }
                            else
                            {
                                // 认为是访问API，读取参数
                                var dic = Converter.FormToModel(qscoll);
                                string json = JsonConvert.SerializeObject(dic);
                                Debug.Log(json);
                                paramsCache.Add(json);
                            }
                            break;
                        case "POST":
                            switch (request.ContentType)
                            {
                                case "application/x-www-form-urlencoded":
                                    //user=test&pwd=666
                                    responseString = text;
                                    Debug.Log($"form===>>>{text}");
                                    //转JSON
                                    string json = Converter.ConvertJson(text);
                                    Debug.Log($"tojson===>>>{json}");
                                    paramsCache.Add(json);
                                    break;
                                case "application/json":
                                    //{"user":"test", "pwd":666}
                                    responseString = text;
                                    paramsCache.Add(text);
                                    break;
                                default:
                                    Debug.LogError("不支持的格式");
                                    break;
                            }
                            break;
                        default:
                            Debug.LogError("不支持的协议");
                            break;
                    }


                    //<<<<
                    if (lst.Count() == 0)
                    {
                        Debug.LogError("找不到方法");
                    }
                    else
                    {
                        MethodInfo method = lst.First(); //c#中找对应函数
                        //Debug.Log($"c# method: {method.Name}"); //get_XX_Handler(@params)
                        ParameterInfo[] parameters = method.GetParameters(); //c#中找对应函数的参数(Int32 id)

                        object[] @params = paramsCache.ToArray();
                        object ret = method.Invoke(this, @params); //执行[Mapping]函数
                    }
                    //Debug.Log($"method: {method != null}");

                }
            }
            //Debug.Log($"{request.HttpMethod} : {text}");


            // 三、返回数据
            // Obtain a response object.
            HttpListenerResponse response = context.Response;
            // Construct a response.
            //string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            // You must close the output stream.
            output.Close();
            //listener.Stop();
        }
    }


    //[Mapping("Person")]
    //public void getPersonHandler(params object[] id)
    //{
    //    Debug.Log($"void Person({(int)id})");
    //}
    [Mapping("2222")]
    public void get2222Handler(string json)
    {
        Debug.Log($"void 2222({json})");
        var obj = JsonUtility.FromJson<Login>(json);
        Debug.Log($"解析：令牌{obj.token}，rid{obj.rid}");
    }
    [Mapping("3333")]
    public void get3333Handler(object id)
    {
        Debug.Log($"void 3333({id})");
    }
    [Mapping("toFriend")]
    public void toFriendHandler(string json)
    {
        Debug.Log($"void toFriend({json})");

        var obj = JsonUtility.FromJson<User>(json);
        Debug.Log($"解析：用户名{obj.user}，密码{obj.pwd}");
    }
}

struct User
{
    public string user;
    public string pwd;
}

struct Login
{
    public string token;
    public string rid;
}

class Mapping : Attribute
{
    public string Map;
    public Mapping(string s)
    {
        Map = s;
    }
}