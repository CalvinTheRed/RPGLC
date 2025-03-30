using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace RPGLC.com.rpglc.subevent;

public class AbilitySave : Subevent {

    public AbilitySave() : base("ability_save") { }

    public override Subevent Clone() {
        Subevent clone = new AttackRoll();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new AbilitySave();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override AbilitySave? Invoke(RPGLContext context, JsonArray originPoint) {
        return (AbilitySave?) base.Invoke(context, originPoint);
    }

    public override AbilitySave JoinSubeventData(JsonObject other) {
        return (AbilitySave) base.JoinSubeventData(other);
    }

    public override AbilitySave Prepare(RPGLContext context, JsonArray originPoint) {
        json.PutIfAbsent("determined", new JsonArray());
        json.PutIfAbsent("use_origin_difficulty_class_ability", false);
        CalculateDifficultyClass(context);
        return this;
    }

    public override AbilitySave Run(RPGLContext context, JsonArray originPoint) {
        AbilityCheck abilityCheck = new AbilityCheck()
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "ability": "{{json.GetString("ability")}}",
                    "skill": "{{GetSkill()}}",
                    "tags": {{GetTags()}},
                    "determined": {{json.GetJsonArray("determined")}}
                }
                """))
            .SetSource(GetTarget())
            .Prepare(context, originPoint)
            .SetTarget(GetSource())
            .Invoke(context, originPoint);

        if (abilityCheck.Get() < GetDifficultyClass()) {
            ResolveNestedSubevents("fail", context, originPoint);
        } else {
            ResolveNestedSubevents("pass", context, originPoint);
        }
        return this;
    }

    public override AbilitySave SetOriginItem(string? originItem) {
        return (AbilitySave) base.SetOriginItem(originItem);
    }

    public override AbilitySave SetSource(RPGLObject source) {
        return (AbilitySave) base.SetSource(source);
    }

    public override AbilitySave SetTarget(RPGLObject target) {
        return (AbilitySave) base.SetTarget(target);
    }

    private void CalculateDifficultyClass(RPGLContext context) {
        CalculateDifficultyClass calculateDifficultyClass = new();
        long? difficultyClass = GetDifficultyClass();
        if (difficultyClass.HasValue) {
            calculateDifficultyClass.JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "difficulty_class": {{difficultyClass}},
                    "tags": {{GetTags()}}
                }
                """));
        } else {
            calculateDifficultyClass.JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "difficulty_class_ability": "{{json.GetString("difficulty_class_ability")}}",
                    "tags": {{GetTags()}}
                }
                """));
        }

        calculateDifficultyClass.SetOriginItem(GetOriginItem())
            .SetSource((bool) json.GetBool("use_origin_difficulty_class_ability")
                ? RPGL.GetRPGLObject(GetSource().GetOriginObject())
                : GetSource()
            )
            .Prepare(context, GetSource().GetPosition())
            .SetTarget(GetSource())
            .Invoke(context, GetSource().GetPosition());

        json.PutLong("difficulty_class", calculateDifficultyClass.Get());
    }

    private void ResolveNestedSubevents(string resolution, RPGLContext context, JsonArray originPoint) {
        JsonArray? subeventJsonArray = json.GetJsonArray(resolution);
        if (!subeventJsonArray.IsEmpty()) {
            for (int i = 0; i < subeventJsonArray.Count(); i++) {
                JsonObject subeventJson = subeventJsonArray.GetJsonObject(i);
                Subevent subevent = Subevent.Subevents[subeventJson.GetString("subevent")]
                    .Clone(subeventJson)
                    .SetSource(GetSource())
                    .Prepare(context, originPoint)
                    .SetTarget(GetTarget())
                    .Invoke(context, originPoint);
            }
        }
    }

    public long? GetDifficultyClass() {
        return json.GetLong("difficulty_class");
    }

    private string? GetSkill() {
        return json.GetString("skill");
    }
}
