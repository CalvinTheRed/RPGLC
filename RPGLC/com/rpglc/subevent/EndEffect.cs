using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.subevent;

/// <summary>
///   Removes an effect from an object.
///   
///   <code>
///   {
///     "subevent": "end_effect",
///     "tags": [
///       &lt;string&gt;
///     ],
///     "effect": &lt;string&gt;,
///     "effect_tags": [
///       &lt;string&gt;
///     ],
///     "effect_source": {
///       "from": "subevent" | "effect",
///       "object": "source" | "target",
///       "as_origin": &lt;bool = false&gt;
///     }
///   }
///   </code>
///   
///   <br /><br />
///   <b>Special Functions</b>
///   <list type="bullet">
///     <item>_</item>
///   </list>
///   
/// </summary>
public class EndEffect : Subevent {
    
    public EndEffect() : base("end_effect") { }

    public override Subevent Clone() {
        Subevent clone = new EndEffect();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new EndEffect();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override EndEffect? Invoke(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return (EndEffect?) base.Invoke(context, originPoint, invokingEffect);
    }

    public override EndEffect JoinSubeventData(JsonObject other) {
        return (EndEffect) base.JoinSubeventData(other);
    }

    public override EndEffect Prepare(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        json.PutIfAbsent("effect", "*");
        json.PutIfAbsent("effect_tags", new JsonArray());
        json.PutIfAbsent("effect_source", new JsonObject());
        return this;
    }

    public override EndEffect Run(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        string effectId = json.GetString("effect");
        List<object> effectTags = json.GetJsonArray("effect_tags").AsList();
        string effectSource = RPGLEffect.GetObject(invokingEffect, this, json.GetJsonObject("effect_source") ?? new())?.GetUuid() ?? "*";
        string effectTarget = RPGLEffect.GetObject(invokingEffect, this, json.GetJsonObject("effect_target") ?? new())?.GetUuid() ?? "*";

        RPGL.GetRPGLEffects().FindAll(e =>
            e.GetTags().ContainsAll(effectTags)
            && (effectId == "*" || effectId == e.GetDatapackId())
            && (effectSource == "*" || effectSource == e.GetSource())
            && (effectTarget == "*" || effectTarget == e.GetTarget())
        ).ForEach(e => RPGL.RemoveRPGLEffect(e));

        return this;
    }

    public override EndEffect SetOriginItem(string? originItem) {
        return (EndEffect) base.SetOriginItem(originItem);
    }

    public override EndEffect SetSource(RPGLObject source) {
        return (EndEffect) base.SetSource(source);
    }

    public override EndEffect SetTarget(RPGLObject target) {
        return (EndEffect) base.SetTarget(target);
    }
}
