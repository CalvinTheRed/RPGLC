using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Applies a list of vampiric behaviors to a damage-dealing subevent.
///   
///   <code>
///   {
///     "function": "add_healing",
///     "vampirism": [
///       &lt;vampirism formula&gt;
///     ]
///   }
///   </code>
///   
///   <i>Note that this function should be paired with a condition checking for the subevent to have either the "base" or "target" tag. Neglecting this check will cause the vampiric behavior to be applied to a damaging subevent more than once.</i>
///   
///   <list type="bullet">
///     <item>
///       "vampirism" is an optional field and will default to a value of
///       <code>
///       [
///         {
///           "damage_type": "*",
///           "scale": {
///             "numerator": 1,
///             "denominator": 1,
///             "round_up": false
///           }
///         }
///       ]
///       </code>
///       if not specified. This field is a list of vampirism formulae to be applied to a damaging subevent.
///     </item>
///   </list>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>AttackRoll</item>
///     <item>DealDamage</item>
///     <item>SavingThrow</item>
///   </list>
///   
/// </summary>
public class AddVampirism : Function {

    public AddVampirism() : base("add_vampirism") {
        functionSteps.AddRange([
            (rpglEffect, subevent, functionJson, context) => {
                if (subevent is IVampiricSubevent vampiricSubevent) {
                    JsonArray vampirismArray = functionJson.GetJsonArray("vampirism") ?? new JsonArray();
                    if (vampirismArray.IsEmpty()) {
                        vampirismArray.AddJsonObject(new());
                    }
                    for (int i = 0; i < vampirismArray.Count(); i++) {
                        JsonObject vampirismJson = vampirismArray.GetJsonObject(i);
                        vampiricSubevent.AddVampirism(subevent, new JsonObject().LoadFromString($$"""
                            {
                                "damage_type": "{{vampirismJson.GetString("damage_type") ?? "*"}}",
                                "scale": {{vampirismJson.GetJsonObject("scale")?.ToString() ?? """
                                {
                                    "numerator": 1,
                                    "denominator": 2,
                                    "round_up": false
                                }
                                """}}
                            }
                            """));
                    }
                }
                return new() {
                    dependency = null,
                    completed = true,
                };
            }
        ]);
    }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is IVampiricSubevent vampiricSubevent) {
            JsonArray vampirismArray = functionJson.GetJsonArray("vampirism") ?? new JsonArray().AddJsonObject(new());
            if (vampirismArray.IsEmpty()) {
                vampirismArray.AddJsonObject(new());
            }
            for (int i = 0; i < vampirismArray.Count(); i++) {
                vampiricSubevent.AddVampirism(subevent, vampirismArray.GetJsonObject(i));
            }
        }
    }

};
