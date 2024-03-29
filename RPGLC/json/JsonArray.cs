﻿using System.Text;

namespace rpglc.json {
    public class JsonArray {

        public List<object> data;

        public JsonArray() {
            this.data = [];
        }

        public JsonArray(List<object> data) {
            this.data = data ?? ([]);
        }

        public List<object> AsList() {
            return this.data;
        }

        public JsonArray DeepClone() {
            JsonArray clone = new();
            foreach (var item in this.data) {
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
            return (index < this.data.Count && this.data[index] is Dictionary<string, object> dict) ? new JsonObject(dict) : null;
        }

        public JsonArray? GetJsonArray(int index) {
            return (index < this.data.Count && this.data[index] is List<object> list) ? new JsonArray(list) : null;
        }

        public string? GetString(int index) {
            return (index < this.data.Count && this.data[index] is string s) ? s : null;
        }

        public int? GetInt(int index) {
            return (index < this.data.Count && this.data[index] is int i) ? i : null;
        }

        public double? GetDouble(int index) {
            return (index < this.data.Count && this.data[index] is double d) ? d : null;
        }

        public bool? GetBool(int index) {
            return (index < this.data.Count && this.data[index] is bool b) ? b : null;
        }

        // =================================================================================================================
        // Add() methods
        // =================================================================================================================

        public void AddJsonObject(JsonObject? jsonObject) {
            if (jsonObject != null) {
                this.data.Add(jsonObject.AsDict());
            }
        }

        public void AddJsonArray(JsonArray? jsonArray) {
            if (jsonArray != null) {
                this.data.Add(jsonArray.AsList());
            }
        }

        public void AddString(string? s) {
            if (s != null) {
                this.data.Add(s);
            }
        }

        public void AddInt(int? i) {
            if (i != null) {
                this.data.Add(i);
            }
        }

        public void AddDouble(double? d) {
            if (d != null) {
                this.data.Add(d);
            }
        }

        public void AddBool(bool? b) {
            if (b != null) {
                this.data.Add(b);
            }
        }

        // =================================================================================================================
        // Remove() methods
        // =================================================================================================================

        public JsonObject? RemoveJsonObject(int index) {
            if (index < this.data.Count && this.data[index] is Dictionary<string, object> dict) {
                this.data.RemoveAt(index);
                return new JsonObject(dict);
            }
            return null;
        }

        public JsonArray? RemoveJsonArray(int index) {
            if (index < this.data.Count && this.data[index] is List<object> list) {
                this.data.RemoveAt(index);
                return new JsonArray(list);
            }
            return null;
        }

        public string? RemoveString(int index) {
            if (index < this.data.Count && this.data[index] is string s) {
                this.data.RemoveAt(index);
                return s;
            }
            return null;
        }

        public int? RemoveInt(int index) {
            if (index < this.data.Count && this.data[index] is int i) {
                this.data.RemoveAt(index);
                return i;
            }
            return null;
        }

        public double? RemoveDouble(int index) {
            if (index < this.data.Count && this.data[index] is double d) {
                this.data.RemoveAt(index);
                return d;
            }
            return null;
        }

        public bool? RemoveBool(int index) {
            if (index < this.data.Count && this.data[index] is bool b) {
                this.data.RemoveAt(index);
                return b;
            }
            return null;
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

        public bool Contains(object item) {
            return this.data.Contains(item);
        }

        public bool ContainsAny(List<object> other) {
            foreach (object item in other) {
                if (this.data.Contains(item)) {
                    return true;
                }
            }
            return false;
        }

        // =================================================================================================================
        // printing methods
        // =================================================================================================================

        public string PrettyPrint() {
            return this.PrettyPrint(0);
        }

        internal string PrettyPrint(int indent) {
            StringBuilder sb = new();
            if (this.IsEmpty()) {
                sb.Append("[ ]");
            } else {
                sb.AppendLine("[");
                for (int i = 0; i < this.data.Count; i++) {
                    sb.Append(string.Concat(Enumerable.Repeat(Consts.indent, indent + 1)));
                    object item = this.data[i];
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
                    if (i < this.data.Count - 1) {
                        sb.Append(',');
                    }
                    sb.AppendLine();
                }
                sb.Append(string.Concat(Enumerable.Repeat(Consts.indent, indent)))
                    .Append(']');
            }
            return sb.ToString();
        }

        public override string ToString() {
            StringBuilder sb = new();
            if (this.IsEmpty()) {
                sb.Append("[]");
            } else {
                sb.Append('[');
                for (int i = 0; i < this.data.Count; i++) {
                    object item = this.data[i];
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
                    if (i < this.data.Count - 1) {
                        sb.Append(',');
                    }
                }
                sb.Append(']');
            }
            return sb.ToString();
        }
    }
}