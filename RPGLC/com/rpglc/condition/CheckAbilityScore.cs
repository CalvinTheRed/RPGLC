using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.condition;

/// <summary>
///   Returns true if the specified object's ability score satisfies a specified condition.
///   
///   <code>
///   {
///     "condition": "check_ability_score",
///     "object": {
///       "from": "subevent" | "effect",
///       "object": "source" | "target",
///       "as_origin": &lt;bool = false&gt;
///     },
///     "ability": &lt;string&gt;,
///     "comparison": "&lt;" | "&lt;=" | "&gt;" | "&gt;=" | "=" | "!=",
///     "compare_to": &lt;long&gt;
///   }
///   </code>
///   
///   <list type="bullet">
///     <item>"object" indicates which object's ability score is being checked.</item>
///     <item>"ability" indicates which ability score is being checked.</item>
///     <item>"comparison" the comparative operator to use in the order of `ability comparison compare_to`.</item>
///     <item>"compare_to" the value the ability score is compared to.</item>
///   </list>
///   
/// </summary>
public class CheckAbilityScore : Condition {

    public CheckAbilityScore() : base("check_ability_score") { }

    public override bool Run(RPGLEffect rpglEffect, Subevent subevent, JsonObject conditionJson, RPGLContext context, JsonArray originPoint) {
        RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, subevent, conditionJson.GetJsonObject("object"));
        return CompareValues(
            rpglObject.GetAbilityScoreFromAbilityName(conditionJson.GetString("ability"), context),
            conditionJson.GetString("comparison"),
            (long) conditionJson.GetLong("compare_to")
        );
    }

};
