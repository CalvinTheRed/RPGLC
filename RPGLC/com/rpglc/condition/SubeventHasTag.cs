using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.condition;

public class SubeventHasTag : Condition {

    public SubeventHasTag() : base("subevent_has_tag") { }

    public override bool Run(RPGLEffect rpglEffect, Subevent subevent, JsonObject conditionJson, RPGLContext context, JsonArray originPoint) {
        return subevent.HasTag(conditionJson.GetString("tag"));
    }

};
