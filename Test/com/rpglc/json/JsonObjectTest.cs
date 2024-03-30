using com.rpglc.json;

namespace RPGLC.json.test;

public class JsonObjectTest {

    // =================================================================================================================
    // constructor tests
    // =================================================================================================================

    [Fact(DisplayName = "loads from string")]
    public void LoadsFromString() {
        string jsonString = """
            {
              "object_key": {
                "object_key": {
                  "object_key": {},
                  "array_key": [],
                  "string_key": "value",
                  "int_key": 123,
                  "double_key": 123.456,
                  "bool_key": false
                },
                "array_key": [
                  {},
                  [],
                  "value",
                  123,
                  123.456,
                  false
                ],
                "string_key": "value",
                "int_key": 123,
                "double_key": 123.456,
                "bool_key": false
              },
              "array_key": [
                {
                  "object_key": {},
                  "array_key": [],
                  "string_key": "value",
                  "int_key": 123,
                  "double_key": 123.456,
                  "bool_key": false
                },
                [
                  {},
                  [],
                  "value",
                  123,
                  123.456,
                  false
                ],
                "value",
                123,
                123.456,
                false
              ],
              "string_key": "value",
              "int_key": 123,
              "double_key": 123.456,
              "bool_key": false
            }
            """;

        JsonObject json = new JsonObject().LoadFromString(jsonString);

        string expected = """
            {
              "array_key": [
                {
                  "array_key": [ ],
                  "bool_key": false,
                  "double_key": 123.456,
                  "int_key": 123,
                  "object_key": { },
                  "string_key": "value"
                },
                [
                  { },
                  [ ],
                  "value",
                  123,
                  123.456,
                  false
                ],
                "value",
                123,
                123.456,
                false
              ],
              "bool_key": false,
              "double_key": 123.456,
              "int_key": 123,
              "object_key": {
                "array_key": [
                  { },
                  [ ],
                  "value",
                  123,
                  123.456,
                  false
                ],
                "bool_key": false,
                "double_key": 123.456,
                "int_key": 123,
                "object_key": {
                  "array_key": [ ],
                  "bool_key": false,
                  "double_key": 123.456,
                  "int_key": 123,
                  "object_key": { },
                  "string_key": "value"
                },
                "string_key": "value"
              },
              "string_key": "value"
            }
            """;

        Assert.Equal(expected, json.PrettyPrint());

        Assert.NotNull(json.SeekJsonObject("object_key.object_key.object_key"));
        Assert.NotNull(json.SeekJsonArray("object_key.object_key.array_key"));
        Assert.NotNull(json.SeekString("object_key.object_key.string_key"));
        Assert.NotNull(json.SeekInt("object_key.object_key.int_key"));
        Assert.NotNull(json.SeekDouble("object_key.object_key.double_key"));
        Assert.NotNull(json.SeekBool("object_key.object_key.bool_key"));

        Assert.NotNull(json.SeekJsonObject("object_key.array_key[0]"));
        Assert.NotNull(json.SeekJsonArray("object_key.array_key[1]"));
        Assert.NotNull(json.SeekString("object_key.array_key[2]"));
        Assert.NotNull(json.SeekInt("object_key.array_key[3]"));
        Assert.NotNull(json.SeekDouble("object_key.array_key[4]"));
        Assert.NotNull(json.SeekBool("object_key.array_key[5]"));
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
            json.GetJsonObject("object_key")?.ToString()
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
            json.GetJsonArray("array_key")?.ToString()
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

        json.PutJsonObject("key", new());

        string expected = "{}";
        Assert.Equal(expected, json.RemoveJsonObject("key")?.ToString());
        Assert.Null(json.GetJsonObject("key"));
        Assert.Null(json.RemoveJsonObject("key"));
    }

    [Fact(DisplayName = "removes JsonArray")]
    public void RemovesJsonArray() {
        JsonObject json = new();

        json.PutJsonArray("key", new());

        string expected = "[]";
        Assert.Equal(expected, json.RemoveJsonArray("key")?.ToString());
        Assert.Null(json.GetJsonArray("key"));
        Assert.Null(json.RemoveJsonArray("key"));
    }

    [Fact(DisplayName = "removes string")]
    public void RemovesString() {
        JsonObject json = new();

        json.PutString("key", "value");

        string expected = "value";
        Assert.Equal(expected, json.RemoveString("key"));
        Assert.Null(json.GetString("key"));
        Assert.Null(json.RemoveString("key"));
    }

    [Fact(DisplayName = "removes int")]
    public void RemovesInt() {
        JsonObject json = new();

        json.PutInt("key", 123);

        int expected = 123;
        Assert.Equal(expected, json.RemoveInt("key"));
        Assert.Null(json.GetInt("key"));
        Assert.Null(json.RemoveInt("key"));
    }

