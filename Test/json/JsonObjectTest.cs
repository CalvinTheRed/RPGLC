using rpglc.json;

namespace RPGLC.json.test {
    public class JsonObjectTest {

        [Fact(DisplayName = "prints as string")]
        public void PrintsAsString() {
            JsonObject json = new(new Dictionary<string, object> {
                { "object_key", new Dictionary<string, object> { { "nested_key", "value" } } },
                { "array_key", new List<object> { "item_1", "item_2" } },
                { "string_key", "value" },
                { "int_key", 123 },
                { "double_key", 123.456 },
                { "bool_key", false },
            });

            string expected = """
                {"array_key":["item_1","item_2"],"bool_key":false,"double_key":123.456,"int_key":123,"object_key":{"nested_key":"value"},"string_key":"value"}
                """;
            Assert.Equal(expected, json.ToString());
        }

        [Fact(DisplayName = "prints as string when empty")]
        public void PrintsAsStringWhenEmpty() {
            JsonObject json = new();
            Assert.Equal("{}", json.ToString());
        }

        [Fact(DisplayName = "pretty prints")]
        public void PrettyPrints() {
            JsonObject json = new(new Dictionary<string, object> {
                { "object_key", new Dictionary<string, object> { { "nested_key", "value" } } },
                { "array_key", new List<object> { "item_1", "item_2" } },
                { "string_key", "value" },
                { "int_key", 123 },
                { "double_key", 123.456 },
                { "bool_key", false },
            });

            string expected = """
                {
                  "array_key": [
                    "item_1",
                    "item_2"
                  ],
                  "bool_key": false,
                  "double_key": 123.456,
                  "int_key": 123,
                  "object_key": {
                    "nested_key": "value"
                  },
                  "string_key": "value"
                }
                """;
            Assert.Equal(expected, json.PrettyPrint());
        }

        [Fact(DisplayName = "pretty prints when empty")]
        public void PrettyPrintsWhenEmpty() {
            JsonObject json = new();
            Assert.Equal("{ }", json.PrettyPrint());
        }

        // =================================================================================================================
        // Put() and Get() tests
        // =================================================================================================================

        [Fact(DisplayName = "puts and gets JsonObject")]
        public void PutsAndGetsJsonObject() {
            JsonObject json = new();

            Assert.Null(json.GetJsonObject("object_key"));

            json.PutJsonObject("object_key", new JsonObject(new Dictionary<string, object> {
                { "nested_key", "value" },
            }));

            Assert.Equal(
                """{"nested_key":"value"}""",
                json.GetJsonObject("object_key").ToString()
            );
        }

        [Fact(DisplayName = "puts and gets JsonArray")]
        public void PutsAndGetsJsonArray() {
            JsonObject json = new();

            Assert.Null(json.GetJsonArray("array_key"));

            json.PutJsonArray("array_key", new JsonArray([
                "item_1", "item_2",
            ]));

            Assert.Equal(
                """["item_1","item_2"]""",
                json.GetJsonArray("array_key").ToString()
            );
        }

        [Fact(DisplayName = "puts and gets string")]
        public void PutsAndGetsString() {
            JsonObject json = new();

            Assert.Null(json.GetString("string_key"));

            json.PutString("string_key", "value");

            Assert.Equal(
                "value",
                json.GetString("string_key")
            );
        }

        [Fact(DisplayName = "puts and gets int")]
        public void PutsAndGetsInt() {
            JsonObject json = new();

            Assert.Null(json.GetInt("int_key"));

            json.PutInt("int_key", 123);

            Assert.Equal(
                123,
                json.GetInt("int_key")
            );
        }

        [Fact(DisplayName = "puts and gets double")]
        public void PutsAndGetsDouble() {
            JsonObject json = new();

            Assert.Null(json.GetDouble("double_key"));

            json.PutDouble("double_key", 123.456);

            Assert.Equal(
                123.456,
                json.GetDouble("double_key")
            );
        }

        [Fact(DisplayName = "puts and gets bool")]
        public void PutsAndGetsBool() {
            JsonObject json = new();

            Assert.Null(json.GetBool("bool_key"));

            json.PutBool("bool_key", false);

            Assert.Equal(
                false,
                json.GetBool("bool_key")
            );
        }

        // =================================================================================================================
        // Remove() tests
        // =================================================================================================================


