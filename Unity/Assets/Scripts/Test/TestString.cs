using UnityEngine;

public class TestString : MonoBehaviour
{
    public void OnString()
    {
        //string json = "{";
        //foreach (var item in qscoll)
        //{
        //    string key = item.ToString();
        //    string value = qscoll[key];
        //    //Debug.Log($"{key} : {value}");

        //    json += $"\"{key}\":\"{value}\",";
        //}
        //json = json.Remove(json.Length - 1, 1);
        //json += "}";
        //paramsCache.Add(json);
    }

    public void OnStringBuilder()
    {
        //StringBuilder sb = new StringBuilder("{");
        //foreach (String key in qscoll.AllKeys)
        //{
        //    string value = qscoll[key];
        //    sb.Append($"\"{key}\":\"{value}\",");
        //}
        //sb.Remove(sb.Length - 1, 1);
        //sb.Append("}");
        //string json = sb.ToString();
        //Debug.Log(json);
        //paramsCache.Add(json);
    }
}