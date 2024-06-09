using com.rpglc.database;
using com.rpglc.json;
using com.rpglc.subevent;
using System.Resources;

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

    public bool? GetProxy() {
        return GetBool("proxy");
    }

    public RPGLObject SetProxy(bool? proxy) {
        PutBool("proxy", proxy);
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

    private void LevelUpRaces(JsonObject choices, long level) {
        JsonArray races = GetRaces();
        for (int i = 0; i < races.Count(); i++) {
            string raceDatapackId = races.GetString(i);
            RPGLRace rpglRace = DBManager.QueryRPGLRaceByDatapackId(raceDatapackId);
            rpglRace.LevelUpRPGLObject(this, choices, level);
        }
    }

    public RPGLObject LevelUp(string classDatapackId, JsonObject choices, JsonObject additionalNestedClasses) {
        RPGLClass rpglClass = DBManager.QueryRPGLClassByDatapackId(classDatapackId);

        // level up
        if (GetLevel() == 0) {
            rpglClass.GrantStartingFeatures(this, choices);
        }
        rpglClass.LevelUpRPGLObject(this, choices);
        
        // update nested classes
        foreach (string key in additionalNestedClasses.AsDict().Keys) {
            JsonObject additionalNestedClassData = additionalNestedClasses.GetJsonObject(key);
            AddAdditionalNestedClass(
                classDatapackId,
                key,
                additionalNestedClassData.GetInt("scale") ?? 1L,
                additionalNestedClassData.GetBool("round_up") ?? false
            );
        }
        LevelUpNestedClasses(classDatapackId, choices);

        // update race features
        LevelUpRaces(choices, GetLevel());

        DBManager.UpdateRPGLObject(this);
        return this;
    }

    public RPGLObject LevelUp(string classDatapackId, JsonObject choices) {
        return LevelUp(classDatapackId, choices, new());
    }

    private void AddAdditionalNestedClass(
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

    public RPGLObject TakeResource(string resourceUuid) {
        if (GetResources().Contains(resourceUuid)) {
            GetResources().AsList().Remove(resourceUuid);
            DBManager.UpdateRPGLObject(this);

            RPGLResource rpglResource = DBManager.QueryRPGLResource(x => x.Uuid == resourceUuid);
            if (rpglResource.GetOriginItem() is null) {
                // delete resource if it is not supplied by an item
                DBManager.DeleteRPGLResource(rpglResource);
            }
            
        }
        return this;
    }

    public List<RPGLResource> GetResourceObjects() {
        List<RPGLResource> resources = [];
        JsonArray resourceUuids = GetResources();
        for (int i = 0; i < resourceUuids.Count(); i++) {
            resources.Add(DBManager.QueryRPGLResource(
                x => x.Uuid == resourceUuids.GetString(i))
            );
        }

        // add resources granted by items equipped appropriately
        JsonObject equippedItems = GetEquippedItems();
        Dictionary<string, List<string>> slotsForEquippedItems = [];
        foreach (string slot in equippedItems.AsDict().Keys) {
            string itemUuid = equippedItems.GetString(slot);
            if (slotsForEquippedItems.ContainsKey(itemUuid)) {
                // add slot to item's list
                slotsForEquippedItems[itemUuid].Add(slot);
            } else {
                // create new list for item
                slotsForEquippedItems[itemUuid] = [slot];
            }
        }
        foreach (string itemUuid in slotsForEquippedItems.Keys) {
            RPGLItem rpglItem = DBManager.QueryRPGLItem(x => x.Uuid == itemUuid);
            resources.AddRange(rpglItem.GetResourcesForSlots(slotsForEquippedItems[itemUuid]));
        }

        return resources;
    }

    // =====================================================================
    // RPGLEvent management helper methods.
    // =====================================================================

    public RPGLObject AddEvent(string eventDatapackId) {
        JsonArray events = GetEvents();
        if (!events.Contains(eventDatapackId)) {
            events.AddString(eventDatapackId);
            DBManager.UpdateRPGLObject(this);
        }
        return this;
    }

    public RPGLObject RemoveEvent(string eventDatapackId) {
        JsonArray events = GetEvents();
        if (events.Contains(eventDatapackId)) {
            events.AsList().Remove(eventDatapackId);
            DBManager.UpdateRPGLObject(this);
        }
        return this;
    }

    public void InvokeEvent(
        RPGLEvent rpglEvent,
        JsonArray originPoint,
        List<RPGLResource> resources,
        List<RPGLObject> targets,
        RPGLContext context
    ) {
        if (rpglEvent.ResourcesMatchCost(resources)) {
            foreach (RPGLResource rpglResource in resources) {
                rpglResource.Exhaust();
            }
            rpglEvent.Scale(resources);

            RPGLObject source;
            if (rpglEvent.GetString("source") is not null) {
                // events with a source pre-assigned via AddEvent take priority
                source = DBManager.QueryRPGLObject(x => x.Uuid == rpglEvent.GetString("source"));
            } else if (GetProxy() ?? false) {
                // proxy objects set their origin object as the source for any events they invoke
                source = DBManager.QueryRPGLObject(x => x.Uuid == GetOriginObject());
            } else {
                // ordinary event invocation sets the calling object as the source
                source = this;
            }

            JsonArray subeventJsonArray = rpglEvent.GetSubevents();
            for (int i = 0; i < subeventJsonArray.Count(); i++) {
                JsonObject subeventJson = subeventJsonArray.GetJsonObject(i);
                Subevent subevent = Subevent.Subevents[subeventJson.GetString("subevent")]
                    .Clone(subeventJson)
                    .SetSource(source)
                    .SetOriginItem(rpglEvent.GetOriginItem())
                    .Prepare(context, originPoint);
                foreach (RPGLObject target in targets) {
                    subevent.Clone().SetTarget(target).Invoke(context, originPoint);
                }
            }
        }
    }

    public bool ProcessSubevent(Subevent subevent, RPGLContext context, JsonArray originPoint) {
        bool wasSubeventProcessed = false;
        List<RPGLEffect> effects = DBManager.QueryRPGLEffects(x => x.Target == GetUuid());
        foreach (RPGLEffect rpglEffect in effects) {
            wasSubeventProcessed |= rpglEffect.ProcessSubevent(subevent, context, originPoint);
        }
        foreach (RPGLResource rpglResource in GetResourceObjects()) {
            rpglResource.ProcessSubevent(subevent, this);
        }
        return wasSubeventProcessed;
    }

    // =====================================================================
    // RPGLEffect management helper methods.
    // =====================================================================

    public RPGLObject AddEffect(RPGLEffect rpglEffect) {
        List<RPGLEffect> effects = DBManager.QueryRPGLEffects(
            x => x.Target == GetUuid()
        );
        bool hasEffect = false;
        foreach (RPGLEffect activeEffect in effects) {
            if (activeEffect.GetUuid() == rpglEffect.GetUuid()) {
                hasEffect = true;
                break;
            }
        }
        if (!hasEffect) {
            rpglEffect.SetTarget(GetUuid());
            DBManager.UpdateRPGLEffect(rpglEffect);
        }
        return this;
    }

    public RPGLObject RemoveEffect(string effectUuid) {
        RPGLEffect? rpglEffect = DBManager.QueryRPGLEffect(
            x => x.Uuid == effectUuid && x.Target == GetUuid()
        );
        if (rpglEffect is not null) {
            if (rpglEffect.GetOriginItem() is not null) {
                // effect coming from an item should persist
                rpglEffect.SetTarget(null);
                DBManager.UpdateRPGLEffect(rpglEffect);
            } else {
                // effects not coming from item should not persist
                DBManager.DeleteRPGLEffect(rpglEffect);
            }
        }
        return this;
    }

    public List<RPGLEffect> GetEffectObjects() {
        List<RPGLEffect> effects = DBManager.QueryRPGLEffects(x => x.Target == GetUuid());

        // add effects granted by items equipped appropriately
        JsonObject equippedItems = GetEquippedItems();
        Dictionary<string, List<string>> slotsForEquippedItems = [];
        foreach (string slot in equippedItems.AsDict().Keys) {
            string itemUuid = equippedItems.GetString(slot);
            if (slotsForEquippedItems.ContainsKey(itemUuid)) {
                // add slot to item's list
                slotsForEquippedItems[itemUuid].Add(slot);
            } else {
                // create new list for item
                slotsForEquippedItems[itemUuid] = [slot];
            }
        }
        foreach (string itemUuid in slotsForEquippedItems.Keys) {
            RPGLItem rpglItem = DBManager.QueryRPGLItem(x => x.Uuid == itemUuid);
            effects.AddRange(rpglItem.GetEffectsForSlots(slotsForEquippedItems[itemUuid]));
        }

        return effects;
    }

};
