using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.condition;

/// <summary>
///   Returns true if the specified object's level in a specified class satisfies a specified condition.
///   
///   <code>
///   {
///     "condition": "check_level",
///     "object": {
///       "from": "subevent" | "effect",
///       "object": "source" | "target"
///     },
///     "class": "*" | &lt;class datapack id&gt;,
///     "comparison": "&lt;" | "&lt;=" | "&gt;" | "&gt;=" | "=" | "!=",
///     "compare_to": &lt;long&gt;
///   }
///   </code>
///   
///   <list type="bullet">
///     <item>"object" indicates which object's level is being checked.</item>
///     <item>"class" indicates which class's level is being checked. If "*", the object's total level is checked, rather than its level in a particular class.</item>
///     <item>"comparison" the comparative operator to use in the order of `ability comparison compare_to`.</item>
///     <item>"compare_to" the value the level is compared to.</item>
///   </list>
///   
/// </summary>
public class CheckLevel : Condition {

    public CheckLevel() : base("check_level") { }

    public override bool Run(RPGLEffect rpglEffect, Subevent subevent, JsonObject conditionJson, RPGLContext context, JsonArray originPoint) {
        RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, subevent, conditionJson.GetJsonObject("object"));
        string classDatapackId = conditionJson.GetString("class");
        long level = classDatapackId == "*"
            ? rpglObject.GetLevel()
            : rpglObject.GetLevel(classDatapackId);
        
        return CompareValues(
            level,
            conditionJson.GetString("comparison"),
            (long) conditionJson.GetLong("compare_to")
        );
    }

};
