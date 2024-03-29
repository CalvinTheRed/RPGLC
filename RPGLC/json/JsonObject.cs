using System.Text;

namespace rpglc.json {

    internal class Consts {
        public const string indent = "  ";
    }

    public class JsonObject {

        public Dictionary<string, object> data;

        public JsonObject() {
            this.data = [];
        }

        public JsonObject(Dictionary<string, object> data) {
            this.data = data ?? [];
        }

        public Dictionary<string, object> AsDict() {
            return this.data;
        }

        public JsonObject DeepClone() {
            JsonObject clone = new();
            foreach (string key in this.data.Keys) {
                object value = this.data[key];
                if (value is Dictionary<string, object>) {
                    clone.PutJsonObject(key, this.GetJsonObject(key).DeepClone());
                } else if (value is List<object>) {
                    clone.PutJsonArray(key, this.GetJsonArray(key).DeepClone());
                } else {
                    clone.data[key] = value;
                }
            }
            return clone;
        }

        // =================================================================================================================
        // Get() methods
        // =================================================================================================================

        public JsonObject? GetJsonObject(string key) {
            return (this.data.TryGetValue(key, out object? value) && value is Dictionary<string, object> dict) ? new JsonObject(dict) : null;
        }

        public JsonArray? GetJsonArray(string key) {
            return (this.data.TryGetValue(key, out object? value) && value is List<object> list) ? new JsonArray(list) : null;
        }

        public string? GetString(string key) {
            return (this.data.TryGetValue(key, out object? value) && value is string s) ? s : null;
        }

        public int? GetInt(string key) {
            return (this.data.TryGetValue(key, out object? value) && value is int i) ? i : null;
        }

        public double? GetDouble(string key) {
            return (this.data.TryGetValue(key, out object? value) && value is double d) ? d : null;
        }

        public bool? GetBool(string key) {
            return (this.data.TryGetValue(key, out object? value) && value is bool b) ? b : null;
        }

        // =================================================================================================================
        // Put() methods
        // =================================================================================================================

        public void PutJsonObject(string key, JsonObject? jsonObject) {
            if (key != null) {
                this.data[key] = jsonObject.AsDict();
            }
        }

        public void PutJsonArray(string key, JsonArray? jsonArray) {
            if (key != null) {
                this.data[key] = jsonArray.AsList();
            }
        }

        public void PutString(string key, string? s) {
            if (key != null) {
                this.data[key] = s;
            }
        }

        public void PutInt(string key, int? i) {
            if (key != null) {
                this.data[key] = i;
            }
        }

        public void PutDouble(string key, double? d) {
            if (key != null) {
                this.data[key] = d;
            }
        }

        public void PutBool(string key, bool? b) {
            if (key != null) {
                this.data[key] = b;
            }
        }

        // =================================================================================================================
        // Remove() methods
        // =================================================================================================================

        public JsonObject? RemoveJsonObject(string key) {
            if (key != null && this.data.TryGetValue(key, out object? value) && value is Dictionary<string, object> dict) {
                if (this.data.Remove(key)) {
                    return new JsonObject(dict);
                }
            }
            return null;
        }

        public JsonArray? RemoveJsonArray(string key) {
            if (key != null && this.data.TryGetValue(key, out object? value) && value is List<object> list) {
                if (this.data.Remove(key)) {
                    return new JsonArray(list);
                }
            }
            return null;
        }

        public string? RemoveString(string key) {
            if (key != null && this.data.TryGetValue(key, out object? value) && value is string s) {
                if (this.data.Remove(key)) {
                    return s;
                }
            }
            return null;
        }

        public int? RemoveInt(string key) {
            if (key != null && this.data.TryGetValue(key, out object? value) && value is int i) {
                if (this.data.Remove(key)) {
                    return i;
                }
            }
            return null;
        }

        public double? RemoveDouble(string key) {
            if (key != null && this.data.TryGetValue(key, out object? value) && value is double d) {
                if (this.data.Remove(key)) {
                    return d;
                }
            }
            return null;
        }

        public bool? RemoveBool(string key) {
            if (key != null && this.data.TryGetValue(key, out object? value) && value is bool b) {
                if (this.data.Remove(key)) {
                    return b;
                }
            }
            return null;
        }

        // =================================================================================================================
        // Seek() methods
        // =================================================================================================================

        public JsonObject? SeekJsonObject(string path) {
            return this.Seek(path) is Dictionary<string, object> dict ? new JsonObject(dict) : null;
        }

        public JsonArray? SeekJsonArray(string path) {
            return this.Seek(path) is List<object> list ? new JsonArray(list) : null;
        }

        public string? SeekString(string path) {
            return this.Seek(path) is string s ? s : null;
        }

        public int? SeekInt(string path) {
            return this.Seek(path) is int i ? i : null;
        }

        public double? SeekDouble(string path) {
            return this.Seek(path) is double d ? d : null;
        }

        public bool? SeekBool(string path) {
            return this.Seek(path) is bool b ? b : null;
        }

