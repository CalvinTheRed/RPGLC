using com.rpglc.database;
using com.rpglc.database.TO;
using com.rpglc.json;

namespace com.rpglc.core;

public class RPGLObject : TaggableContent {

    public JsonObject GetAbilityScores() {
        return GetJsonObject("ability_scores");
    }

    public RPGLObject SetAbilityScores(JsonObject abilityScores) {
        PutJsonObject("ability_scores", abilityScores);
        return this;
    }

    public JsonObject GetEquippedItems() {
        return GetJsonObject("equipped_items");
    }

    public RPGLObject SetEquippedItems(JsonObject equippedItems) {
        PutJsonObject("equipped_items", equippedItems);
        return this;
    }

    public JsonArray GetClasses() {
        return GetJsonArray("classes");
    }

    public RPGLObject SetClasses(JsonArray classes) {
        PutJsonArray("classes", classes);
        return this;
    }

    public JsonArray GetEvents() {
        return GetJsonArray("events");
    }

    public RPGLObject SetEvents(JsonArray events) {
        PutJsonArray("events", events);
        return this;
    }

    public JsonArray GetInventory() {
        return GetJsonArray("inventory");
    }

    public RPGLObject SetInventory(JsonArray inventory) {
        PutJsonArray("inventory", inventory);
        return this;
    }

    public JsonArray GetPosition() {
        return GetJsonArray("position");
    }

    public RPGLObject SetPosition(JsonArray position) {
        PutJsonArray("position", position);
        return this;
    }

    public JsonArray GetRaces() {
        return GetJsonArray("races");
    }

    public RPGLObject SetRaces(JsonArray races) {
        PutJsonArray("races", races);
        return this;
    }

    public JsonArray GetResources() {
        return GetJsonArray("resources");
    }

    public RPGLObject SetResources(JsonArray resources) {
        PutJsonArray("resources", resources);
        return this;
    }

    public JsonArray GetRotation() {
        return GetJsonArray("rotation");
    }

    public RPGLObject SetRotation(JsonArray rotation) {
        PutJsonArray("rotation", rotation);
        return this;
    }

    public string? GetOriginObject() {
        return GetString("origin_object");
    }

    public RPGLObject SetOriginObject(string? originObject) {
        PutString("origin_object", originObject);
        return this;
    }

    public string? GetProxyObject() {
        return GetString("proxy_object");
    }

    public RPGLObject SetProxyObject(string? proxyObject) {
        PutString("proxy_object", proxyObject);
        return this;
    }

    public string GetUserId() {
        return GetString("user_id");
    }

    public RPGLObject SetUserId(string userId) {
        PutString("user_id", userId);
        return this;
    }

    public long GetHealthBase() {
        return (long) GetInt("health_base");
    }

    public RPGLObject SetHealthBase(long healthBase) {
        PutInt("health_base", healthBase);
        return this;
    }

    public long GetHealthCurrent() {
        return (long) GetInt("health_current");
    }

    public RPGLObject SetHealthCurrent(long healthCurrent) {
        PutInt("health_current", healthCurrent);
        return this;
    }

    // TODO temporary health may benefit from having a more involved data structure to track where it came from...

    public long GetHealthTemporary() {
        return (long) GetInt("health_temporary");
    }

    public RPGLObject SetHealthTemporary(long healthTemporary) {
        PutInt("health_temporary", healthTemporary);
        return this;
    }

    public long GetProficiencyBonus() {
        return (long) GetInt("proficiency_bonus");
    }

    public RPGLObject SetProficiencyBonus(long proficiencyBonus) {
        PutInt("proficiency_bonus", proficiencyBonus);
        return this;
    }

    // =====================================================================
    // Inventory management helper methods.
    // =====================================================================

    public RPGLObject GiveItem(string uuid) {
        if (!GetInventory().Contains(uuid)) {
            GetInventory().AddString(uuid);
            DBManager.UpdateRPGLObject(this);
        }
        return this;
    }

