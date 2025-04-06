using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.subevent;

/// <summary>
///   Performs an ability check and compares it to a difficulty class. Different results may occur on a pass or fail.
///   
///   <code>
///   {
///     "subevent": "ability_save",
///     "tags": [
///       &lt;string&gt;
///     ],
///     "ability": &lt;string&gt;,
///     "skill": &lt;string = null&gt;,
///     "use_origin_difficulty_class_ability": &lt;bool = false&gt;,
///     "difficulty_class_ability": &lt;string = null&gt;,
///     "difficulty_class": &lt;long = null&gt;,
///     "pass": [
///       &lt;nested subevent&gt;
///     ],
///     "fail": [
///       &lt;nested subevent&gt;
///     ]
///   }
///   </code>
///   
///   <i>Note that "difficulty_class_ability" and "difficulty_class" are mutually exclusive, but that one of them is required for the subevent to work as intended.</i>
///   
///   <br /><br />
///   <list type="bullet">
///     <item>"tags" is an optional field and will default to a value of [ ] if left unspecified. Any tags provided will be inherited by any nested subevents.</item>
///     <item>"ability" indicates what ability is used to make the ability check.</item>
///     <item>"skill" is an optional field and will default to a value of null if left unspecified. This field indicates what skill, if any, is used to make the ability check. This typically goes to inform how proficiency bonuses will be applied to the subevent.</item>
///     <item>"use_origin_difficulty_class_ability" is an optional field and it will default to a value of false if left unspecified. If true, the ability score used for this subevent will be taken from the source's origin object, instead of from the source.</item>
///     <item>"difficulty_class_ability" is an optional field and it will default to a value of null if left unspecified. This field indicates what ability, if any, should be used to calculate the save's difficulty class. If null, the difficulty class will not be calculated in this way.</item>
///     <item>"difficulty_class" is an optional field and it will default to a value of null if left unspecified. This field indicates what value, if any, to use as the difficulty class. If null, the difficulty class will not be assigned in this way.</item>
///     <item>"pass" is an optional field and it will default to a vlaue of [ ] if left unspecified. This field contains a list of subevents that will be invoked if the target passes the save.</item>
///     <item>"fail" is an optional field and it will default to a value of [ ] if left unspecified. This field contains a list of subevents that will be invoked if the target fails the save.</item>
///   </list>
///   
/// </summary>
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
        long? difficultyClass = GetDifficultyClass();

        CalculateDifficultyClass calculateDifficultyClass = new CalculateDifficultyClass()
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    {{(difficultyClass.HasValue
                    ? $"\"difficulty_class\": {difficultyClass}"
                    : $"\"difficulty_class_ability\": \"{json.GetString("difficulty_class_ability")}\"")}},
                    "tags": {{GetTags()}}
                }
            """))
            .SetOriginItem(GetOriginItem())
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

    private long? GetDifficultyClass() {
        return json.GetLong("difficulty_class");
    }

    private string? GetSkill() {
        return json.GetString("skill");
    }
}