        [Fact(DisplayName = "removes JsonObject")]
        public void RemovesJsonObject() {
            JsonObject json = new();

            json.PutJsonObject("object_key", new JsonObject(new Dictionary<string, object> {
                { "nested_key", "value" },
            }));

            Assert.Equal(
                """{"nested_key":"value"}""",
                json.RemoveJsonObject("object_key").ToString()
            );
            Assert.Null(json.GetJsonObject("object_key"));
            Assert.Null(json.RemoveJsonObject("object_key"));
        }

        [Fact(DisplayName = "removes JsonArray")]
        public void RemovesJsonArray() {
            JsonObject json = new();

            json.PutJsonArray("array_key", new JsonArray([
                "item_1", "item_2",
            ]));

            Assert.Equal(
                """["item_1","item_2"]""",
                json.RemoveJsonArray("array_key").ToString()
            );
            Assert.Null(json.GetJsonArray("array_key"));
            Assert.Null(json.RemoveJsonArray("array_key"));
        }

        [Fact(DisplayName = "removes string")]
        public void RemovesString() {
            JsonObject json = new();

            json.PutString("string_key", "value");

            Assert.Equal(
                "value",
                json.RemoveString("string_key")
            );
            Assert.Null(json.GetString("string_key"));
            Assert.Null(json.RemoveString("string_key"));
        }

        [Fact(DisplayName = "removes int")]
        public void RemovesInt() {
            JsonObject json = new();

            json.PutInt("int_key", 123);

            Assert.Equal(
                123,
                json.RemoveInt("int_key")
            );
            Assert.Null(json.GetInt("int_key"));
            Assert.Null(json.RemoveInt("int_key"));
        }

        [Fact(DisplayName = "removes double")]
        public void RemovesDouble() {
            JsonObject json = new();

            json.PutDouble("double_key", 123.456);

            Assert.Equal(
                123.456,
                json.RemoveDouble("double_key")
            );
            Assert.Null(json.GetDouble("double_key"));
            Assert.Null(json.RemoveDouble("double_key"));
        }

        [Fact(DisplayName = "removes bool")]
        public void RemovesBool() {
            JsonObject json = new();

            json.PutBool("bool_key", false);

            Assert.Equal(
                false,
                json.RemoveBool("bool_key")
            );
            Assert.Null(json.GetBool("bool_key"));
            Assert.Null(json.RemoveBool("bool_key"));
        }

        // =================================================================================================================
        // Seek() tests
        // =================================================================================================================

        [Fact(DisplayName = "seeks JsonObject shallow")]
        public void SeeksJsonObjectShallow() {
            JsonObject json = new(new Dictionary<string, object> {
                { "key", new Dictionary<string, object>() }
            });
            Assert.Equal("{}", json.SeekJsonObject("key")?.ToString());
        }

        [Fact(DisplayName = "seeks JsonObject deep")]
        public void SeeksJsonObjectDeep() {
            JsonObject json = new(new Dictionary<string, object> {
                { "key1", new Dictionary<string, object> {
                    { "key2", new List<object> { 
                        new Dictionary<string, object> {
                            { "key3", new Dictionary<string, object>() }
                        }
                    } }
                } }
            });
            Assert.Equal("{}", json.SeekJsonObject("key1.key2[0].key3")?.ToString());
        }

        [Fact(DisplayName = "seeks JsonArray shallow")]
        public void SeeksJsonArrayShallow() {
            JsonObject json = new(new Dictionary<string, object> {
                { "key", new List<object>() }
            });
            Assert.Equal("[]", json.SeekJsonArray("key")?.ToString());
        }

        [Fact(DisplayName = "seeks JsonArray deep")]
        public void SeeksJsonArrayDeep() {
            JsonObject json = new(new Dictionary<string, object> {
                { "key1", new Dictionary<string, object> {
                    { "key2", new List<object> {
                        new Dictionary<string, object> {
                            { "key3", new List<object>() }
                        }
                    } }
                } }
            });
            Assert.Equal("[]", json.SeekJsonArray("key1.key2[0].key3")?.ToString());
        }

        [Fact(DisplayName = "seeks string shallow")]
        public void SeeksStringShallow() {
            JsonObject json = new(new Dictionary<string, object> {
                { "key", "value" }
            });
            Assert.Equal("value", json.SeekString("key"));
        }

        [Fact(DisplayName = "seeks string deep")]
        public void SeeksStringDeep() {
            JsonObject json = new(new Dictionary<string, object> {
                { "key1", new Dictionary<string, object> {
                    { "key2", new List<object> {
                        new Dictionary<string, object> {
                            { "key3", "value" }
                        }
                    } }
                } }
            });
            Assert.Equal("value", json.SeekString("key1.key2[0].key3"));
        }