    [Fact(DisplayName = "removes double")]
    public void RemovesDouble() {
        JsonObject json = new();

        json.PutDouble("key", 123.456);

        double expected = 123.456;
        Assert.Equal(expected, json.RemoveDouble("key"));
        Assert.Null(json.GetDouble("key"));
        Assert.Null(json.RemoveDouble("key"));
    }

    [Fact(DisplayName = "removes bool")]
    public void RemovesBool() {
        JsonObject json = new();

        json.PutBool("key", false);

        bool expected = false;
        Assert.Equal(expected, json.RemoveBool("key"));
        Assert.Null(json.GetBool("key"));
        Assert.Null(json.RemoveBool("key"));
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

    [Fact(DisplayName = "joins shallow")]
    public void JoinsShallow() {
        JsonObject json = new(new Dictionary<string, object> {
            { "object_key_1", new Dictionary<string, object>() },
            { "array_key_1", new List<object>() },
            { "string_key_1", "value" },
            { "int_key_1", 123 },
            { "double_key_1", 123.456 },
            { "bool_key_1", false },
        });
        json.Join(new Dictionary<string, object> {
            { "object_key_2", new Dictionary<string, object>() },
            { "array_key_2", new List<object>() },
            { "string_key_2", "value" },
            { "int_key_2", 123 },
            { "double_key_2", 123.456 },
            { "bool_key_2", false },
        });

        string expected = """
            {
              "array_key_1": [ ],
              "array_key_2": [ ],
              "bool_key_1": false,
              "bool_key_2": false,
              "double_key_1": 123.456,
              "double_key_2": 123.456,
              "int_key_1": 123,
              "int_key_2": 123,
              "object_key_1": { },
              "object_key_2": { },
              "string_key_1": "value",
              "string_key_2": "value"
            }
            """;
        Assert.Equal(expected, json.PrettyPrint());
    }

    [Fact(DisplayName = "joins deep")]
    public void JoinsDeep() {
        JsonObject json = new(new Dictionary<string, object> {
            { "key", new Dictionary<string, object> {
                { "object_key_1", new Dictionary<string, object>() },
                { "array_key_1", new List<object>() },
                { "string_key_1", "value" },
                { "int_key_1", 123 },
                { "double_key_1", 123.456 },
                { "bool_key_1", false },
            } },
        });
        json.Join(new Dictionary<string, object> {
            { "key", new Dictionary<string, object> {
                { "object_key_2", new Dictionary<string, object>() },
                { "array_key_2", new List<object>() },
                { "string_key_2", "value" },
                { "int_key_2", 123 },
                { "double_key_2", 123.456 },
                { "bool_key_2", false },
            } },
        });

        string expected = """
            {
              "key": {
                "array_key_1": [ ],
                "array_key_2": [ ],
                "bool_key_1": false,
                "bool_key_2": false,
                "double_key_1": 123.456,
                "double_key_2": 123.456,
                "int_key_1": 123,
                "int_key_2": 123,
                "object_key_1": { },
                "object_key_2": { },
                "string_key_1": "value",
                "string_key_2": "value"
              }
            }
            """;
        Assert.Equal(expected, json.PrettyPrint());
    }

    [Fact(DisplayName = "joins with collisions")]
    public void JoinsWithCollisions() {
        JsonObject json = new(new Dictionary<string, object> {
            { "object_key", new Dictionary<string, object> {
                { "key1", "value 1" }
            } },
            { "array_key", new List<object> {
                "old value"
            } },
            { "string_key", "value 1" },
            { "int_key", 1234 },
            { "double_key", 12.34 },
            { "bool_key", false },
        });
        json.Join(new Dictionary<string, object> {
            { "object_key", new Dictionary<string, object> {
                { "key2", "value 2" }
            } },
            { "array_key", new List<object> {
                "new value"
            } },
            { "string_key", "value 2" },
            { "int_key", 5678 },
            { "double_key", 56.78 },
            { "bool_key", true },
        });

        string expected = """
            {
              "array_key": [
                "old value",
                "new value"
              ],
              "bool_key": true,
              "double_key": 56.78,
              "int_key": 5678,
              "object_key": {
                "key1": "value 1",
                "key2": "value 2"
              },
              "string_key": "value 2"
            }
            """;
        Assert.Equal(expected, json.PrettyPrint());
    }

    // =================================================================================================================
    // content-checking tests
    // =================================================================================================================

    [Fact(DisplayName = "returns correct size")]
    public void ReturnsCorrectSize() {
        JsonObject json = new();
        Assert.Equal(0, json.Count());
        json.PutString("key", "value");
        Assert.Equal(1, json.Count());
    }

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
}