    public RPGLObject TakeItem(string uuid) {
        JsonArray inventory = GetInventory();
        if (inventory.Contains(uuid)) {
            inventory.AsList().Remove(uuid);
            JsonObject equippedItems = GetEquippedItems();
            foreach (string equipmentSlot in equippedItems.AsDict().Keys) {
                if (equippedItems.GetString(equipmentSlot) == uuid) {
                    equippedItems.RemoveString(equipmentSlot);
                }
            }
            DBManager.UpdateRPGLObject(this);
        }
        return this;
    }

    public RPGLObject EquipItem(string itemUuid, string equipmentSlot) {
        if (GetInventory().Contains(itemUuid)) {
            GetEquippedItems().PutString(equipmentSlot, itemUuid);
            DBManager.UpdateRPGLObject(this);
        }
        return this;
    }

    public RPGLObject UnequipItem(string equipmentSlot) {
        JsonObject equippedItems = GetEquippedItems();
        if (equippedItems.AsDict().Keys.Contains(equipmentSlot)) {
            equippedItems.RemoveString(equipmentSlot);
            DBManager.UpdateRPGLObject(this);
        }
        return this;
    }

    // =====================================================================
    // Class/Level management helper methods.
    // =====================================================================

    public long GetLevel(string classDatapackId) {
        JsonArray classList = GetClasses();
        for (int i = 0; i < classList.Count(); i++) {
            JsonObject classData = classList.GetJsonObject(i);
            if (classData.GetString("id") == classDatapackId) {
                return (long) classData.GetInt("level");
            }
        }
        return 0L;
    }

    public long GetLevel() {
        JsonArray classes = GetClasses();
        List<string> classDatapackIds = [];
        List<string> nestedClassDatapackIds = [];
        for (int i = 0; i < classes.Count(); i++) {
            JsonObject classData = classes.GetJsonObject(i);
            string classDatapackId = classData.GetString("id");
            classDatapackIds.Add(classDatapackId);
            RPGLClass rpglClass = DBManager.QueryRPGLClassByDatapackId(classDatapackId);
            JsonObject nestedClasses = rpglClass.GetNestedClasses();
            var keyCollection = nestedClasses.AsDict().Keys;
            foreach (string key in keyCollection) {
                nestedClassDatapackIds.Add(key);
            }
            foreach (string key in classData.GetJsonObject("additional_nested_classes").AsDict().Keys) {
                nestedClassDatapackIds.Add(key);
            }
        }
        classDatapackIds.RemoveAll(nestedClassDatapackIds.Contains);
        long level = 0L;
        foreach (string classDatapackId in classDatapackIds) {
            level += GetLevel(classDatapackId);
        }
        return level;
    }

    private long CalculateLevelForNestedClass(string nestedClassDatapackId) {
        long nestedClassLevel = 0;
        JsonArray classList = GetClasses();
        for (int i = 0; i < classList.Count(); i++) {
            JsonObject classData = classList.GetJsonObject(i);
            RPGLClass rpglClass = DBManager.QueryRPGLClassByDatapackId(classData.GetString("id"));
            JsonObject nestedClassList = rpglClass.GetNestedClasses();
            JsonObject additionalNestedClasses = classData.GetJsonObject("additional_nested_classes");
            JsonObject? nestedClassData = null;
            if (nestedClassList.AsDict().ContainsKey(nestedClassDatapackId)) {
                nestedClassData = nestedClassList.GetJsonObject(nestedClassDatapackId);
            } else if (additionalNestedClasses.AsDict().ContainsKey(nestedClassDatapackId)) {
                nestedClassData = additionalNestedClasses.GetJsonObject(nestedClassDatapackId);
            }
            if (nestedClassData is not null) {
                long classLevel = (long) classData.GetInt("level");
                long scale = (long) nestedClassData.GetInt("scale");
                bool roundUp = (bool) nestedClassData.GetBool("round_up");
                if (roundUp) {
                    nestedClassLevel += (long) Math.Ceiling(classLevel / (double) scale);
                } else {
                    nestedClassLevel += (long) (classLevel / (double) scale);
                }
            }
        }
        return nestedClassLevel;
    }