        private object Seek(string path) { // TODO might break if path ends with index
            object focus = this.AsDict();
            foreach(string key in path.Split('.')) {
                if (key.Contains('[')) {
                    string arrayKey = key[..key.IndexOf('[')];
                    focus = ((Dictionary<string, object>) focus)[arrayKey];

                    int startIndex = key.IndexOf('[') + 1;
                    int substringLength = key.Length - 1 - startIndex;
                    string[] indices = key.Substring(startIndex, substringLength).Split("][");
                    foreach (string index in indices) {
                        focus = ((List<object>) focus)[Int32.Parse(index)];
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

        public void InsertJsonObject(string path, JsonObject jsonObject) {
            if (path.Contains('.')) {
                string relativeRoot = path[..path.LastIndexOf('.')];
                string key = path[(path.LastIndexOf('.') + 1)..]; // this is faulty
                this.SeekJsonObject(relativeRoot)?.PutJsonObject(key, jsonObject);
            } else {
                this.PutJsonObject(path, jsonObject);
            }
        }

        public void InsertJsonArray(string path, JsonArray jsonArray) {
            if (path.Contains('.')) {
                string relativeRoot = path[..path.LastIndexOf('.')];
                string key = path[(path.LastIndexOf('.') + 1)..];
                this.SeekJsonObject(relativeRoot)?.PutJsonArray(key, jsonArray);
            } else {
                this.PutJsonArray(path, jsonArray);
            }
        }

        public void InsertString(string path, string s) {
            if (path.Contains('.')) {
                string relativeRoot = path[..path.LastIndexOf('.')];
                string key = path[(path.LastIndexOf('.') + 1)..];
                this.SeekJsonObject(relativeRoot)?.PutString(key, s);
            } else {
                this.PutString(path, s);
            }
        }

        public void InsertInt(string path, int i) {
            if (path.Contains('.')) {
                string relativeRoot = path[..path.LastIndexOf('.')];
                string key = path[(path.LastIndexOf('.') + 1)..];
                this.SeekJsonObject(relativeRoot)?.PutInt(key, i);
            } else {
                this.PutInt(path, i);
            }
        }

        public void InsertDouble(string path, double d) {
            if (path.Contains('.')) {
                string relativeRoot = path[..path.LastIndexOf('.')];
                string key = path[(path.LastIndexOf('.') + 1)..];
                this.SeekJsonObject(relativeRoot)?.PutDouble(key, d);
            } else {
                this.PutDouble(path, d);
            }
        }

        public void InsertBool(string path, bool b) {
            if (path.Contains('.')) {
                string relativeRoot = path[..path.LastIndexOf('.')];
                string key = path[(path.LastIndexOf('.') + 1)..];
                this.SeekJsonObject(relativeRoot)?.PutBool(key, b);
            } else {
                this.PutBool(path, b);
            }
        }

        // =================================================================================================================
        // Join() methods
        // =================================================================================================================

        public void Join(Dictionary<string, object> other) {
            this.Join(new JsonObject(other));
        }

        public void Join(JsonObject other) {
            JsonObject otherClone = other.DeepClone();
            foreach (string otherKey in otherClone.data.Keys) {
                object otherValue = otherClone.data[otherKey];
                if (otherValue is Dictionary<string, object> otherMap) {
                    object thisValue = this.data[otherKey];
                    if (thisValue is Dictionary<string, object> thisMap) {
                        // nested join if a map is being joined to a map
                        JsonObject thisJsonObject = new();
                        thisJsonObject.Join(new JsonObject(thisMap));
                        thisJsonObject.Join(new JsonObject(otherMap));
                        this.PutJsonObject(otherKey, thisJsonObject);
                    } else {
                        // override this key if this is not also a map
                        this.data.Add(otherKey, otherMap);
                    }
                } else if (otherValue is List<object> otherList) {
                    object thisValue = this.data[otherKey];
                    if (thisValue is List<object> thisList) {
                        // union if a list is being joined to a list
                        foreach (object item in otherList) {
                            if (!thisList.Contains(item)) {
                                thisList.Add(item);
                            }
                        }
                    } else {
                        // override the key if this is not also a list
                        this.data.Add(otherKey, otherList);
                    }
                } else {
                    // override any primitives being joined
                    this.data.Add(otherKey, otherValue);
                }
            }
        }

        // =================================================================================================================
        // content-checking methods
        // =================================================================================================================

        public int Count() {
            return this.data.Count;
        }

        public bool IsEmpty() {
            return this.data == null || this.data.Count == 0;
        }

        // =================================================================================================================
        // printing methods
        // =================================================================================================================

        public string PrettyPrint() {
            return this.PrettyPrint(0);
        }

        internal string PrettyPrint(int indent) {
            StringBuilder sb = new();
            int remainingItems = this.data.Count;
            if (remainingItems == 0) {
                sb.Append("{ }");
            } else {
                sb.AppendLine("{");
                foreach (string key in this.data.Keys.OrderBy(q => q).ToList()) {
                    sb.Append(string.Concat(Enumerable.Repeat(Consts.indent, indent + 1)))
                        .Append('"').Append(key).Append("\": ");
                    object value = this.data[key];
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
                sb.Append(string.Concat(Enumerable.Repeat(Consts.indent, indent)))
                    .Append('}');
            }
            return sb.ToString();
        }

        override public string ToString() {
            StringBuilder sb = new();
            int remainingItems = this.data.Count;
            if (remainingItems == 0) {
                sb.Append("{}");
            } else {
                sb.Append('{');
                foreach (string key in this.data.Keys.OrderBy(q => q).ToList()) {
                    sb.Append('"').Append(key).Append("\":");
                    object value = this.data[key];
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
    }
}