using Newtonsoft.Json.Linq;

namespace com.rpglc.json;

internal class JsonUtils {
    public const string indent = "  ";

    public static Dictionary<string, object> ConvertJObjectToDict(JObject arg) {
        Dictionary<string, object> dict = [];
        foreach ((string key, object value) in arg) {
            if (value is JObject jObject) {
                dict.Add(key, ConvertJObjectToDict(jObject));
            } else if (value is JArray jArray) {
                dict.Add(key, ConvertJArrayToList(jArray));
            } else if (value is JValue jValue) {
                switch(jValue.Type) {
                    case JTokenType.String:
                        dict.Add(key, jValue.Value<string>());
                        break;
                    case JTokenType.Integer:
                        dict.Add(key, jValue.Value<int>());
                        break;
                    case JTokenType.Float:
                        dict.Add(key, jValue.Value<double>());
                        break;
                    case JTokenType.Boolean:
                        dict.Add(key, jValue.Value<bool>());
                        break;
                }
            }
        }
        return dict;
    }

    public static List<object> ConvertJArrayToList(JArray arg) {
        List<object> list = [];
        for (int i = 0; i < arg.Count; i++) {
            object item = arg[i];
            if (item is JObject jObject) {
                list.Add(ConvertJObjectToDict(jObject));
            } else if (item is JArray jArray) {
                list.Add(ConvertJArrayToList(jArray));
            } else if (item is JValue jValue) {
                switch(jValue.Type) {
                    case JTokenType.String:
                        list.Add(jValue.Value<string>());
                        break;
                    case JTokenType.Integer:
                        list.Add(jValue.Value<int>());
                        break;
                    case JTokenType.Float:
                        list.Add(jValue.Value<double>());
                        break;
                    case JTokenType.Boolean:
                        list.Add(jValue.Value<bool>());
                        break;
                }
            }
        }
        return list;
    }

};
