using LiteDB;
using System.Linq.Expressions;

using com.rpglc.json;
using com.rpglc.database.TO;

namespace com.rpglc.database;

public class DBManager {
    private static string dbDir;
    private static string dbName;

    public static void SetDatabase(string dbDir, string dbName) {
        DBManager.dbDir = dbDir;
        DBManager.dbName = dbName;
    }

    public static void LoadDatapacks(string path) {
        foreach (string datapackPath in Directory.GetDirectories(path)) {
            _ = new Datapack(datapackPath);
        }
    }

    private static void SwapArraysForLists(Dictionary<string, object> data) {
        foreach (string key in data.Keys) {
            if (data[key] is Dictionary<string, object> dict) {
                SwapArraysForLists(dict);
            } else if (data[key] is Object[] array) {
                List<object> list = new(array);
                data[key] = list;
                SwapArraysForLists(list);
            } else if (data[key] is List<object> list) {
                SwapArraysForLists(list);
            }
        }
    }

    private static void SwapArraysForLists(List<object> list) {
        for (int i = 0; i < list.Count; i++) {
            if (list[i] is Dictionary<string, object> dict) {
                SwapArraysForLists(dict);
            } else if (list[i] is Object[] array) {
                List<object> newList = new(array);
                list[i] = newList;
                SwapArraysForLists(newList);
            } else if (list[i] is List<object> nestedList) {
                SwapArraysForLists(nestedList);
            }
        }
    }

    public static void InsertRPGLEffectTemplate(JsonObject data) {
        using DBConnection connection = new(dbDir, dbName);
        connection.Collection<RPGLEffectTemplateTO>("effect_templates").Insert(new RPGLEffectTemplateTO() {
            metadata = data.GetJsonObject("metadata").AsDict(),
            name = data.GetString("name"),
            description = data.GetString("description"),
            datapackId = data.GetString("datapack_id"),

            subeventFilters = data.GetJsonObject("subevent_filters").AsDict(),
        });
    }

    public static void InsertRPGLEventTemplate(JsonObject data) {
        using DBConnection connection = new(dbDir, dbName);
        connection.Collection<RPGLEventTemplateTO>("event_templates").Insert(new RPGLEventTemplateTO() {
            metadata = data.GetJsonObject("metadata").AsDict(),
            name = data.GetString("name"),
            description = data.GetString("description"),
            datapackId = data.GetString("datapack_id"),

            areaOfEffect = data.GetJsonObject("area_of_effect").AsDict(),
            subevents = data.GetJsonArray("subevents").AsList(),
            cost = data.GetJsonArray("cost").AsList(),
            // origin item not included in templates
        });
    }

    public static void InsertRPGLItemTemplate(JsonObject data) {
        using DBConnection connection = new(dbDir, dbName);
        connection.Collection<RPGLItemTemplateTO>("item_templates").Insert(new RPGLItemTemplateTO() {
            metadata = data.GetJsonObject("metadata").AsDict(),
            name = data.GetString("name"),
            description = data.GetString("description"),
            datapackId = data.GetString("datapack_id"),

            tags = data.GetJsonArray("tags").AsList(),

            weight = data.GetInt("weight"),
            cost = data.GetInt("cost"),
            effects = data.GetJsonArray("effects").AsList(),
            events = data.GetJsonArray("events").AsList(),
            resources = data.GetJsonArray("resources").AsList(),
        });
    }

    public static void InsertRPGLObjectTemplate(JsonObject data) {
        using DBConnection connection = new(dbDir, dbName);
        connection.Collection<RPGLObjectTemplateTO>("object_templates").Insert(new RPGLObjectTemplateTO() {
            metadata = data.GetJsonObject("metadata").AsDict(),
            name = data.GetString("name"),
            description = data.GetString("description"),
            datapackId = data.GetString("datapack_id"),

            tags = data.GetJsonArray("tags").AsList(),

            abilityScores = data.GetJsonObject("ability_scores").AsDict(),
            equippedItems = data.GetJsonObject("equipped_items").AsDict(),
            classes = data.GetJsonArray("classes").AsList(),
            effects = data.GetJsonArray("effects").AsList(),
            events = data.GetJsonArray("events").AsList(),
            inventory = data.GetJsonArray("inventory").AsList(),
            races = data.GetJsonArray("races").AsList(),
            resources = data.GetJsonArray("resources").AsList(),
            // position is not included in template data
            // rotation is not included in template data
            // origin object is not included in template data
            // proxy object is not included in template data
            // user id is not included in template data
            healthBase = data.GetInt("health_base"),
            healthCurrent = data.GetInt("health_current"),
            healthTemporary = data.GetInt("health_temporary"),
            proficiencyBonus = data.GetInt("proficiency_bonus"),
        });
    }

