using LiteDB;

using com.rpglc.json;
using com.rpglc.data.TO;
using com.rpglc.core;

namespace com.rpglc.data;

public class DBManager {
    private static string? dbDir;
    private static string? dbName;

    public static void SetDatabase(string dbDir, string dbName) {
        DBManager.dbDir = dbDir;
        DBManager.dbName = dbName;
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

    // =====================================================================
    // Insertions
    // =====================================================================

    public static void InsertRPGLEffectTemplate(JsonObject data) {
        using DBConnection connection = new(dbDir, dbName);
        connection.Collection<RPGLEffectTemplateTO>("effect_templates").Insert(new RPGLEffectTemplateTO() {
            DatapackId = data.GetString("datapack_id"),
            Description = data.GetString("description"),
            Metadata = data.GetJsonObject("metadata").AsDict(),
            Name = data.GetString("name"),

            Tags = data.GetJsonArray("tags").AsList(),

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

            Effects = data.GetJsonObject("effects").AsDict(),
            Events = data.GetJsonObject("events").AsDict(),
            Resources = data.GetJsonObject("resources").AsDict(),
            Cost = (long) data.GetLong("cost"),
            Weight = (long) data.GetLong("weight"),
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
            HealthTemporary = data.GetJsonObject("health_temporary").AsDict(),
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
            ProficiencyBonus = data.GetLong("proficiency_bonus"),
            HealthBase = (long) data.GetLong("health_base"),
            HealthCurrent = (long) data.GetLong("health_current"),
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

            // available uses is not included in template data
            RefreshCriterion = data.GetJsonArray("refresh_criterion").AsList(),
            // origin item is not included in template data
            MaximumUses = data.GetLong("maximum_uses"),
            Potency = (long) data.GetLong("potency"),
        });
    }

    public static void InsertRPGLEffect(RPGLEffect rpglEffect) {
        using DBConnection connection = new(dbDir, dbName);
        connection.Collection<RPGLEffectTO>("effects").Insert(new RPGLEffectTO() {
            DatapackId = rpglEffect.GetDatapackId(),
            Description = rpglEffect.GetDescription(),
            Metadata = rpglEffect.GetMetadata().AsDict(),
            Name = rpglEffect.GetName(),

            Tags = rpglEffect.GetTags().AsList(),

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

            Effects = rpglItem.GetEffects().AsDict(),
            Events = rpglItem.GetEvents().AsDict(),
            Resources = rpglItem.GetResources().AsDict(),
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
            HealthTemporary = rpglObject.GetHealthTemporary().AsDict(),
            Classes = rpglObject.GetClasses().AsList(),
            Events = rpglObject.GetEvents().AsList(),
            Inventory = rpglObject.GetInventory().AsList(),
            Position = rpglObject.GetPosition().AsList(),
            Races = rpglObject.GetRaces().AsList(),
            Resources = rpglObject.GetResources().AsList(),
            Rotation = rpglObject.GetRotation().AsList(),
            OriginObject = rpglObject.GetOriginObject(),
            UserId = rpglObject.GetUserId(),
            HealthBase = rpglObject.GetHealthBase(),
            HealthCurrent = rpglObject.GetHealthCurrent(),
            ProficiencyBonus = rpglObject.GetProficiencyBonus(),
            Proxy = rpglObject.GetProxy(),
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
            AvailableUses = rpglResource.GetAvailableUses(),
            MaximumUses = rpglResource.GetMaximumUses(),
            Potency = rpglResource.GetPotency(),
        });
    }

    public static void InsertRPGLClass(JsonObject data) {
        using DBConnection connection = new(dbDir, dbName);
        connection.Collection<RPGLClassTO>("classes").Insert(new RPGLClassTO() {
            DatapackId = data.GetString("datapack_id"),
            Description = data.GetString("description"),
            Metadata = data.GetJsonObject("metadata").AsDict(),
            Name = data.GetString("name"),

            NestedClasses = data.GetJsonObject("nested_classes")?.AsDict(),
            StartingFeatures = data.GetJsonObject("starting_features")?.AsDict(),
            Features = data.GetJsonObject("features").AsDict(),
            AbilityScoreIncreaseLevels = data.GetJsonArray("ability_score_increase_levels")?.AsList(),
            MulticlassRequirements = data.GetJsonArray("multiclass_requirements")?.AsList(),
            SubclassLevel = data.GetLong("subclass_level"),
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

    // =====================================================================
    // Queries
    // =====================================================================

    public static List<RPGLEffectTemplate> QueryRPGLEffectTemplates() {
        List<RPGLEffectTemplate> templates = [];
        using (DBConnection connection = new(dbDir, dbName)) {
            foreach (var data in connection.Collection<RPGLEffectTemplateTO>("effect_templates").FindAll()) {
                templates.Add(data.ToTemplate());
            }
        }
        return templates;
    }

    public static List<RPGLEventTemplate> QueryRPGLEventTemplates() {
        List<RPGLEventTemplate> templates = [];
        using (DBConnection connection = new(dbDir, dbName)) {
            foreach (var data in connection.Collection<RPGLEventTemplateTO>("event_templates").FindAll()) {
                templates.Add(data.ToTemplate());
            }
        }
        return templates;
    }

    public static List<RPGLItemTemplate> QueryRPGLItemTemplates() {
        List<RPGLItemTemplate> templates = [];
        using (DBConnection connection = new(dbDir, dbName)) {
            foreach (var data in connection.Collection<RPGLItemTemplateTO>("item_templates").FindAll()) {
                templates.Add(data.ToTemplate());
            }
        }
        return templates;
    }

    public static List<RPGLObjectTemplate> QueryRPGLObjectTemplates() {
        List<RPGLObjectTemplate> templates = [];
        using (DBConnection connection = new(dbDir, dbName)) {
            foreach (var data in connection.Collection<RPGLObjectTemplateTO>("object_templates").FindAll()) {
                templates.Add(data.ToTemplate());
            }
        }
        return templates;
    }

    public static List<RPGLResourceTemplate> QueryRPGLResourceTemplates() {
        List<RPGLResourceTemplate> templates = [];
        using (DBConnection connection = new(dbDir, dbName)) {
            foreach (var data in connection.Collection<RPGLResourceTemplateTO>("resource_templates").FindAll()) {
                templates.Add(data.ToTemplate());
            }
        }
        return templates;
    }

    public static List<RPGLEffect> QueryRPGLEffects() {
        List<RPGLEffect> rpglEffects = [];
        using (DBConnection connection = new(dbDir, dbName)) {
            foreach (var data in connection.Collection<RPGLEffectTO>("effects").FindAll()) {
                rpglEffects.Add(data.ToRPGLEffect());
            }
        }
        return rpglEffects;
    }

    public static List<RPGLItem> QueryRPGLItems() {
        List<RPGLItem> rpglItems = [];
        using (DBConnection connection = new(dbDir, dbName)) {
            foreach (var data in connection.Collection<RPGLItemTO>("items").FindAll()) {
                rpglItems.Add(data.ToRPGLItem());
            }
        }
        return rpglItems;
    }

    public static List<RPGLObject> QueryRPGLObjects() {
        List<RPGLObject> rpglObjects = [];
        using (DBConnection connection = new(dbDir, dbName)) {
            foreach (var data in connection.Collection<RPGLObjectTO>("objects").FindAll()) {
                rpglObjects.Add(data.ToRPGLObject());
            }
        }
        return rpglObjects;
    }

    public static List<RPGLResource> QueryRPGLResources() {
        List<RPGLResource> rpglResources = [];
        using (DBConnection connection = new(dbDir, dbName)) {
            foreach (var data in connection.Collection<RPGLResourceTO>("resources").FindAll()) {
                rpglResources.Add(data.ToRPGLResource());
            }
        }
        return rpglResources;
    }

    public static List<RPGLClass> QueryRPGLClasses() {
        List<RPGLClass> rpglClasses = [];
        using (DBConnection connection = new(dbDir, dbName)) {
            foreach (var data in connection.Collection<RPGLClassTO>("classes").FindAll()) {
                rpglClasses.Add(data.ToRPGLClass());
            }
        }
        return rpglClasses;
    }

    public static List<RPGLRace> QueryRPGLRaces() {
        List<RPGLRace> rpglRaces = [];
        using (DBConnection connection = new(dbDir, dbName)) {
            foreach (var data in connection.Collection<RPGLRaceTO>("races").FindAll()) {
                rpglRaces.Add(data.ToRPGLRace());
            }
        }
        return rpglRaces;
    }

    // =====================================================================
    // Utility methods.
    // =====================================================================

    public static bool IsUuidAvailable<T>(string collectionName, string uuid) where T : PersistentContentTO {
        bool isUuidAvailable = true;
        using (DBConnection connection = new(dbDir, dbName)) {
            if (connection.Collection<T>(collectionName).FindOne(x => x.Uuid == uuid) is not null) {
                isUuidAvailable = false;
            }
        }
        return isUuidAvailable;
    }

    public static void DumpDatabase() {
        DeleteCollection<RPGLEffectTemplateTO>("effect_templates");
        DeleteCollection<RPGLEventTemplateTO>("event_templates");
        DeleteCollection<RPGLItemTemplateTO>("item_templates");
        DeleteCollection<RPGLObjectTemplateTO>("object_templates");
        DeleteCollection<RPGLResourceTemplateTO>("resource_templates");
        DeleteCollection<RPGLEffectTO>("effects");
        DeleteCollection<RPGLItemTO>("items");
        DeleteCollection<RPGLObjectTO>("objects");
        DeleteCollection<RPGLResourceTO>("resources");
        DeleteCollection<RPGLClassTO>("classes");
        DeleteCollection<RPGLRaceTO>("races");
    }

    private static int DeleteCollection<T>(string collectionName) {
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
