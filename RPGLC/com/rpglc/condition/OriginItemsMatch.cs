using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.condition;

/// <summary>
///   Returns true if the subevent and effect have a common origin item.
///   
///   <code>
///   {
///     "condition": "origin_items_match"
///   }
///   </code>
///   
/// </summary>
public class OriginItemsMatch : Condition {

    public OriginItemsMatch() : base("origin_items_match") {
        conditionSteps.AddRange([
            (rpglEffect, subevent, conditionJson, context) => {
                string effectOriginItem = rpglEffect.GetOriginItem();
                string subeventOriginItem = subevent.GetOriginItem();
                this.evaluation = effectOriginItem is not null && Equals(effectOriginItem, subeventOriginItem);

                return new() {
                    conditionDependency = null,
                    subeventDependency = null,
                    stepCompleted = true,
                };
            },
        ]);
    }

    public override OriginItemsMatch Clone() {
        return new();
    }

    public override bool Run(RPGLEffect rpglEffect, Subevent subevent, JsonObject conditionJson, RPGLContext context, JsonArray originPoint) {
        string effectOriginItem = rpglEffect.GetOriginItem();
        string subeventOriginItem = subevent.GetOriginItem();
        return effectOriginItem is not null && effectOriginItem == subeventOriginItem;
    }

};
