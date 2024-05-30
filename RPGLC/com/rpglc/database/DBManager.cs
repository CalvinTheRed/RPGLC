using LiteDB;

using com.rpglc.json;
using com.rpglc.database.TO;
using com.rpglc.core;

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
            } else if (data[key] is object[] array) {
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
            } else if (list[i] is object[] array) {
                List<object> newList = new(array);
                list[i] = newList;
                SwapArraysForLists(newList);
            } else if (list[i] is List<object> nestedList) {
                SwapArraysForLists(nestedList);
            }
        }
    }

    public static void InsertRPGLClass(JsonObject data) {
        using DBConnection connection = new(dbDir, dbName);
        connection.Collection<RPGLClassTO>("classes").Insert(new RPGLClassTO() {
            DatapackId = data.GetString("datapack_id"),
            Description = data.GetString("description"),
            Metadata = data.GetJsonObject("metadata").AsDict(),
            Name = data.GetString("name"),

            StartingFeatures = data.GetJsonObject("starting_features")?.AsDict(),
            Features = data.GetJsonObject("features").AsDict(),
            NestedClasses = data.GetJsonObject("nested_classes").AsDict(),
            AbilityScoreIncreaseLevels = data.GetJsonArray("ability_score_increase_levels")?.AsList(),
            MulticlassRequirements = data.GetJsonArray("multiclass_requirements")?.AsList(),
            SubclassLevel = data.GetInt("subclass_level"),
        });
    }

    public static void InsertRPGLRace(JsonObject data) {
        using DBConnection connection = new(dbDir, dbName);
        connection.Collection<RPGLRaceTO>("races").Insert(new RPGLRaceTO() {
            DatapackId = data.GetString("datapack_id"),
            Description = data.GetString("description"),
            Metadata = data.GetJsonObject("metadata").AsDict(),
            Name = data.GetString("name"),

            AbilityScoreBonuses = data.GetJsonObject("ability_score_bonuses").AsDict(),
            Features = data.GetJsonObject("features").AsDict(),
        });
    }

    public static void InsertRPGLEffectTemplate(JsonObject data) {
        using DBConnection connection = new(dbDir, dbName);
        connection.Collection<RPGLEffectTemplateTO>("effect_templates").Insert(new RPGLEffectTemplateTO() {
            DatapackId = data.GetString("datapack_id"),
            Description = data.GetString("description"),
            Metadata = data.GetJsonObject("metadata").AsDict(),
            Name = data.GetString("name"),

            SubeventFilters = data.GetJsonObject("subevent_filters").AsDict(),
        });
    }

    public static void InsertRPGLEventTemplate(JsonObject data) {
        using DBConnection connection = new(dbDir, dbName);
        connection.Collection<RPGLEventTemplateTO>("event_templates").Insert(new RPGLEventTemplateTO() {
            DatapackId = data.GetString("datapack_id"),
            Description = data.GetString("description"),
            Metadata = data.GetJsonObject("metadata").AsDict(),
            Name = data.GetString("name"),

            AreaOfEffect = data.GetJsonObject("area_of_effect").AsDict(),
            Cost = data.GetJsonArray("cost").AsList(),
            Subevents = data.GetJsonArray("subevents").AsList(),
            // origin item is not included in templates
        });
    }

    public static void InsertRPGLItemTemplate(JsonObject data) {
        using DBConnection connection = new(dbDir, dbName);
        connection.Collection<RPGLItemTemplateTO>("item_templates").Insert(new RPGLItemTemplateTO() {
            DatapackId = data.GetString("datapack_id"),
            Description = data.GetString("description"),
            Metadata = data.GetJsonObject("metadata").AsDict(),
            Name = data.GetString("name"),

            Tags = data.GetJsonArray("tags").AsList(),

            Effects = data.GetJsonArray("effects").AsList(),
            Events = data.GetJsonArray("events").AsList(),
            Resources = data.GetJsonArray("resources").AsList(),
            Cost = (long) data.GetInt("cost"),
            Weight = (long) data.GetInt("weight"),
        });
    }

    public static void InsertRPGLObjectTemplate(JsonObject data) {
        using DBConnection connection = new(dbDir, dbName);
        connection.Collection<RPGLObjectTemplateTO>("object_templates").Insert(new RPGLObjectTemplateTO() {
            DatapackId = data.GetString("datapack_id"),
            Description = data.GetString("description"),
            Metadata = data.GetJsonObject("metadata").AsDict(),
            Name = data.GetString("name"),

            Tags = data.GetJsonArray("tags").AsList(),

            AbilityScores = data.GetJsonObject("ability_scores").AsDict(),
            EquippedItems = data.GetJsonObject("equipped_items").AsDict(),
            Classes = data.GetJsonArray("classes").AsList(),
            Effects = data.GetJsonArray("effects").AsList(),
            Events = data.GetJsonArray("events").AsList(),
            Inventory = data.GetJsonArray("inventory").AsList(),
            // position is not included in template data
            Races = data.GetJsonArray("races").AsList(),
            Resources = data.GetJsonArray("resources").AsList(),
            // rotation is not included in template data
            // origin object is not included in template data
            // proxy object is not included in template data
            // user id is not included in template data
            HealthBase = (long) data.GetInt("health_base"),
            HealthCurrent = (long) data.GetInt("health_current"),
            HealthTemporary = (long) data.GetInt("health_temporary"),
            ProficiencyBonus = (long) data.GetInt("proficiency_bonus"),
        });
    }

    public static void InsertRPGLResourceTemplate(JsonObject data) {
        using DBConnection connection = new(dbDir, dbName);
        connection.Collection<RPGLResourceTemplateTO>("resource_templates").Insert(new RPGLResourceTemplateTO() {
            DatapackId = data.GetString("datapack_id"),
            Description = data.GetString("description"),
            Metadata = data.GetJsonObject("metadata").AsDict(),
            Name = data.GetString("name"),

            Tags = data.GetJsonArray("tags").AsList(),

            RefreshCriterion = data.GetJsonArray("refresh_criterion").AsList(),
            // origin item is not included in template data
            Potency = (long) data.GetInt("potency"),
            // exhausted is not included in template data
        });
    }

    public static void InsertRPGLEffect(RPGLEffect rpglEffect) {
        using DBConnection connection = new(dbDir, dbName);
        connection.Collection<RPGLEffectTO>("effects").Insert(new RPGLEffectTO() {
            DatapackId = rpglEffect.GetDatapackId(),
            Description = rpglEffect.GetDescription(),
            Metadata = rpglEffect.GetMetadata().AsDict(),
            Name = rpglEffect.GetName(),

            Uuid = rpglEffect.GetUuid(),

            SubeventFilters = rpglEffect.GetSubeventFilters().AsDict(),
            OriginItem = rpglEffect.GetOriginItem(),
            Source = rpglEffect.GetSource(),
            Target = rpglEffect.GetTarget(),
        });
    }

    public static void InsertRPGLItem(RPGLItem rpglItem) {
        using DBConnection connection = new(dbDir, dbName);
        connection.Collection<RPGLItemTO>("items").Insert(new RPGLItemTO() {
            DatapackId = rpglItem.GetDatapackId(),
            Description = rpglItem.GetDescription(),
            Metadata = rpglItem.GetMetadata().AsDict(),
            Name = rpglItem.GetName(),

            Tags = rpglItem.GetTags().AsList(),

            Uuid = rpglItem.GetUuid(),

            Effects = rpglItem.GetEffects().AsList(),
            Events = rpglItem.GetEvents().AsList(),
            Resources = rpglItem.GetResources().AsList(),
            Cost = rpglItem.GetCost(),
            Weight = rpglItem.GetWeight(),
        });
    }

    public static void InsertRPGLObject(RPGLObject rpglObject) {
        using DBConnection connection = new(dbDir, dbName);
        connection.Collection<RPGLObjectTO>("objects").Insert(new RPGLObjectTO() {
            DatapackId = rpglObject.GetDatapackId(),
            Description = rpglObject.GetDescription(),
            Metadata = rpglObject.GetMetadata().AsDict(),
            Name = rpglObject.GetName(),

            Tags = rpglObject.GetTags().AsList(),

            Uuid = rpglObject.GetUuid(),

            AbilityScores = rpglObject.GetAbilityScores().AsDict(),
            EquippedItems = rpglObject.GetEquippedItems().AsDict(),
            Classes = rpglObject.GetClasses().AsList(),
            Events = rpglObject.GetEvents().AsList(),
            Inventory = rpglObject.GetInventory().AsList(),
            Position = rpglObject.GetPosition().AsList(),
            Races = rpglObject.GetRaces().AsList(),
            Resources = rpglObject.GetResources().AsList(),
            Rotation = rpglObject.GetRotation().AsList(),
            OriginObject = rpglObject.GetOriginObject(),
            ProxyObject = rpglObject.GetProxyObject(),
            UserId = rpglObject.GetUserId(),
            HealthBase = rpglObject.GetHealthBase(),
            HealthCurrent = rpglObject.GetHealthCurrent(),
            HealthTemporary = rpglObject.GetHealthTemporary(),
            ProficiencyBonus = rpglObject.GetProficiencyBonus(),
        });
    }

    public static void InsertRPGLResource(RPGLResource rpglResource) {
        using DBConnection connection = new(dbDir, dbName);
        connection.Collection<RPGLResourceTO>("resources").Insert(new RPGLResourceTO() {
            DatapackId = rpglResource.GetDatapackId(),
            Description = rpglResource.GetDescription(),
            Metadata = rpglResource.GetMetadata().AsDict(),
            Name = rpglResource.GetName(),

            Tags = rpglResource.GetTags().AsList(),

            Uuid = rpglResource.GetUuid(),

            RefreshCriterion = rpglResource.GetRefreshCriterion().AsList(),
            OriginItem = rpglResource.GetOriginItem(),
            Potency = rpglResource.GetPotency(),
            Exhausted = false, // new RPGLResource should never be exhausted
        });
    }

    public static RPGLClass QueryRPGLClassByDatapackId(string datapackId) {
        RPGLClass rpglClass;
        using (DBConnection connection = new(dbDir, dbName)) {
            rpglClass = connection.Collection<RPGLClassTO>("classes")
                .FindOne(x => x.DatapackId == datapackId)
                .ToRPGLClass();
        }
        SwapArraysForLists(rpglClass.AsDict());
        return rpglClass;
    }

    public static RPGLRace QueryRPGLRaceByDatapackId(string datapackId) {
        RPGLRace rpglRace;
        using (DBConnection connection = new(dbDir, dbName)) {
            rpglRace = connection.Collection<RPGLRaceTO>("races")
                .FindOne(x => x.DatapackId == datapackId)
                .ToRPGLRace();
        }
        SwapArraysForLists(rpglRace.AsDict());
        return rpglRace;
    }

    public static RPGLEffectTemplate QueryRPGLEffectTemplateByDatapackId(string datapackId) {
        RPGLEffectTemplate template;
        using (DBConnection connection = new(dbDir, dbName)) {
            template = connection.Collection<RPGLEffectTemplateTO>("effect_templates")
                .FindOne(x => x.DatapackId == datapackId)
                .ToTemplate();
        }
        SwapArraysForLists(template.AsDict());
        return template;
    }

    public static RPGLEventTemplate QueryRPGLEventTemplateByDatapackId(string datapackId) {
        RPGLEventTemplate template;
        using (DBConnection connection = new(dbDir, dbName)) {
            template = connection.Collection<RPGLEventTemplateTO>("event_templates")
                .FindOne(x => x.DatapackId == datapackId)
                .ToTemplate();
        }
        SwapArraysForLists(template.AsDict());
        return template;
    }

    public static RPGLItemTemplate QueryRPGLItemTemplateByDatapackId(string datapackId) {
        RPGLItemTemplate template;
        using (DBConnection connection = new(dbDir, dbName)) {
            template = connection.Collection<RPGLItemTemplateTO>("item_templates")
                .FindOne(x => x.DatapackId == datapackId)
                .ToTemplate();
        }
        SwapArraysForLists(template.AsDict());
        return template;
    }

    public static RPGLObjectTemplate QueryRPGLObjectTemplateByDatapackId(string datapackId) {
        RPGLObjectTemplate template;
        using (DBConnection connection = new(dbDir, dbName)) {
            template = connection.Collection<RPGLObjectTemplateTO>("object_templates")
                .FindOne(x => x.DatapackId == datapackId)
                .ToTemplate();
        }
        SwapArraysForLists(template.AsDict());
        return template;
    }

    public static RPGLResourceTemplate QueryRPGLResourceTemplateByDatapackId(string datapackId) {
        RPGLResourceTemplate template;
        using (DBConnection connection = new(dbDir, dbName)) {
            template = connection.Collection<RPGLResourceTemplateTO>("resource_templates")
                .FindOne(x => x.DatapackId == datapackId)
                .ToTemplate();
        }
        SwapArraysForLists(template.AsDict());
        return template;
    }

    public static RPGLEffect QueryRPGLEffectByUuid(long uuid) {
        RPGLEffect rpglEffect;
        using (DBConnection connection = new(dbDir, dbName)) {
            rpglEffect = connection.Collection<RPGLEffectTO>("effects")
                .FindOne(x => x.Uuid == uuid)
                .ToRPGLEffect();
        }
        SwapArraysForLists(rpglEffect.AsDict());
        return rpglEffect;
    }

    public static RPGLItem QueryRPGLItemByUuid(long uuid) {
        RPGLItem rpglItem;
        using (DBConnection connection = new(dbDir, dbName)) {
            rpglItem = connection.Collection<RPGLItemTO>("items")
                .FindOne(x => x.Uuid == uuid)
                .ToRPGLItem();
        }
        SwapArraysForLists(rpglItem.AsDict());
        return rpglItem;
    }

    public static RPGLObject QueryRPGLObjectByUuid(long uuid) {
        RPGLObject rpglObject;
        using (DBConnection connection = new(dbDir, dbName)) {
            rpglObject = connection.Collection<RPGLObjectTO>("objects")
                .FindOne(x => x.Uuid == uuid)
                .ToRPGLObject();
        }
        SwapArraysForLists(rpglObject.AsDict());
        return rpglObject;
    }

    public static RPGLResource QueryRPGLResourceByUuid(long uuid) {
        RPGLResource rpglResource;
        using (DBConnection connection = new(dbDir, dbName)) {
            rpglResource  = connection.Collection<RPGLResourceTO>("resources")
                .FindOne(x => x.Uuid == uuid)
                .ToRPGLResource();
        }
        SwapArraysForLists(rpglResource.AsDict());
        return rpglResource;
    }

    public static bool IsUuidAvailable<T>(string collectionName, long uuid) where T : PersistentContentTO {
        bool isUuidAvailable = true;
        using (DBConnection connection = new(dbDir, dbName)) {
            if (connection.Collection<T>(collectionName).FindOne(x => x.Uuid == uuid) is not null) {
                isUuidAvailable = false;
            }
        }
        return isUuidAvailable;
    }

    public static int DeleteCollection<T>(string collectionName) {
        int numDeleted;
        using (DBConnection connection = new(dbDir, dbName)) {
            numDeleted = connection.Collection<T>(collectionName).DeleteAll();
        }
        return numDeleted;
    }

};

public class DBConnection : IDisposable {
    private readonly LiteDatabase db;

    public DBConnection(string dbDir, string dbName) {
        if (!Directory.Exists(dbDir)) {
            Directory.CreateDirectory(dbDir);
        }
        string dbPath = Path.Combine(dbDir, dbName);
        this.db = new LiteDatabase(dbPath);
    }

    public void Dispose() {
        GC.SuppressFinalize(this);
        this.db.Dispose();
    }

    public ILiteCollection<T> Collection<T>(string name) {
        return this.db.GetCollection<T>(name);
    }

};
