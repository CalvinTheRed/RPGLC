using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
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
        return this.LoadFromString(File.ReadAllText(path));
    }

    public JsonArray LoadFromString(string json) {
        this.data = JsonConvert.DeserializeObject<List<object>>(json) ?? [];

        // convert Newtonsoft data types to Collections data types
        for (int i = 0; i < this.data.Count; i++) {
            object value = this.data[i];
            if (value is JObject jObject) {
                this.data[i] = JsonUtils.ConvertJObjectToDict(jObject);
            } else if (value is JArray jArray) {
                this.data[i] = JsonUtils.ConvertJArrayToList(jArray);
            }
        }

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

    public int? GetInt(int index) {
        return index < data.Count && data[index] is int i ? i : null;
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

    public void AddJsonObject(JsonObject? jsonObject) {
        if (jsonObject != null) {
            data.Add(jsonObject.AsDict());
        }
    }

    public void AddJsonArray(JsonArray? jsonArray) {
        if (jsonArray != null) {
            data.Add(jsonArray.AsList());
        }
    }

    public void AddString(string? s) {
        if (s != null) {
            data.Add(s);
        }
    }

    public void AddInt(int? i) {
        if (i != null) {
            data.Add(i);
        }
    }

    public void AddDouble(double? d) {
        if (d != null) {
            data.Add(d);
        }
    }

    public void AddBool(bool? b) {
        if (b != null) {
            data.Add(b);
        }
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

    public int? RemoveInt(int index) {
        if (index < data.Count && data[index] is int i) {
            data.RemoveAt(index);
            return i;
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
}