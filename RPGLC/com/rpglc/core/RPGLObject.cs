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

    public long? GetOriginObject() {
        return GetInt("origin_object");
    }

    public RPGLObject SetOriginObject(long? originObject) {
        PutInt("origin_object", originObject);
        return this;
    }

    public long? GetProxyObject() {
        return GetInt("proxy_object");
    }

    public RPGLObject SetProxyObject(long? proxyObject) {
        PutInt("proxy_object", proxyObject);
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
    // 
    // =====================================================================

    public RPGLObject GiveItem(long uuid) {
        if (!GetInventory().Contains(uuid)) {
            GetInventory().AddInt(uuid);
        }
        return this;
    }

    public RPGLObject TakeItem(long uuid) {
        GetInventory().AsList().Remove(uuid);
        return this;
    }

};
