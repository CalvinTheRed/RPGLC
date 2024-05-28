using com.rpglc.json;

namespace com.rpglc.core;

public class RPGLObject : TaggableContent {

    public JsonObject GetAbilityScores() {
        return base.GetJsonObject("ability_scores");
    }

    public RPGLObject SetAbilityScores(JsonObject abilityScores) {
        base.PutJsonObject("ability_scores", abilityScores);
        return this;
    }

    public JsonObject GetEquippedItems() {
        return base.GetJsonObject("equipped_items");
    }

    public RPGLObject SetEquippedItems(JsonObject equippedItems) {
        base.PutJsonObject("equipped_items", equippedItems);
        return this;
    }

    public JsonArray GetClasses() {
        return base.GetJsonArray("classes");
    }

    public RPGLObject SetClasses(JsonArray classes) {
        base.PutJsonArray("classes", classes);
        return this;
    }

    public JsonArray GetEffects() {
        return base.GetJsonArray("effects");
    }

    public RPGLObject SetEffects(JsonArray effects) {
        base.PutJsonArray("effects", effects);
        return this;
    }

    public JsonArray GetEvents() {
        return base.GetJsonArray("events");
    }

    public RPGLObject SetEvents(JsonArray events) {
        base.PutJsonArray("events", events);
        return this;
    }

    public JsonArray GetInventory() {
        return base.GetJsonArray("inventory");
    }

    public RPGLObject SetInventory(JsonArray inventory) {
        base.PutJsonArray("inventory", inventory);
        return this;
    }

    public JsonArray GetPosition() {
        return base.GetJsonArray("position");
    }

    public RPGLObject SetPosition(JsonArray position) {
        base.PutJsonArray("position", position);
        return this;
    }

    public JsonArray GetRaces() {
        return base.GetJsonArray("races");
    }

    public RPGLObject SetRaces(JsonArray races) {
        base.PutJsonArray("races", races);
        return this;
    }

    public JsonArray GetResources() {
        return base.GetJsonArray("resources");
    }

    public RPGLObject SetResources(JsonArray resources) {
        base.PutJsonArray("resources", resources);
        return this;
    }

    public JsonArray GetRotation() {
        return base.GetJsonArray("rotation");
    }

    public RPGLObject SetRotation(JsonArray rotation) {
        base.PutJsonArray("rotation", rotation);
        return this;
    }

    public string GetOriginObject() {
        return base.GetString("origin_object");
    }

    public RPGLObject SetOriginObject(string originObject) {
        base.PutString("origin_object", originObject);
        return this;
    }

    public string GetProxyObject() {
        return base.GetString("proxy_object");
    }

    public RPGLObject SetProxyObject(string proxyObject) {
        base.PutString("proxy_object", proxyObject);
        return this;
    }

    public string GetUserId() {
        return base.GetString("user_id");
    }

    public RPGLObject SetUserId(string userId) {
        base.PutString("user_id", userId);
        return this;
    }

    public long GetHealthBase() {
        return base.GetInt("health_base");
    }

    public RPGLObject SetHealthBase(long healthBase) {
        base.PutInt("health_base", healthBase);
        return this;
    }

    public long GetHealthCurrent() {
        return base.GetInt("health_current");
    }

    public RPGLObject SetHealthCurrent(long healthCurrent) {
        base.PutInt("health_current", healthCurrent);
        return this;
    }

    // TODO temporary health may benefit from having a more involved data structure to track where it came from...

    public long GetHealthTemporary() {
        return base.GetInt("health_temporary");
    }

    public RPGLObject SetHealthTemporary(long healthTemporary) {
        base.PutInt("health_temporary", healthTemporary);
        return this;
    }

    public long GetProficiencyBonus() {
        return base.GetInt("proficiency_bonus");
    }

    public RPGLObject SetProficiencyBonus(long proficiencyBonus) {
        base.PutInt("proficiency_bonus", proficiencyBonus);
        return this;
    }

};
