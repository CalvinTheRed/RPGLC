using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace com.rpglc.json;

public class JsonArray {

    public List<object> data;

    public JsonArray() {
        data = [];
    }

    public JsonArray(List<object> data) {
        this.data = data ?? ([]);
    }

    public JsonArray LoadFromFile(string path) {
        return LoadFromString(File.ReadAllText(path));
    }

    public JsonArray LoadFromString(string json) {
        data = JsonConvert.DeserializeObject<List<object>>(json) ?? [];

        // convert Newtonsoft data types to Collections data types
        for (int i = 0; i < data.Count; i++) {
            object value = data[i];
            if (value is JObject jObject) {
                data[i] = JsonUtils.ConvertJObjectToDict(jObject);
            } else if (value is JArray jArray) {
                data[i] = JsonUtils.ConvertJArrayToList(jArray);
            }
        }

        // convert any int values to longs
        JsonUtils.PreferLong(this.AsList());

        return this;
    }

    public List<object> AsList() {
        return data;
    }

    public JsonArray DeepClone() {
        JsonArray clone = new();
        foreach (var item in data) {
            if (item is Dictionary<string, object> dict) {
                clone.AddJsonObject(new JsonObject(dict).DeepClone());
            } else if (item is List<object> list) {
                clone.AddJsonArray(new JsonArray(list).DeepClone());
            } else {
                clone.data.Add(item);
            }
        }
        return clone;
    }

    // =================================================================================================================
    // Get() methods
    // =================================================================================================================

    public JsonObject? GetJsonObject(int index) {
        return index < data.Count && data[index] is Dictionary<string, object> dict ? new JsonObject(dict) : null;
    }

    public JsonArray? GetJsonArray(int index) {
        return index < data.Count && data[index] is List<object> list ? new JsonArray(list) : null;
    }

    public string? GetString(int index) {
        return index < data.Count && data[index] is string s ? s : null;
    }

    public long? GetLong(int index) {
        return index < data.Count && data[index] is long l ? l : null;
    }

    public double? GetDouble(int index) {
        return index < data.Count && data[index] is double d ? d : null;
    }

    public bool? GetBool(int index) {
        return index < data.Count && data[index] is bool b ? b : null;
    }

    // =================================================================================================================
    // Add() methods
    // =================================================================================================================

    public JsonArray AddJsonObject(JsonObject? jsonObject) {
        if (jsonObject is not null) {
            data.Add(jsonObject.AsDict());
        }
        return this;
    }

    public JsonArray AddJsonArray(JsonArray? jsonArray) {
        if (jsonArray is not null) {
            data.Add(jsonArray.AsList());
        }
        return this;
    }

    public JsonArray AddString(string? s) {
        if (s is not null) {
            data.Add(s);
        }
        return this;
    }

    public JsonArray AddLong(long? l) {
        if (l is not null) {
            data.Add(l);
        }
        return this;
    }

    public JsonArray AddDouble(double? d) {
        if (d is not null) {
            data.Add(d);
        }
        return this;
    }

    public JsonArray AddBool(bool? b) {
        if (b is not null) {
            data.Add(b);
        }
        return this;
    }

    // =================================================================================================================
    // Remove() methods
    // =================================================================================================================

    public JsonObject? RemoveJsonObject(int index) {
        if (index < data.Count && data[index] is Dictionary<string, object> dict) {
            data.RemoveAt(index);
            return new JsonObject(dict);
        }
        return null;
    }

    public JsonArray? RemoveJsonArray(int index) {
        if (index < data.Count && data[index] is List<object> list) {
            data.RemoveAt(index);
            return new JsonArray(list);
        }
        return null;
    }

    public string? RemoveString(int index) {
        if (index < data.Count && data[index] is string s) {
            data.RemoveAt(index);
            return s;
        }
        return null;
    }

    public long? RemoveLong(int index) {
        if (index < data.Count && data[index] is long l) {
            data.RemoveAt(index);
            return l;
        }
        return null;
    }

    public double? RemoveDouble(int index) {
        if (index < data.Count && data[index] is double d) {
            data.RemoveAt(index);
            return d;
        }
        return null;
    }

    public bool? RemoveBool(int index) {
        if (index < data.Count && data[index] is bool b) {
            data.RemoveAt(index);
            return b;
        }
        return null;
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

    public bool Contains(object item) {
        return data.Contains(item);
    }

    public bool ContainsAny(List<object> other) {
        foreach (object item in other) {
            if (data.Contains(item)) {
                return true;
            }
        }
        return false;
    }

    public bool ContainsAll(List<object> other) {
        foreach (object item in other) {
            if (!data.Contains(item)) {
                return false;
            }
        }
        return true;
    }

    public override bool Equals(object? obj) {
        return obj is not null
            && obj is JsonArray jsonArray
            && jsonArray.ToString() == ToString();
    }

    // =================================================================================================================
    // printing methods
    // =================================================================================================================

    public string PrettyPrint() {
        return PrettyPrint(0);
    }

    internal string PrettyPrint(int indent) {
        StringBuilder sb = new();
        if (IsEmpty()) {
            sb.Append("[ ]");
        } else {
            sb.AppendLine("[");
            for (int i = 0; i < data.Count; i++) {
                sb.Append(string.Concat(Enumerable.Repeat(JsonUtils.indent, indent + 1)));
                object item = data[i];
                if (item is Dictionary<string, object> dict) {
                    sb.Append(new JsonObject(dict).PrettyPrint(indent + 1));
                } else if (item is List<object> list) {
                    sb.Append(new JsonArray(list).PrettyPrint(indent + 1));
                } else if (item is string) {
                    sb.Append('"').Append(item).Append('"');
                } else if (item is bool b) {
                    sb.Append(b ? "true" : "false");
                } else if (item is null) {
                    sb.Append("null");
                } else {
                    sb.Append(item);
                }
                if (i < data.Count - 1) {
                    sb.Append(',');
                }
                sb.AppendLine();
            }
            sb.Append(string.Concat(Enumerable.Repeat(JsonUtils.indent, indent)))
                .Append(']');
        }
        return sb.ToString();
    }

    public override string ToString() {
        StringBuilder sb = new();
        if (IsEmpty()) {
            sb.Append("[]");
        } else {
            sb.Append('[');
            for (int i = 0; i < data.Count; i++) {
                object item = data[i];
                if (item is Dictionary<string, object> dict) {
                    sb.Append(new JsonObject(dict).ToString());
                } else if (item is List<object> list) {
                    sb.Append(new JsonArray(list).ToString());
                } else if (item is string s) {
                    sb.Append('"').Append(s).Append('"');
                } else if (item is bool b) {
                    sb.Append(b ? "true" : "false");
                } else if (item is null) {
                    sb.Append("null");
                } else {
                    sb.Append(item);
                }
                if (i < data.Count - 1) {
                    sb.Append(',');
                }
            }
            sb.Append(']');
        }
        return sb.ToString();
    }

};
