using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

public class Converter
{
    // Form-data转Json
    public static string ConvertJson(string str)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        Regex re = new Regex(@"(^|&)?(\w+)=([^&]+)(&|$)?", RegexOptions.Compiled);
        MatchCollection mc = re.Matches(str);
        foreach (Match m in mc)
        {
            dic.Add(m.Result("$2"), m.Result("$3"));
        }

        string json = JsonConvert.SerializeObject(dic);
        return json;
    }

    // NameValueCollection转Json
    public static Dictionary<string, string> FormToModel(System.Collections.Specialized.NameValueCollection form)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        try
        {
            if (form != null)
            {
                foreach (string key in form)
                {
                    if (key != null)
                        dic.Add(key, form[key]);
                }
            }
        }
        catch (Exception ex)
        {
            dic.Add("ERROR", ex.Message);
        }
        return dic;
    }
}