using LiteDB;

using com.rpglc.json;
using com.rpglc.database.TO;
using com.rpglc.core;
using System.Linq.Expressions;

namespace com.rpglc.database;

public class DBManager {
    private static string? dbDir;
    private static string? dbName;

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

    // =====================================================================
    // Class/Race insertions
    // =====================================================================

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

    // =====================================================================
    // Template insertions
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
            MaximumUses = data.GetInt("maximum_uses"),
            Potency = (long) data.GetInt("potency"),
        });
    }

    // =====================================================================
    // Template instance insertions
    // =====================================================================

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
            HealthTemporary = rpglObject.GetHealthTemporary(),
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

    // =====================================================================
    // Class/Race queries
    // =====================================================================

    public static RPGLClass? QueryRPGLClassByDatapackId(string datapackId) {
        RPGLClass? rpglClass = null;
        using (DBConnection connection = new(dbDir, dbName)) {
            rpglClass = connection.Collection<RPGLClassTO>("classes")
                .FindOne(x => x.DatapackId == datapackId)
                .ToRPGLClass();
        }
        if (rpglClass is not null) {
            SwapArraysForLists(rpglClass.AsDict());
            return rpglClass;
        }
        return null;
    }

    public static RPGLRace? QueryRPGLRaceByDatapackId(string datapackId) {
        RPGLRace? rpglRace;
        using (DBConnection connection = new(dbDir, dbName)) {
            rpglRace = connection.Collection<RPGLRaceTO>("races")
                .FindOne(x => x.DatapackId == datapackId)
                .ToRPGLRace();
        }
        if (rpglRace is not null) {
            SwapArraysForLists(rpglRace.AsDict());
            return rpglRace;
        }
        return null;
    }

    // =====================================================================
    // Template queries
    // =====================================================================

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

    // =====================================================================
    // Template instance queries
    // =====================================================================

    public static RPGLEffect? QueryRPGLEffect(Expression<Func<RPGLEffectTO, bool>> predicate) {
        RPGLEffect? rpglEffect;
        using (DBConnection connection = new(dbDir, dbName)) {
            rpglEffect = connection.Collection<RPGLEffectTO>("effects")
                .FindOne(predicate)
                .ToRPGLEffect();
        }
        if (rpglEffect is null) {
            return null;
        } else {
            SwapArraysForLists(rpglEffect.AsDict());
            return rpglEffect;
        }
    }

    public static List<RPGLEffect> QueryRPGLEffects(Expression<Func<RPGLEffectTO, bool>> predicate) {
        List<RPGLEffect> effects = [];
        using (DBConnection connection = new(dbDir, dbName)) {
            connection.Collection<RPGLEffectTO>("effects")
                .Query()
                .Where(predicate)
                .ToList()
                .ForEach(x => {
                    RPGLEffect rpglEffect = x.ToRPGLEffect();
                    SwapArraysForLists(rpglEffect.AsDict());
                    effects.Add(rpglEffect);
                });
        }
        return effects;
    }

    public static RPGLItem? QueryRPGLItem(Expression<Func<RPGLItemTO, bool>> predicate) {
        RPGLItem? rpglItem;
        using (DBConnection connection = new(dbDir, dbName)) {
            rpglItem = connection.Collection<RPGLItemTO>("items")
                .FindOne(predicate)
                .ToRPGLItem();
        }
        if (rpglItem is null) {
            return null;
        } else {
            SwapArraysForLists(rpglItem.AsDict());
            return rpglItem;
        }
    }

    public static List<RPGLItem> QueryRPGLItems(Expression<Func<RPGLItemTO, bool>> predicate) {
        List<RPGLItem> items = [];
        using (DBConnection connection = new(dbDir, dbName)) {
            connection.Collection<RPGLItemTO>("items")
                .Query()
                .Where(predicate)
                .ToList()
                .ForEach(x => {
                    RPGLItem rpglItem = x.ToRPGLItem();
                    SwapArraysForLists(rpglItem.AsDict());
                    items.Add(rpglItem);
                });
        }
        return items;
    }

    public static RPGLObject? QueryRPGLObject(Expression<Func<RPGLObjectTO, bool>> predicate) {
        RPGLObject? rpglObject;
        using (DBConnection connection = new(dbDir, dbName)) {
            rpglObject = connection.Collection<RPGLObjectTO>("objects")
                .FindOne(predicate)
                .ToRPGLObject();
        }
        if (rpglObject is null) {
            return null;
        } else {
            SwapArraysForLists(rpglObject.AsDict());
            return rpglObject;
        }
    }

    public static List<RPGLObject> QueryRPGLObjects(Expression<Func<RPGLObjectTO, bool>> predicate) {
        List<RPGLObject> objects = [];
        using (DBConnection connection = new(dbDir, dbName)) {
            connection.Collection<RPGLObjectTO>("objects")
                .Query()
                .Where(predicate)
                .ToList()
                .ForEach(x => {
                    RPGLObject rpglObject = x.ToRPGLObject();
                    SwapArraysForLists(rpglObject.AsDict());
                    objects.Add(rpglObject);
                });
        }
        return objects;
    }

    public static RPGLResource? QueryRPGLResource(Expression<Func<RPGLResourceTO, bool>> predicate) {
        RPGLResource? rpglResource;
        using (DBConnection connection = new(dbDir, dbName)) {
            rpglResource = connection.Collection<RPGLResourceTO>("resources")
                .FindOne(predicate)
                ?.ToRPGLResource();
        }
        if (rpglResource is null) {
            return null;
        } else {
            SwapArraysForLists(rpglResource.AsDict());
            return rpglResource;
        }
    }

    public static List<RPGLResource> QueryRPGLResources(Expression<Func<RPGLResourceTO, bool>> predicate) {
        List<RPGLResource> resources = [];
        using (DBConnection connection = new(dbDir, dbName)) {
            connection.Collection<RPGLResourceTO>("resources")
                .Query()
                .Where(predicate)
                .ToList()
                .ForEach(x => {
                    RPGLResource rpglResource = x.ToRPGLResource();
                    SwapArraysForLists(rpglResource.AsDict());
                    resources.Add(rpglResource);
                });
        }
        return resources;
    }

    // =====================================================================
    // Template instance deletions
    // =====================================================================

    public static void DeleteRPGLEffect(RPGLEffect rpglEffect) {
        using DBConnection connection = new(dbDir, dbName);
        connection.Collection<RPGLEffectTO>("effects").Delete(rpglEffect.GetId());
    }

    public static void DeleteRPGLEffectByUuid(string uuid) {
        using DBConnection connection = new(dbDir, dbName);
        ILiteCollection<RPGLEffectTO> collection = connection.Collection<RPGLEffectTO>("effects");
        RPGLEffectTO effectTO = collection.FindOne(x => x.Uuid == uuid);
        collection.Delete(effectTO._id);
    }

    public static void DeleteRPGLItem(RPGLItem rpglItem) {
        using DBConnection connection = new(dbDir, dbName);
        connection.Collection<RPGLItemTO>("items").Delete(rpglItem.GetId());

        // delete contained effects
        JsonObject effects = rpglItem.GetEffects();
        foreach (string uuid in effects.AsDict().Keys) {
            DeleteRPGLEffectByUuid(uuid);
        }

        // delete contained resources
        JsonObject resources = rpglItem.GetResources();
        foreach (string uuid in resources.AsDict().Keys) {
            DeleteRPGLResourceByUuid(uuid);
        }
    }

    public static void DeleteRPGLItemByUuid(string uuid) {
        RPGLItem rpglItem;
        using (DBConnection connection = new(dbDir, dbName)) {
            rpglItem = connection.Collection<RPGLItemTO>("items")
                .FindOne(x => x.Uuid == uuid)
                .ToRPGLItem();
        }
        DeleteRPGLItem(rpglItem);
    }

    public static void DeleteRPGLObject(RPGLObject rpglObject) {
        using DBConnection connection = new(dbDir, dbName);
        connection.Collection<RPGLObjectTO>("objects").Delete(rpglObject.GetId());

        // TODO drop items in the world as loot?
        // delete contained items (must occur before deleting effects or resources)
        JsonArray itemUuidList = rpglObject.GetInventory();
        for (int i = 0; i < itemUuidList.Count(); i++) {
            DeleteRPGLItemByUuid(itemUuidList.GetString(i));
        }

        // delete effects targeting object
        List<RPGLEffect> effects = QueryRPGLEffects(x => x.Target == rpglObject.GetUuid());
        foreach (RPGLEffect rpglEffect in effects) {
            DeleteRPGLEffect(rpglEffect);
        }

        // delete contained resources
        JsonArray resourceUuidList = rpglObject.GetResources();
        for (int i = 0; i < resourceUuidList.Count(); i++) {
            DeleteRPGLResourceByUuid(resourceUuidList.GetString(i));
        }
    }

    public static void DeleteRPGLObjectByUuid(string uuid) {
        RPGLObject rpglObject;
        using (DBConnection connection = new(dbDir, dbName)) {
            rpglObject = connection.Collection<RPGLObjectTO>("objects")
                .FindOne(x => x.Uuid == uuid)
                .ToRPGLObject();
        }
        DeleteRPGLObject(rpglObject);
    }

    public static void DeleteRPGLResource(RPGLResource rpglResource) {
        using DBConnection connection = new(dbDir, dbName);
        connection.Collection<RPGLResourceTO>("resources").Delete(rpglResource.GetId());
    }

    public static void DeleteRPGLResourceByUuid(string uuid) {
        using DBConnection connection = new(dbDir, dbName);
        ILiteCollection<RPGLResourceTO> collection = connection.Collection<RPGLResourceTO>("resources");
        RPGLResourceTO resourceTO = collection.FindOne(x => x.Uuid == uuid);
        collection.Delete(resourceTO._id);
    }

    // =====================================================================
    // Template instance updates
    // =====================================================================

    public static void UpdateRPGLEffect(RPGLEffect rpglEffect) {
        using DBConnection connection = new(dbDir, dbName);
        connection.Collection<RPGLEffectTO>("effects").Update(new RPGLEffectTO() {
            _id = rpglEffect.GetId(),

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

    public static void UpdateRPGLItem(RPGLItem rpglItem) {
        using DBConnection connection = new(dbDir, dbName);
        connection.Collection<RPGLItemTO>("items").Update(new RPGLItemTO() {
            _id = rpglItem.GetId(),

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

    public static void UpdateRPGLObject(RPGLObject rpglObject) {
        using DBConnection connection = new(dbDir, dbName);
        connection.Collection<RPGLObjectTO>("objects").Update(new RPGLObjectTO() {
            _id = rpglObject.GetId(),

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
            UserId = rpglObject.GetUserId(),
            HealthBase = rpglObject.GetHealthBase(),
            HealthCurrent = rpglObject.GetHealthCurrent(),
            HealthTemporary = rpglObject.GetHealthTemporary(),
            ProficiencyBonus = rpglObject.GetProficiencyBonus(),
            Proxy = rpglObject.GetProxy(),
        });
    }

    public static void UpdateRPGLResource(RPGLResource rpglResource) {
        using DBConnection connection = new(dbDir, dbName);
        connection.Collection<RPGLResourceTO>("resources").Update(new RPGLResourceTO() {
            _id = rpglResource.GetId(),

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

    // =====================================================================
    // Utility operations
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
