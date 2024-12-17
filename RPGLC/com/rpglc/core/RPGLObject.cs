using com.rpglc.database;
using com.rpglc.json;
using com.rpglc.subevent;

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

    public JsonObject GetHealthTemporary() {
        return GetJsonObject("health_temporary");
    }

    public RPGLObject SetHealthTemporary(JsonObject healthTemporary) {
        PutJsonObject("health_temporary", healthTemporary);
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

    public long? GetProficiencyBonus() {
        return GetLong("proficiency_bonus");
    }

    public RPGLObject SetProficiencyBonus(long? proficiencyBonus) {
        PutLong("proficiency_bonus", proficiencyBonus);
        return this;
    }

    public long GetHealthBase() {
        return (long) GetLong("health_base");
    }

    public RPGLObject SetHealthBase(long healthBase) {
        PutLong("health_base", healthBase);
        return this;
    }

    public long GetHealthCurrent() {
        return (long) GetLong("health_current");
    }

    public RPGLObject SetHealthCurrent(long healthCurrent) {
        PutLong("health_current", healthCurrent);
        return this;
    }

    // =====================================================================
    // Inventory management helper methods.
    // =====================================================================

    public RPGLObject GiveItem(string uuid) {
        if (!GetInventory().Contains(uuid)) {
            GetInventory().AddString(uuid);
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
        }
        return this;
    }

    public RPGLObject EquipItem(string itemUuid, string equipmentSlot) {
        if (GetInventory().Contains(itemUuid)) {
            GetEquippedItems().PutString(equipmentSlot, itemUuid);
        }
        return this;
    }

    public RPGLObject UnequipItem(string equipmentSlot) {
        JsonObject equippedItems = GetEquippedItems();
        if (equippedItems.AsDict().ContainsKey(equipmentSlot)) {
            equippedItems.RemoveString(equipmentSlot);
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
                return (long) classData.GetLong("level");
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
                long classLevel = (long) classData.GetLong("level");
                long scale = (long) nestedClassData.GetLong("scale");
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
                additionalNestedClassData.GetLong("scale") ?? 1L,
                additionalNestedClassData.GetBool("round_up") ?? false
            );
        }
        LevelUpNestedClasses(classDatapackId, choices);

        // update race features
        LevelUpRaces(choices, GetLevel());

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
                        .PutLong("scale", scale)
                        .PutBool("round_up", roundUp)
                );
            }
        }
    }

    public long GetProficiencyBonusByLevel() {
        return (long) (1 + Math.Ceiling(GetLevel() / 4.0));
    }

    // =====================================================================
    // Resource management helper methods.
    // =====================================================================

    public RPGLObject GiveResource(RPGLResource rpglResource) {
        if (!GetResources().Contains(rpglResource.GetUuid())) {
            GetResources().AddString(rpglResource.GetUuid());
        }
        return this;
    }

    public RPGLObject TakeResource(string resourceUuid) {
        if (GetResources().Contains(resourceUuid)) {
            GetResources().AsList().Remove(resourceUuid);

            RPGLResource rpglResource = RPGL.GetRPGLResources().Find(x => x.GetUuid() == resourceUuid);
            if (rpglResource.GetOriginItem() is null) {
                // destroy resource if it is not supplied by an item
                RPGL.RemoveRPGLResource(rpglResource);
            }
            
        }
        return this;
    }

    public List<RPGLResource> GetResourceObjects() {
        List<RPGLResource> resources = [];
        JsonArray resourceUuids = GetResources();
        for (int i = 0; i < resourceUuids.Count(); i++) {
            resources.Add(RPGL.GetRPGLResources().Find(
                x => x.GetUuid() == resourceUuids.GetString(i))
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
            RPGLItem rpglItem = RPGL.GetRPGLItems().Find(x => x.GetUuid() == itemUuid);
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
        }
        return this;
    }

    public RPGLObject RemoveEvent(string eventDatapackId) {
        JsonArray events = GetEvents();
        if (events.Contains(eventDatapackId)) {
            events.AsList().Remove(eventDatapackId);
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
        if (rpglEvent.ResourcesSatisfyCost(resources)) {
            rpglEvent.Scale(resources);
            rpglEvent.SpendResources(resources);

            RPGLObject source;
            if (rpglEvent.GetString("source") is not null) {
                // events with a source pre-assigned via AddEvent take priority
                source = RPGL.GetRPGLObjects().Find(x => x.GetUuid() == rpglEvent.GetString("source"));
            } else if (GetProxy() ?? false) {
                // proxy objects set their origin object as the source for any events they invoke
                source = RPGL.GetRPGLObjects().Find(x => x.GetUuid() == GetOriginObject());
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
        List<RPGLEffect> effects = RPGL.GetRPGLEffects().FindAll(x => x.GetTarget() == GetUuid());
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
        List<RPGLEffect> effects = RPGL.GetRPGLEffects().FindAll(x => x.GetTarget() == GetUuid());
        bool hasEffect = false;
        foreach (RPGLEffect activeEffect in effects) {
            if (activeEffect.GetUuid() == rpglEffect.GetUuid()) {
                hasEffect = true;
                break;
            }
        }
        if (!hasEffect) {
            rpglEffect.SetTarget(GetUuid());
        }
        return this;
    }

    public RPGLObject RemoveEffect(string effectUuid) {
        RPGLEffect? rpglEffect = RPGL.GetRPGLEffects().Find(
            x => x.GetUuid() == effectUuid && x.GetTarget() == GetUuid()
        );
        if (rpglEffect is not null) {
            if (rpglEffect.GetOriginItem() is not null) {
                // effect coming from an item should persist
                rpglEffect.SetTarget(null);
            } else {
                // effects not coming from item should not persist
                RPGL.RemoveRPGLEffect(rpglEffect);
            }
        }
        return this;
    }

    public List<RPGLEffect> GetEffectObjects() {
        List<RPGLEffect> effects = RPGL.GetRPGLEffects().FindAll(x => x.GetTarget() == GetUuid());

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
            RPGLItem rpglItem = RPGL.GetRPGLItems().Find(x => x.GetUuid() == itemUuid);
            effects.AddRange(rpglItem.GetEffectsForSlots(slotsForEquippedItems[itemUuid]));
        }

        return effects;
    }

    public long GetEffectiveProficiencyBonus(RPGLContext context) {
        return new CalculateProficiencyBonus()
            .JoinSubeventData(new JsonObject()
                .PutJsonArray("tags", GetTags().DeepClone())
            )
            .SetSource(this)
            .Prepare(context, GetPosition())
            .SetTarget(this)
            .Invoke(context, GetPosition())
            .Get();
    }

    public long GetAbilityScoreFromAbilityName(string ability, RPGLContext context) {
        return new CalculateAbilityScore()
            .JoinSubeventData(new JsonObject()
                .PutJsonArray("tags", GetTags().DeepClone())
                .PutString("ability", ability)
            )
            .SetSource(this)
            .Prepare(context, GetPosition())
            .SetTarget(this)
            .Invoke(context, GetPosition())
            .Get();
    }

    public long GetAbilityModifierFromAbilityName(string ability, RPGLContext context) {
        return GetAbilityModifierFromAbilityScore(GetAbilityScoreFromAbilityName(ability, context));
    }

    public static long GetAbilityModifierFromAbilityScore(long score) {
        if (score < 10) {
            // integer division rounds toward zero, so abilityScore must be
            // adjusted to calculate the correct values for negative modifiers
            score--;
        }
        return (score - 10L) / 2L;
    }

    public long CalculateArmorClass(RPGLContext context, Subevent? subevent = null) {
        return new CalculateArmorClass()
            .JoinSubeventData(new JsonObject()
                .PutJsonArray("tags", subevent is null ? new() : subevent.GetTags())
            )
            .SetOriginItem(subevent is null ? null : subevent.GetOriginItem())
            .SetSource(subevent is null ? this : subevent.GetSource())
            .Prepare(context, GetPosition())
            .SetTarget(this)
            .Invoke(context, GetPosition())
            .Get();
    }

    // =====================================================================
    // Hit point management helper methods.
    // =====================================================================

    public long GetMaximumHitPoints(RPGLContext context) {
        return new CalculateMaximumHitPoints()
            .SetSource(this)
            .Prepare(context, GetPosition())
            .SetTarget(this)
            .Invoke(context, GetPosition())
            .Get();
    }

    public void ReceiveDamage(DamageDelivery damageDelivery, RPGLContext context) {
        JsonObject damageJson = damageDelivery.GetDamage();
        long damage = 0L;
        foreach (string key in damageJson.AsDict().Keys) {
            damage += (long) damageJson.GetLong(key);
        }
        ReduceHitPoints(damage, context);
    }

    private void ReduceHitPoints(long damage, RPGLContext context) {
        long temporaryHitPoints = GetTemporaryHitPoints();
        long currentHitPoints = GetHealthCurrent();
        if (damage > temporaryHitPoints) {
            if (temporaryHitPoints > 0) {
                damage -= temporaryHitPoints;
                temporaryHitPoints = 0L;
                currentHitPoints -= damage;
                SetTemporaryHitPoints(temporaryHitPoints);
                SetHealthCurrent(currentHitPoints);
                // TODO info_subevent for 0 THP
            } else {
                currentHitPoints -= damage;
                SetHealthCurrent(currentHitPoints);
            }
        } else {
            temporaryHitPoints -= damage;
            SetTemporaryHitPoints(temporaryHitPoints);
        }
        // TODO info_subevents for 0 hp or instant death
    }

    public void ReceiveHealing(HealingDelivery healingDelivery, RPGLContext context) {
        long health = GetHealthCurrent();
        health += healingDelivery.GetHealing();
        long maximumHitPoints = GetMaximumHitPoints(context);
        if (health > maximumHitPoints) {
            health = maximumHitPoints;
        }
        SetHealthCurrent(health);
    }

    public void ReceiveTemporaryHitPoints(TemporaryHitPointDelivery temporaryHitPointDelivery, JsonArray riderEffects) {
        long temporaryHitPoints = GetTemporaryHitPoints();
        long newTemporaryHitPoints = temporaryHitPointDelivery.GetTemporaryHitPoints();

        if (newTemporaryHitPoints >= temporaryHitPoints) {
            // TODO update temporary hit point count
            SetTemporaryHitPoints(newTemporaryHitPoints);
            // TODO remove any old rider effects
            // TODO add any new rider effects
        }
    }

    public long GetTemporaryHitPoints() {
        return (long) GetHealthTemporary().GetLong("count");
    }

    public RPGLObject SetTemporaryHitPoints(long temporaryHitPoints) {
        GetHealthTemporary().PutLong("count", temporaryHitPoints);
        return this;
    }

};