    public static void InsertRPGLResourceTemplate(JsonObject data) {
        using DBConnection connection = new(dbDir, dbName);
        connection.Collection<RPGLResourceTemplateTO>("resource_templates").Insert(new RPGLResourceTemplateTO() {
            metadata = data.GetJsonObject("metadata").AsDict(),
            name = data.GetString("name"),
            description = data.GetString("description"),
            datapackId = data.GetString("datapack_id"),

            tags = data.GetJsonArray("tags").AsList(),

            refreshCriterion = data.GetJsonArray("refresh_criterion").AsList(),
            // origin item is not included in template data
            potency = data.GetInt("potency"),
            // exhausted is not included in template data
        });
    }

    public static RPGLEffectTemplate QueryRPGLEffectTemplateByDatapackId(string datapackId) {
        RPGLEffectTemplate template;
        using (DBConnection connection = new(dbDir, dbName)) {
            template = connection.Collection<RPGLEffectTemplateTO>("effect_templates")
                .FindOne(x => x.datapackId == datapackId)
                .ToTemplate();
        }
        SwapArraysForLists(template.AsDict());
        return template;
    }

    public static RPGLEventTemplate QueryRPGLEventTemplateByDatapackId(string datapackId) {
        RPGLEventTemplate template;
        using (DBConnection connection = new(dbDir, dbName)) {
            template = connection.Collection<RPGLEventTemplateTO>("event_templates")
                .FindOne(x => x.datapackId == datapackId)
                .ToTemplate();
        }
        SwapArraysForLists(template.AsDict());
        return template;
    }

    public static RPGLItemTemplate QueryRPGLItemTemplateByDatapackId(string datapackId) {
        RPGLItemTemplate template;
        using (DBConnection connection = new(dbDir, dbName)) {
            template = connection.Collection<RPGLItemTemplateTO>("item_templates")
                .FindOne(x => x.datapackId == datapackId)
                .ToTemplate();
        }
        SwapArraysForLists(template.AsDict());
        return template;
    }

    public static RPGLObjectTemplate QueryRPGLObjectTemplateByDatapackId(string datapackId) {
        RPGLObjectTemplate template;
        using (DBConnection connection = new(dbDir, dbName)) {
            template = connection.Collection<RPGLObjectTemplateTO>("object_templates")
                .FindOne(x => x.datapackId == datapackId)
                .ToTemplate();
        }
        SwapArraysForLists(template.AsDict());
        return template;
    }

    public static RPGLResourceTemplate QueryRPGLResourceTemplateByDatapackId(string datapackId) {
        RPGLResourceTemplate template;
        using (DBConnection connection = new(dbDir, dbName)) {
            template = connection.Collection<RPGLResourceTemplateTO>("resource_templates")
                .FindOne(x => x.datapackId == datapackId)
                .ToTemplate();
        }
        SwapArraysForLists(template.AsDict());
        return template;
    }

    public static int DeleteCollection<T>(string collectionName) {
        int numDeleted;
        using (DBConnection connection = new(dbDir, dbName)) {
            numDeleted = connection.Collection<T>(collectionName).DeleteAll();
        }
        return numDeleted;
    }





    public static void InsertRPGLDummyType(JsonObject data) {
        using DBConnection connection = new(dbDir, dbName);
        connection.Collection<RPGLDummyType>("dummies").Insert(new RPGLDummyType() {
            name = data.GetString("name"),
            health = data.GetInt("health"),
            userId = data.GetString("user_id"),
            uuid = data.GetString("uuid"),
        });
    }

    public static void DeleteRPGLDummyType(ObjectId id) {
        using DBConnection conn = new(dbDir, dbName);
        conn.Collection<RPGLDummyType>("dummies").Delete(id);
    }

    public static List<JsonObject> QueryDummies(Expression<Func<RPGLDummyType, bool>> expression) {
        List<JsonObject> list = [];
        using (DBConnection conn = new(dbDir, dbName)) {
            conn.Collection<RPGLDummyType>("dummies").Query()
                .Where(expression)
                .ToList()
                .ForEach(x => list.Add(new JsonObject()
                    .PutString("name", x.name)
                    .PutInt("health", x.health)
                    .PutString("user_id", x.userId)
                    .PutString("uuid", x._id.ToString())
                ));
        }
        return list;
    }
};

public class DBConnection : IDisposable {
    private readonly LiteDatabase db;

    public DBConnection(string dbDir, string dbName) {
        if (!Directory.Exists(dbDir)) {
            Directory.CreateDirectory(dbDir);
        }
        string dbPath = Path.Combine(dbDir, dbName);
        if (!File.Exists(dbPath)) {
        }
        this.db = new LiteDatabase(dbPath);
    }

    public void Dispose() {
        this.db.Dispose();
    }

    public ILiteCollection<T> Collection<T>(string name) {
        return this.db.GetCollection<T>(name);
    }
};

public class RPGLDummyType {
    public ObjectId _id { get; set; }

    public string name { get; set; }
    public Int64 health { get; set; }
    public string userId { get; set; }
    public string uuid { get; set; }
};
