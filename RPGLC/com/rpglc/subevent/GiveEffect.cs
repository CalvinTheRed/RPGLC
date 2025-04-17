using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.subevent;

/// <summary>
///   Gives an effect to an object.
///   
///   <code>
///   {
///     "subevent": "give_effect",
///     "tags": [
///       &lt;string&gt;
///     ],
///     "effect": &lt;string&gt;
///   }
///   </code>
/// </summary>
public class GiveEffect : Subevent {
    
    public GiveEffect() : base("give_effect") { }

    public override Subevent Clone() {
        Subevent clone = new GiveEffect();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new GiveEffect();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override GiveEffect? Invoke(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return (GiveEffect?) base.Invoke(context, originPoint, invokingEffect);
    }

    public override GiveEffect JoinSubeventData(JsonObject other) {
        return (GiveEffect) base.JoinSubeventData(other);
    }

    public override GiveEffect Prepare(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return this;
    }

    public override GiveEffect Run(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        GetTarget().AddEffect(RPGLFactory.NewEffect(json.GetString("effect"))
            .SetOriginItem(GetOriginItem())
            .SetSource(GetSource().GetUuid()));
        return this;
    }

    public override GiveEffect SetOriginItem(string? originItem) {
        return (GiveEffect) base.SetOriginItem(originItem);
    }

    public override GiveEffect SetSource(RPGLObject source) {
        return (GiveEffect) base.SetSource(source);
    }

    public override GiveEffect SetTarget(RPGLObject target) {
        return (GiveEffect) base.SetTarget(target);
    }
}
