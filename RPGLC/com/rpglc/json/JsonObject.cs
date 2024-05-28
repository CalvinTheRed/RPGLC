using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace com.rpglc.json;

public class JsonObject {

    public Dictionary<string, object> data;

    public JsonObject() {
        data = [];
    }

    public JsonObject(Dictionary<string, object> data) {
        this.data = data ?? [];
    }

    public JsonObject LoadFromFile(string path) {
        return this.LoadFromString(File.ReadAllText(path));
    }

    public JsonObject LoadFromString(string json) {
        this.data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json) ?? [];

        // convert Newtonsoft data types to Collections data types
        foreach ((string key, object value) in this.data) {
            if (value is JObject jObject) {
                this.data[key] = JsonUtils.ConvertJObjectToDict(jObject);
            } else if (value is JArray jArray) {
                this.data[key] = JsonUtils.ConvertJArrayToList(jArray);
            }
        }

        return this;
    }

    public Dictionary<string, object> AsDict() {
        return data;
    }

    public JsonObject DeepClone() {
        JsonObject clone = new();
        foreach (string key in data.Keys) {
            object value = data[key];
            if (value is Dictionary<string, object>) {
                clone.PutJsonObject(key, GetJsonObject(key)?.DeepClone());
            } else if (value is List<object>) {
                clone.PutJsonArray(key, GetJsonArray(key)?.DeepClone());
            } else {
                clone.data[key] = value;
            }
        }
        return clone;
    }

    // =================================================================================================================
    // Get() methods
    // =================================================================================================================

    public JsonObject GetJsonObject(string key) {
        return new((Dictionary<string, object>) data[key]);
    }

    public JsonArray GetJsonArray(string key) {
        return new((List<object>) data[key]);
    }

    public string GetString(string key) {
        return (string) data[key];
    }

    public long GetInt(string key) {
        return (long) data[key];
    }

    public double GetDouble(string key) {
        return (double) data[key];
    }

    public bool GetBool(string key) {
        return (bool) data[key];
    }

    // =================================================================================================================
    // Put() methods
    // =================================================================================================================

    public JsonObject PutJsonObject(string key, JsonObject jsonObject) {
        data[key] = jsonObject.AsDict();
        return this;
    }

    public JsonObject PutJsonArray(string key, JsonArray jsonArray) {
        data[key] = jsonArray.AsList();
        return this;
    }

    public JsonObject PutString(string key, string s) {
        data[key] = s;
        return this;
    }

    public JsonObject PutInt(string key, long i) {
        data[key] = i;
        return this;
    }

    public JsonObject PutDouble(string key, double d) {
        data[key] = d;
        return this;
    }

    public JsonObject PutBool(string key, bool b) {
        data[key] = b;
        return this;
    }

    // =================================================================================================================
    // Remove() methods
    // =================================================================================================================

    public JsonObject? RemoveJsonObject(string key) {
        if (key != null && data.TryGetValue(key, out object? value) && value is Dictionary<string, object> dict) {
            if (data.Remove(key)) {
                return new JsonObject(dict);
            }
        }
        return null;
    }

    public JsonArray? RemoveJsonArray(string key) {
        if (key != null && data.TryGetValue(key, out object? value) && value is List<object> list) {
            if (data.Remove(key)) {
                return new JsonArray(list);
            }
        }
        return null;
    }

    public string? RemoveString(string key) {
        if (key != null && data.TryGetValue(key, out object? value) && value is string s) {
            if (data.Remove(key)) {
                return s;
            }
        }
        return null;
    }

    public long? RemoveInt(string key) {
        if (key != null && data.TryGetValue(key, out object? value) && value is long i) {
            if (data.Remove(key)) {
                return i;
            }
        }
        return null;
    }

    public double? RemoveDouble(string key) {
        if (key != null && data.TryGetValue(key, out object? value) && value is double d) {
            if (data.Remove(key)) {
                return d;
            }
        }
        return null;
    }

    public bool? RemoveBool(string key) {
        if (key != null && data.TryGetValue(key, out object? value) && value is bool b) {
            if (data.Remove(key)) {
                return b;
            }
        }
        return null;
    }

    // =================================================================================================================
    // Seek() methods
    // =================================================================================================================

    public JsonObject SeekJsonObject(string path) {
        return new((Dictionary<string, object>) Seek(path));
    }

    public JsonArray SeekJsonArray(string path) {
        return new((List<object>) Seek(path));
    }

    public string SeekString(string path) {
        return (string) Seek(path);
    }

    public long SeekInt(string path) {
        return (long) Seek(path);
    }

    public double SeekDouble(string path) {
        return (double) Seek(path);
    }

    public bool SeekBool(string path) {
        return (bool) Seek(path);
    }

    private object Seek(string path) { // TODO might break if path ends with index
        object focus = AsDict();
        foreach (string key in path.Split('.')) {
            if (key.Contains('[')) {
                string arrayKey = key[..key.IndexOf('[')];
                focus = ((Dictionary<string, object>) focus)[arrayKey];

                int startIndex = key.IndexOf('[') + 1;
                int substringLength = key.Length - 1 - startIndex;
                string[] indices = key.Substring(startIndex, substringLength).Split("][");
                foreach (string index in indices) {
                    focus = ((List<object>) focus)[int.Parse(index)];
                }
            } else {
                focus = ((Dictionary<string, object>) focus)[key];
            }
        }
        return focus;
    }

    // =================================================================================================================
    // Insert() methods
    // =================================================================================================================

    public JsonObject InsertJsonObject(string path, JsonObject jsonObject) {
        if (path.Contains('.')) {
            string relativeRoot = path[..path.LastIndexOf('.')];
            string key = path[(path.LastIndexOf('.') + 1)..]; // this is faulty
            SeekJsonObject(relativeRoot)?.PutJsonObject(key, jsonObject);
        } else {
            PutJsonObject(path, jsonObject);
        }
        return this;
    }

    public JsonObject InsertJsonArray(string path, JsonArray jsonArray) {
        if (path.Contains('.')) {
            string relativeRoot = path[..path.LastIndexOf('.')];
            string key = path[(path.LastIndexOf('.') + 1)..];
            SeekJsonObject(relativeRoot)?.PutJsonArray(key, jsonArray);
        } else {
            PutJsonArray(path, jsonArray);
        }
        return this;
    }

    public JsonObject InsertString(string path, string s) {
        if (path.Contains('.')) {
            string relativeRoot = path[..path.LastIndexOf('.')];
            string key = path[(path.LastIndexOf('.') + 1)..];
            SeekJsonObject(relativeRoot)?.PutString(key, s);
        } else {
            PutString(path, s);
        }
        return this;
    }

    public JsonObject InsertInt(string path, long i) {
        if (path.Contains('.')) {
            string relativeRoot = path[..path.LastIndexOf('.')];
            string key = path[(path.LastIndexOf('.') + 1)..];
            SeekJsonObject(relativeRoot)?.PutInt(key, i);
        } else {
            PutInt(path, i);
        }
        return this;
    }

    public JsonObject InsertDouble(string path, double d) {
        if (path.Contains('.')) {
            string relativeRoot = path[..path.LastIndexOf('.')];
            string key = path[(path.LastIndexOf('.') + 1)..];
            SeekJsonObject(relativeRoot)?.PutDouble(key, d);
        } else {
            PutDouble(path, d);
        }
        return this;
    }

    public JsonObject InsertBool(string path, bool b) {
        if (path.Contains('.')) {
            string relativeRoot = path[..path.LastIndexOf('.')];
            string key = path[(path.LastIndexOf('.') + 1)..];
            SeekJsonObject(relativeRoot)?.PutBool(key, b);
        } else {
            PutBool(path, b);
        }
        return this;
    }

    // =================================================================================================================
    // Join() methods
    // =================================================================================================================

    public JsonObject Join(Dictionary<string, object> other) {
        return this.Join(new JsonObject(other));
    }

    public JsonObject Join(JsonObject other) {
        JsonObject otherClone = other.DeepClone();
        foreach (string otherKey in otherClone.data.Keys) {
            object otherValue = otherClone.data[otherKey];
            if (otherValue is Dictionary<string, object> otherMap) {
                object? thisValue = data.TryGetValue(otherKey, out object? value) ? value : null;
                if (thisValue is Dictionary<string, object> thisMap) {
                    // nested join if a map is being joined to a map
                    JsonObject thisJsonObject = new();
                    thisJsonObject.Join(new JsonObject(thisMap));
                    thisJsonObject.Join(new JsonObject(otherMap));
                    PutJsonObject(otherKey, thisJsonObject);
                } else {
                    // override this key if this is not also a map
                    data.Add(otherKey, otherMap);
                }
            } else if (otherValue is List<object> otherList) {
                object? thisValue = data.TryGetValue(otherKey, out object? value) ? value : null;
                if (thisValue is List<object> thisList) {
                    // union if a list is being joined to a list
                    foreach (object item in otherList) {
                        if (!thisList.Contains(item)) {
                            thisList.Add(item);
                        }
                    }
                } else {
                    // override the key if this is not also a list
                    data[otherKey] = otherList;
                }
            } else {
                // override any primitives being joined
                data[otherKey] = otherValue;
            }
        }
        return this;
    }

    // =================================================================================================================
    // content-checking methods
    // =================================================================================================================

    public int Count() {
        return data.Count;
    }

    public bool IsEmpty() {
        return data == null || data.Count == 0;
    }

    // =================================================================================================================
    // printing methods
    // =================================================================================================================

    public string PrettyPrint() {
        return PrettyPrint(0);
    }

    public string PrettyPrint(int indent) {
        StringBuilder sb = new();
        int remainingItems = data.Count;
        if (remainingItems == 0) {
            sb.Append("{ }");
        } else {
            sb.AppendLine("{");
            foreach (string key in data.Keys.OrderBy(q => q).ToList()) {
                sb.Append(string.Concat(Enumerable.Repeat(JsonUtils.indent, indent + 1)))
                    .Append('"').Append(key).Append("\": ");
                object value = data[key];
                if (value is Dictionary<string, object> dict) {
                    sb.Append(new JsonObject(dict).PrettyPrint(indent + 1));
                } else if (value is List<object> list) {
                    sb.Append(new JsonArray(list).PrettyPrint(indent + 1));
                } else if (value is string s) {
                    sb.Append('"').Append(s).Append('"');
                } else if (value is bool b) {
                    sb.Append(b ? "true" : "false");
                } else if (value is null) {
                    sb.Append("null");
                } else {
                    sb.Append(value);
                }
                remainingItems--;
                if (remainingItems > 0) {
                    sb.Append(',');
                }
                sb.AppendLine();
            }
            sb.Append(string.Concat(Enumerable.Repeat(JsonUtils.indent, indent)))
                .Append('}');
        }
        return sb.ToString();
    }

    override public string ToString() {
        StringBuilder sb = new();
        int remainingItems = data.Count;
        if (remainingItems == 0) {
            sb.Append("{}");
        } else {
            sb.Append('{');
            foreach (string key in data.Keys.OrderBy(q => q).ToList()) {
                sb.Append('"').Append(key).Append("\":");
                object value = data[key];
                if (value is Dictionary<string, object> dict) {
                    sb.Append(new JsonObject(dict).ToString());
                } else if (value is List<object> list) {
                    sb.Append(new JsonArray(list).ToString());
                } else if (value is string s) {
                    sb.Append('"').Append(s).Append('"');
                } else if (value is bool b) {
                    sb.Append(b ? "true" : "false");
                } else if (value is null) {
                    sb.Append("null");
                } else {
                    sb.Append(value);
                }
                remainingItems--;
                if (remainingItems > 0) {
                    sb.Append(',');
                }
            }
            sb.Append('}');
        }
        return sb.ToString();
    }

};