    private List<string> GetNestedClassIds(string classDatapackId) {
        RPGLClass rpglClass = DBManager.QueryRPGLClassByDatapackId(classDatapackId);
        JsonObject nestedClassList = rpglClass.GetNestedClasses();
        List<string> nestedClassDatapackIds = new(nestedClassList.AsDict().Keys);
        JsonArray classList = GetClasses();
        for (int i = 0; i < classList.Count(); i++) {
            JsonObject classData = classList.GetJsonObject(i);
            if (classData.GetString("id") == classDatapackId) {
                nestedClassDatapackIds.AddRange(classData.GetJsonObject("additional_nested_classes").AsDict().Keys);
                break;
            }
        }
        return nestedClassDatapackIds;
    }

    public void LevelUpNestedClasses(string classDatapackId, JsonObject choices) {
        foreach (string nestedClassDatapackId in GetNestedClassIds(classDatapackId)) {
            RPGLClass rpglClass = DBManager.QueryRPGLClassByDatapackId(nestedClassDatapackId);
            long intendedLevel = CalculateLevelForNestedClass(nestedClassDatapackId);
            long currentLevel = GetLevel(nestedClassDatapackId);
            while (currentLevel < intendedLevel) {
                rpglClass.LevelUpRPGLObject(this, choices);
                currentLevel = this.GetLevel(nestedClassDatapackId);
            }
        }
    }

    internal void LevelUpRaces(JsonObject choices, long level) {
        JsonArray races = GetRaces();
        for (int i = 0; i < races.Count(); i++) {
            string raceDatapackId = races.GetString(i);
            RPGLRace rpglRace = DBManager.QueryRPGLRaceByDatapackId(raceDatapackId);
            rpglRace.LevelUpRPGLObject(this, choices, level);
        }
    }

    public RPGLObject LevelUp(string classDatapackId, JsonObject choices, bool updateDatabase = true) {
        RPGLClass rpglClass = DBManager.QueryRPGLClassByDatapackId(classDatapackId);
        if (GetLevel() == 0) {
            rpglClass.GrantStartingFeatures(this, choices);
        }
        rpglClass.LevelUpRPGLObject(this, choices);
        LevelUpNestedClasses(classDatapackId, choices);
        LevelUpRaces(choices, GetLevel());
        if (updateDatabase) {
            DBManager.UpdateRPGLObject(this);
        }
        return this;
    }

    public void AddAdditionalNestedClass(
            string classDatapackId,
            string additionalNestedClassDatapackId,
            long scale,
            bool roundUp
    ) {
        JsonArray classes = GetClasses();
        for (int i = 0; i < classes.Count(); i++) {
            JsonObject classData = classes.GetJsonObject(i);
            if (classData.GetString("id") == classDatapackId) {
                classData.GetJsonObject("additional_nested_classes").PutJsonObject(
                    additionalNestedClassDatapackId,
                    new JsonObject()
                        .PutInt("scale", scale)
                        .PutBool("round_up", roundUp)
                );
            }
        }
    }

    // =====================================================================
    // Resource management helper methods.
    // =====================================================================

    public RPGLObject GiveResource(RPGLResource rpglResource) {
        if (!GetResources().Contains(rpglResource.GetUuid())) {
            GetResources().AddString(rpglResource.GetUuid());
            DBManager.UpdateRPGLObject(this);
        }
        return this;
    }

    public RPGLObject TakeResource(long resourceUuid) {
        if (GetResources().Contains(resourceUuid)) {
            GetResources().AsList().Remove(resourceUuid);
            DBManager.UpdateRPGLObject(this);
        }
        return this;
    }

};
