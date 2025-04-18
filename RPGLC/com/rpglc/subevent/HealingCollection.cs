﻿using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.subevent;

/// <summary>
///   Collects healing for a healing subevent.
///   
///   <br /><br />
///   <i>This subevent is unavailable to be used directly inside an RPGLEvent.</i>
///   
///   <br /><br />
///   <i>Note that all healing subevents will create two HealingCollection subevents. The first will have the "base" tag, and will represent healing that is applied to all targets of the healing subevent. The second will have the "target" tag, and will represent healing that is only applied to a specific target of the healing subevent.</i>
///   
///   <br /><br />
///   <b>Special Functions</b>
///   <list type="bullet">
///     <item>AddHealing</item>
///   </list>
///   
/// </summary>
public class HealingCollection : Subevent {

    public HealingCollection() : base("healing_collection") { }

    public override Subevent Clone() {
        Subevent clone = new HealingCollection();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new HealingCollection();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override HealingCollection? Invoke(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return (HealingCollection?) base.Invoke(context, originPoint, invokingEffect);
    }

    public override HealingCollection JoinSubeventData(JsonObject other) {
        return (HealingCollection) base.JoinSubeventData(other);
    }

    public override HealingCollection Prepare(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        json.PutIfAbsent("healing", new JsonArray());
        PrepareHealing(context);
        return this;
    }

    public override HealingCollection Run(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return this;
    }

    public override HealingCollection SetOriginItem(string? originItem) {
        return (HealingCollection) base.SetOriginItem(originItem);
    }

    public override HealingCollection SetSource(RPGLObject source) {
        return (HealingCollection) base.SetSource(source);
    }

    public override HealingCollection SetTarget(RPGLObject target) {
        return (HealingCollection) base.SetTarget(target);
    }

    private void PrepareHealing(RPGLContext context) {
        JsonArray healingArray = json.RemoveJsonArray("healing");
        json.PutJsonArray("healing", new());

        RPGLEffect rpglEffect = new();
        rpglEffect.SetSource(GetSource().GetUuid());
        rpglEffect.SetTarget(null);
        for (int i = 0; i < healingArray.Count(); i++) {
            AddHealing(CalculationSubevent.SimplifyCalculationFormula(
                rpglEffect,
                this,
                healingArray.GetJsonObject(i),
                context
            ));
        }
    }

    public HealingCollection AddHealing(JsonObject healingJson) {
        GetHealingCollection().AddJsonObject(healingJson);
        return this;
    }

    public JsonArray GetHealingCollection() {
        return json.GetJsonArray("healing");
    }

};