        [Fact(DisplayName = "seeks int shallow")]
        public void SeeksIntShallow() {
            JsonObject json = new(new Dictionary<string, object> {
                { "key", 123 }
            });
            Assert.Equal(123, json.SeekInt("key"));
        }

        [Fact(DisplayName = "seeks int deep")]
        public void SeeksIntDeep() {
            JsonObject json = new(new Dictionary<string, object> {
                { "key1", new Dictionary<string, object> {
                    { "key2", new List<object> {
                        new Dictionary<string, object> {
                            { "key3", 123 }
                        }
                    } }
                } }
            });
            Assert.Equal(123, json.SeekInt("key1.key2[0].key3"));
        }

        [Fact(DisplayName = "seeks double shallow")]
        public void SeeksDoubleShallow() {
            JsonObject json = new(new Dictionary<string, object> {
                { "key", 123.456 }
            });
            Assert.Equal(123.456, json.SeekDouble("key"));
        }

        [Fact(DisplayName = "seeks double deep")]
        public void SeeksDoubleDeep() {
            JsonObject json = new(new Dictionary<string, object> {
                { "key1", new Dictionary<string, object> {
                    { "key2", new List<object> {
                        new Dictionary<string, object> {
                            { "key3", 123.456 }
                        }
                    } }
                } }
            });
            Assert.Equal(123.456, json.SeekDouble("key1.key2[0].key3"));
        }

        [Fact(DisplayName = "seeks bool shallow")]
        public void SeeksBoolShallow() {
            JsonObject json = new(new Dictionary<string, object> {
                { "key", false }
            });
            Assert.False(json.SeekBool("key"));
        }

        [Fact(DisplayName = "seeks bool deep")]
        public void SeeksBoolDeep() {
            JsonObject json = new(new Dictionary<string, object> {
                { "key1", new Dictionary<string, object> {
                    { "key2", new List<object> {
                        new Dictionary<string, object> {
                            { "key3", false }
                        }
                    } }
                } }
            });
            Assert.False(json.SeekBool("key1.key2[0].key3"));
        }

        // =================================================================================================================
        // Insert() tests
        // =================================================================================================================

        [Fact(DisplayName = "inserts JsonObject shallow")]
        public void InsertsJsonObjectShallow() {
            JsonObject json = new();
            json.InsertJsonObject("key", new());

            string expected = """
                {
                  "key": { }
                }
                """;
            Assert.Equal(expected, json.PrettyPrint());
        }

        [Fact(DisplayName = "inserts JsonObject deep")]
        public void InsertsJsonObjectDeep() {
            JsonObject json = new(new Dictionary<string, object> {
                { "key1", new Dictionary<string, object> {
                    { "key2", new List<object> {
                        new Dictionary<string, object>()
                    } }
                } }
            });
            json.InsertJsonObject("key1.key2[0].key3", new());

            string expected = """
                {
                  "key1": {
                    "key2": [
                      {
                        "key3": { }
                      }
                    ]
                  }
                }
                """;
            Assert.Equal(expected, json.PrettyPrint());
        }

        [Fact(DisplayName = "inserts JsonArray shallow")]
        public void InsertsJsonArrayShallow() {
            JsonObject json = new();
            json.InsertJsonArray("key", new());

            string expected = """
                {
                  "key": [ ]
                }
                """;
            Assert.Equal(expected, json.PrettyPrint());
        }

        [Fact(DisplayName = "inserts JsonArray deep")]
        public void InsertsJsonArrayDeep() {
            JsonObject json = new(new Dictionary<string, object> {
                { "key1", new Dictionary<string, object> {
                    { "key2", new List<object> {
                        new Dictionary<string, object>()
                    } }
                } }
            });
            json.InsertJsonArray("key1.key2[0].key3", new());

            string expected = """
                {
                  "key1": {
                    "key2": [
                      {
                        "key3": [ ]
                      }
                    ]
                  }
                }
                """;
            Assert.Equal(expected, json.PrettyPrint());
        }

        // =================================================================================================================
        // Join() tests
        // =================================================================================================================

        // =================================================================================================================
        // content-checking tests
        // =================================================================================================================

        [Fact(DisplayName = "identifies emptiness")]
        public void IdentifiesEmptiness() {
            JsonObject json = new();
            Assert.True(json.IsEmpty());
            json.PutBool("bool_key", false);
            Assert.False(json.IsEmpty());
        }

        // =================================================================================================================
        // printing tests
        // =================================================================================================================






    }
}