using System.Text;

namespace json {

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

        public JsonObject? GetJsonObject(string key) {
            return (this.data.TryGetValue(key, out object? value) && value is Dictionary<string, object> dict) ? new JsonObject(dict) : null;
        }

        public void PutJsonObject(string key, JsonObject jsonObject) {
            if (key != null) {
                this.data[key] = jsonObject.AsDict();
            }
        }

        public JsonObject? RemoveJsonObject(string key) {
            if (key != null && this.data.TryGetValue(key, out object? value) && value is Dictionary<string, object> dict) {
                if (this.data.Remove(key)) {
                    return new JsonObject(dict);
                }
            }
            return null;
        }

        public JsonArray? GetJsonArray(string key) {
            return (this.data.TryGetValue(key, out object? value) && value is List<object> list) ? new JsonArray(list) : null;
        }

        public void PutJsonArray(string key, JsonArray jsonArray) {
            if (key != null) {
                this.data[key] = jsonArray.AsList();
            }
        }

        public JsonArray? RemoveJsonArray(string key) {
            if (key != null && this.data.TryGetValue(key, out object? value) && value is List<object> list) {
                if (this.data.Remove(key)) {
                    return new JsonArray(list);
                }
            }
            return null;
        }

        public string? GetString(string key) {
            return (this.data.TryGetValue(key, out object? value) && value is string s) ? s : null;
        }

        public void PutString(string key, string s) {
            if (key != null) {
                this.data[key] = s;
            }
        }

        public string? RemoveString(string key) {
            if (key != null && this.data.TryGetValue(key, out object? value) && value is string s) {
                if (this.data.Remove(key)) {
                    return s;
                }
            }
            return null;
        }

        public int? GetInt(string key) {
            return (this.data.TryGetValue(key, out object? value) && value is int i) ? i : null;
        }

        public void PutInt(string key, int i) {
            if (key != null) {
                this.data[key] = i;
            }
        }

        public int? RemoveInt(string key) {
            if (key != null && this.data.TryGetValue(key, out object? value) && value is int i) {
                if (this.data.Remove(key)) {
                    return i;
                }
            }
            return null;
        }

        public double? GetDouble(string key) {
            return (this.data.TryGetValue(key, out object? value) && value is double d) ? d : null;
        }

        public void PutDouble(string key, double d) {
            if (key != null) {
                this.data[key] = d;
            }
        }

        public double? RemoveDouble(string key) {
            if (key != null && this.data.TryGetValue(key, out object? value) && value is double d) {
                if (this.data.Remove(key)) {
                    return d;
                }
            }
            return null;
        }

        public bool? GetBool(string key) {
            return (this.data.TryGetValue(key, out object? value) && value is bool b) ? b : null;
        }

        public void PutBool(string key, bool b) {
            if (key != null) {
                this.data[key] = b;
            }
        }

        public bool? RemoveBool(string key) {
            if (key != null && this.data.TryGetValue(key, out object? value) && value is bool b) {
                if (this.data.Remove(key)) {
                    return b;
                }
            }
            return null;
        }

        public bool IsEmpty() {
            return this.data == null || this.data.Count == 0;
        }

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