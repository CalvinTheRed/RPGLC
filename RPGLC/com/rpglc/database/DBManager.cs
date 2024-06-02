using LiteDB;

using com.rpglc.json;
using com.rpglc.database.TO;
using com.rpglc.core;
using System.Linq.Expressions;

namespace com.rpglc.database;

public class DBManager {
    private static string? dbDir;
    private static string? dbName;
    private static DBConnection? connection;

    public static string? GetDatabase() {
        if (IsDatabaseConnected()) {
            return Path.Combine(dbDir, dbName);
        }
        return null;
    }

    public static void ConnectToDatabase(string dbDir, string dbName) {
        DBManager.dbDir = dbDir;
        DBManager.dbName = dbName;

        DisconnectFromDatabase();
        connection = new(dbDir, dbName);
    }

    public static void DisconnectFromDatabase() {
        connection?.Dispose();
        connection = null;
    }

    public static bool IsDatabaseConnected() {
        return connection is not null;
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
        connection?.Collection<RPGLClassTO>("classes").Insert(new RPGLClassTO() {
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
        connection?.Collection<RPGLRaceTO>("races").Insert(new RPGLRaceTO() {
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
        connection?.Collection<RPGLEffectTemplateTO>("effect_templates").Insert(new RPGLEffectTemplateTO() {
            DatapackId = data.GetString("datapack_id"),
            Description = data.GetString("description"),
            Metadata = data.GetJsonObject("metadata").AsDict(),
            Name = data.GetString("name"),

            SubeventFilters = data.GetJsonObject("subevent_filters").AsDict(),
        });
    }

    public static void InsertRPGLEventTemplate(JsonObject data) {
        connection?.Collection<RPGLEventTemplateTO>("event_templates").Insert(new RPGLEventTemplateTO() {
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
        connection?.Collection<RPGLItemTemplateTO>("item_templates").Insert(new RPGLItemTemplateTO() {
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
        connection?.Collection<RPGLObjectTemplateTO>("object_templates").Insert(new RPGLObjectTemplateTO() {
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
        connection?.Collection<RPGLResourceTemplateTO>("resource_templates").Insert(new RPGLResourceTemplateTO() {
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

    // =====================================================================
    // Template instance insertions
    // =====================================================================

    public static void InsertRPGLEffect(RPGLEffect rpglEffect) {
        connection?.Collection<RPGLEffectTO>("effects").Insert(new RPGLEffectTO() {
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
        connection?.Collection<RPGLItemTO>("items").Insert(new RPGLItemTO() {
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
        connection?.Collection<RPGLObjectTO>("objects").Insert(new RPGLObjectTO() {
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
        connection?.Collection<RPGLResourceTO>("resources").Insert(new RPGLResourceTO() {
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

    // =====================================================================
    // Class/Race queries
    // =====================================================================

    public static RPGLClass? QueryRPGLClassByDatapackId(string datapackId) {
        if (connection is not null) {
            RPGLClass rpglClass = connection.Collection<RPGLClassTO>("classes")
                .FindOne(x => x.DatapackId == datapackId)
                .ToRPGLClass();
            SwapArraysForLists(rpglClass.AsDict());
            return rpglClass;
        }
        return null;
    }

    public static RPGLRace? QueryRPGLRaceByDatapackId(string datapackId) {
        if (connection is not null) {
            RPGLRace rpglRace = connection.Collection<RPGLRaceTO>("races")
                .FindOne(x => x.DatapackId == datapackId)
                .ToRPGLRace();
            SwapArraysForLists(rpglRace.AsDict());
            return rpglRace;
        }
        return null;
    }

    // =====================================================================
    // Template queries
    // =====================================================================

    public static RPGLEffectTemplate? QueryRPGLEffectTemplateByDatapackId(string datapackId) {
        if (connection is not null) {
            RPGLEffectTemplate template = connection.Collection<RPGLEffectTemplateTO>("effect_templates")
                .FindOne(x => x.DatapackId == datapackId)
                .ToTemplate();
            SwapArraysForLists(template.AsDict());
            return template;
        }
        return null;
    }

    public static RPGLEventTemplate? QueryRPGLEventTemplateByDatapackId(string datapackId) {
        if (connection is not null) {
            RPGLEventTemplate template = connection.Collection<RPGLEventTemplateTO>("event_templates")
                .FindOne(x => x.DatapackId == datapackId)
                .ToTemplate();
            SwapArraysForLists(template.AsDict());
            return template;
        }
        return null;
    }

    public static RPGLItemTemplate? QueryRPGLItemTemplateByDatapackId(string datapackId) {
        if (connection is not null) {
            RPGLItemTemplate template = connection.Collection<RPGLItemTemplateTO>("item_templates")
                .FindOne(x => x.DatapackId == datapackId)
                .ToTemplate();
            SwapArraysForLists(template.AsDict());
            return template;
        }
        return null;
    }

    public static RPGLObjectTemplate? QueryRPGLObjectTemplateByDatapackId(string datapackId) {
        if (connection is not null) {
            RPGLObjectTemplate template = connection.Collection<RPGLObjectTemplateTO>("object_templates")
                .FindOne(x => x.DatapackId == datapackId)
                .ToTemplate();
            SwapArraysForLists(template.AsDict());
            return template;
        }
        return null;
    }

    public static RPGLResourceTemplate? QueryRPGLResourceTemplateByDatapackId(string datapackId) {
        if (connection is not null) {
            RPGLResourceTemplate template = connection.Collection<RPGLResourceTemplateTO>("resource_templates")
                .FindOne(x => x.DatapackId == datapackId)
                .ToTemplate();
            SwapArraysForLists(template.AsDict());
            return template;
        }
        return null;
    }

    // =====================================================================
    // Template instance queries
    // =====================================================================

    public static RPGLEffect? QueryRPGLEffect(Expression<Func<RPGLEffectTO, bool>> predicate) {
        if (connection is not null) {
            RPGLEffect? rpglEffect = connection.Collection<RPGLEffectTO>("effects")
                .FindOne(predicate)
                .ToRPGLEffect();
            if (rpglEffect is not null) {
                SwapArraysForLists(rpglEffect.AsDict());
                return rpglEffect;
            }
        }
        return null;
    }

    public static List<RPGLEffect> QueryRPGLEffects(Expression<Func<RPGLEffectTO, bool>> predicate) {
        List<RPGLEffect> effects = [];
        connection?.Collection<RPGLEffectTO>("effects")
            .Query()
            .Where(predicate)
            .ToList()
            .ForEach(x => {
                RPGLEffect rpglEffect = x.ToRPGLEffect();
                SwapArraysForLists(rpglEffect.AsDict());
                effects.Add(rpglEffect);
            });
        return effects;
    }

    public static RPGLItem? QueryRPGLItem(Expression<Func<RPGLItemTO, bool>> predicate) {
        if (connection is not null) {
            RPGLItem? rpglItem = connection.Collection<RPGLItemTO>("items")
                .FindOne(predicate)
                .ToRPGLItem();
            if (rpglItem is not null) {
                SwapArraysForLists(rpglItem.AsDict());
                return rpglItem;
            }
        }
        return null;
    }

    public static List<RPGLItem> QueryRPGLItems(Expression<Func<RPGLItemTO, bool>> predicate) {
        List<RPGLItem> items = [];
        connection?.Collection<RPGLItemTO>("items")
            .Query()
            .Where(predicate)
            .ToList()
            .ForEach(x => {
                RPGLItem rpglItem = x.ToRPGLItem();
                SwapArraysForLists(rpglItem.AsDict());
                items.Add(rpglItem);
            });
        return items;
    }

    public static RPGLObject? QueryRPGLObject(Expression<Func<RPGLObjectTO, bool>> predicate) {
        if (connection is not null) {
            RPGLObject? rpglObject = connection.Collection<RPGLObjectTO>("objects")
                .FindOne(predicate)
                .ToRPGLObject();
            if (rpglObject is not null) {
                SwapArraysForLists(rpglObject.AsDict());
                return rpglObject;
            }
        }
        return null;
    }

    public static List<RPGLObject> QueryRPGLObjects(Expression<Func<RPGLObjectTO, bool>> predicate) {
        List<RPGLObject> objects = [];
        connection?.Collection<RPGLObjectTO>("objects")
            .Query()
            .Where(predicate)
            .ToList()
            .ForEach(x => {
                RPGLObject rpglObject = x.ToRPGLObject();
                SwapArraysForLists(rpglObject.AsDict());
                objects.Add(rpglObject);
            });
        return objects;
    }

    public static RPGLResource? QueryRPGLResource(Expression<Func<RPGLResourceTO, bool>> predicate) {
        if (connection is not null) {
            RPGLResource? rpglResource = connection.Collection<RPGLResourceTO>("resources")
                .FindOne(predicate)
                ?.ToRPGLResource();
            if (rpglResource is not null) {
                SwapArraysForLists(rpglResource.AsDict());
                return rpglResource;
            }
        }
        return null;
    }

    public static List<RPGLResource> QueryRPGLResources(Expression<Func<RPGLResourceTO, bool>> predicate) {
        List<RPGLResource> resources = [];
        connection?.Collection<RPGLResourceTO>("resources")
            .Query()
            .Where(predicate)
            .ToList()
            .ForEach(x => {
                RPGLResource rpglResource = x.ToRPGLResource();
                SwapArraysForLists(rpglResource.AsDict());
                resources.Add(rpglResource);
            });
        return resources;
    }

    // =====================================================================
    // Template instance deletions
    // =====================================================================

    public static void DeleteRPGLEffect(RPGLEffect rpglEffect) {
        connection?.Collection<RPGLEffectTO>("effects").Delete(rpglEffect.GetId());
    }

    public static void DeleteRPGLEffectByUuid(string uuid) {
        if (connection is not null) {
            ILiteCollection<RPGLEffectTO> collection = connection.Collection<RPGLEffectTO>("effects");
            RPGLEffectTO effectTO = collection.FindOne(x => x.Uuid == uuid);
            collection.Delete(effectTO._id);
        }
    }

    public static void DeleteRPGLItem(RPGLItem rpglItem) {
        if (connection is not null) {
            connection.Collection<RPGLItemTO>("items").Delete(rpglItem.GetId());

            // delete contained effects
            JsonArray effectsBySlot = rpglItem.GetEffects();
            for (int i = 0; i < effectsBySlot.Count(); i++) {
                JsonObject effectsForSlot = effectsBySlot.GetJsonObject(i);
                JsonArray effectUuidList = effectsForSlot.GetJsonArray("resources");
                for (int j = 0; j < effectUuidList.Count(); j++) {
                    DeleteRPGLEffectByUuid(effectUuidList.GetString(j));
                }
            }

            // delete contained resources
            JsonArray resourcesBySlot = rpglItem.GetResources();
            for (int i = 0; i < resourcesBySlot.Count(); i++) {
                JsonObject resourcesForSlot = resourcesBySlot.GetJsonObject(i);
                JsonArray resourceUuidList = resourcesForSlot.GetJsonArray("resources");
                for (int j = 0; j < resourceUuidList.Count(); j++) {
                    DeleteRPGLResourceByUuid(resourceUuidList.GetString(j));
                }
            }
        }
    }

    public static void DeleteRPGLItemByUuid(string uuid) {
        if (connection is not null) {
            RPGLItem rpglItem= connection.Collection<RPGLItemTO>("items")
                .FindOne(x => x.Uuid == uuid)
                .ToRPGLItem();
            DeleteRPGLItem(rpglItem);
        }
    }

    public static void DeleteRPGLObject(RPGLObject rpglObject) {
        if (connection is not null) {
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
    }

    public static void DeleteRPGLObjectByUuid(string uuid) {
        if (connection is not null) {
            RPGLObject rpglObject = connection.Collection<RPGLObjectTO>("objects")
                .FindOne(x => x.Uuid == uuid)
                .ToRPGLObject();
            DeleteRPGLObject(rpglObject);
        }
    }

    public static void DeleteRPGLResource(RPGLResource rpglResource) {
        connection?.Collection<RPGLResourceTO>("resources").Delete(rpglResource.GetId());
    }

    public static void DeleteRPGLResourceByUuid(string uuid) {
        if (connection is not null) {
            ILiteCollection<RPGLResourceTO> collection = connection.Collection<RPGLResourceTO>("resources");
            RPGLResourceTO resourceTO = collection.FindOne(x => x.Uuid == uuid);
            collection.Delete(resourceTO._id);
        }
    }

    // =====================================================================
    // Template instance updates
    // =====================================================================

    public static void UpdateRPGLEffect(RPGLEffect rpglEffect) {
        connection?.Collection<RPGLEffectTO>("effects").Update(new RPGLEffectTO() {
            _id = rpglEffect.GetId(),

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

    public static void UpdateRPGLItem(RPGLItem rpglItem) {
        connection?.Collection<RPGLItemTO>("items").Update(new RPGLItemTO() {
            _id = rpglItem.GetId(),

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

    public static void UpdateRPGLObject(RPGLObject rpglObject) {
        connection?.Collection<RPGLObjectTO>("objects").Update(new RPGLObjectTO() {
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
            ProxyObject = rpglObject.GetProxyObject(),
            UserId = rpglObject.GetUserId(),
            HealthBase = rpglObject.GetHealthBase(),
            HealthCurrent = rpglObject.GetHealthCurrent(),
            HealthTemporary = rpglObject.GetHealthTemporary(),
            ProficiencyBonus = rpglObject.GetProficiencyBonus(),
        });
    }

    public static void UpdateRPGLResource(RPGLResource rpglResource) {
        connection?.Collection<RPGLResourceTO>("resources").Update(new RPGLResourceTO() {
            _id = rpglResource.GetId(),

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

    // =====================================================================
    // Utility operations
    // =====================================================================

    public static bool? IsUuidAvailable<T>(string collectionName, string uuid) where T : PersistentContentTO {
        if (connection is not null) {
            if (connection.Collection<T>(collectionName).FindOne(x => x.Uuid == uuid) is not null) {
                return true;
            }
            return false;
        }
        return null;
    }

    public static void DeleteCollection<T>(string collectionName) {
        connection?.Collection<T>(collectionName).DeleteAll();
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
