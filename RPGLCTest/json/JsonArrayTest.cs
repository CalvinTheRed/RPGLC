using json;

namespace json.test {
    public class JsonArrayTest {

        [Fact(DisplayName = "prints as string")]
        public void PrintsAsString() {
            JsonArray json = new([
                new Dictionary<string, object> {
                    { "nested_key", "value" },
                },
                new List<object> { "item_1", "item_2" },
                "value",
                123,
                123.456,
                false,
            ]);

            string expected = """
                [{"nested_key":"value"},["item_1","item_2"],"value",123,123.456,false]
                """;
            Assert.Equal(expected, json.ToString());
        }

        [Fact(DisplayName = "prints as string when empty")]
        public void PrintsAsStringWhenEmpty() {
            JsonArray json = new();
            Assert.Equal("[]", json.ToString());
        }

        [Fact(DisplayName = "pretty prints")]
        public void PrettyPrints() {
            JsonArray json = new([
                new Dictionary<string, object> {
                    { "nested_key", "value" },
                },
                new List<object> { "item_1", "item_2" },
                "value",
                123,
                123.456,
                false,
            ]);

            string expected = """
                [
                  {
                    "nested_key": "value"
                  },
                  [
                    "item_1",
                    "item_2"
                  ],
                  "value",
                  123,
                  123.456,
                  false
                ]
                """;
            Assert.Equal(expected, json.PrettyPrint());
        }

        [Fact(DisplayName = "pretty prints when empty")]
        public void PrettyPrintsWhenEmpty() {
            JsonArray json = new();
            Assert.Equal("[ ]", json.PrettyPrint());
        }

        [Fact(DisplayName = "adds and gets JsonObject")]
        public void AddsAndGetsJsonObject() {
            JsonArray json = new();

            Assert.Null(json.GetJsonObject(0));

            json.AddJsonObject(new JsonObject(new Dictionary<string, object> {
                { "nested_key", "value" },
            }));

            Assert.Equal(
                """{"nested_key":"value"}""",
                json.GetJsonObject(0).ToString()
            );
        }

        [Fact(DisplayName = "removes JsonObject")]
        public void RemovesJsonObject() {
            JsonArray json = new();

            json.AddJsonObject(new JsonObject(new Dictionary<string, object> {
                { "nested_key", "value" },
            }));

            Assert.Equal(
                """{"nested_key":"value"}""",
                json.RemoveJsonObject(0).ToString()
            );
            Assert.Null(json.GetJsonObject(0));
            Assert.Null(json.RemoveJsonObject(0));
        }

        [Fact(DisplayName = "adds and gets JsonArray")]
        public void AddsAndGetsJsonArray() {
            JsonArray json = new();

            Assert.Null(json.GetJsonObject(0));

            json.AddJsonArray(new JsonArray([
                "item_1", "item_2",
            ]));

            Assert.Equal(
                """["item_1","item_2"]""",
                json.GetJsonArray(0).ToString()
            );
        }

        [Fact(DisplayName = "removes JsonArray")]
        public void RemovesJsonArray() {
            JsonArray json = new();

            json.AddJsonArray(new JsonArray([
                "item_1", "item_2",
            ]));

            Assert.Equal(
                """["item_1","item_2"]""",
                json.RemoveJsonArray(0).ToString()
            );
            Assert.Null(json.GetJsonArray(0));
            Assert.Null(json.RemoveJsonArray(0));
        }

        [Fact(DisplayName = "adds and gets string")]
        public void AddsAndGetsString() {
            JsonArray json = new();

            Assert.Null(json.GetString(0));

            json.AddString("value");

            Assert.Equal(
                "value",
                json.GetString(0)
            );
        }

        [Fact(DisplayName = "removes string")]
        public void RemovesString() {
            JsonArray json = new();
            json.AddString("value");
            Assert.Equal(
                "value",
                json.RemoveString(0)
            );
            Assert.Null(json.GetString(0));
            Assert.Null(json.RemoveString(0));
        }

        [Fact(DisplayName = "adds and gets int")]
        public void AddsAndGetsInt() {
            JsonArray json = new();

            Assert.Null(json.GetInt(0));

            json.AddInt(123);

            Assert.Equal(
                123,
                json.GetInt(0)
            );
        }

        [Fact(DisplayName = "removes int")]
        public void RemovesInt() {
            JsonArray json = new();

            json.AddInt(123);

            Assert.Equal(
                123,
                json.RemoveInt(0)
            );
            Assert.Null(json.GetInt(0));
            Assert.Null(json.RemoveInt(0));
        }

        [Fact(DisplayName = "adds and gets double")]
        public void AddsAndGetsDouble() {
            JsonArray json = new();

            Assert.Null(json.GetDouble(0));

            json.AddDouble(123.456);

            Assert.Equal(
                123.456,
                json.GetDouble(0)
            );
        }

        [Fact(DisplayName = "removes double")]
        public void RemovesDouble() {
            JsonArray json = new();

            json.AddDouble(123.456);

            Assert.Equal(
                123.456,
                json.RemoveDouble(0)
            );
            Assert.Null(json.GetDouble(0));
            Assert.Null(json.RemoveDouble(0));
        }

        [Fact(DisplayName = "adds and gets bool")]
        public void AddsAndGetsBool() {
            JsonArray json = new();

            Assert.Null(json.GetBool(0));

            json.AddBool(false);

            Assert.Equal(
                false,
                json.GetBool(0)
            );
        }

        [Fact(DisplayName = "removes bool")]
        public void RemovesBool() {
            JsonArray json = new();

            json.AddBool(false);

            Assert.Equal(
                false,
                json.RemoveBool(0)
            );
            Assert.Null(json.GetBool(0));
            Assert.Null(json.RemoveBool(0));
        }

        [Fact(DisplayName = "identifies emptiness")]
        public void IdentifiesEmptiness() {
            JsonArray json = new();
            Assert.True(json.IsEmpty());
            json.AddBool(false);
            Assert.False(json.IsEmpty());
        }
    }
}