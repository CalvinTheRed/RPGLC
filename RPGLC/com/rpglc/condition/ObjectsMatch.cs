﻿using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.condition;

public class ObjectsMatch : Condition {

    public ObjectsMatch() : base("objects_match") { }

    public override bool Run(RPGLEffect rpglEffect, Subevent subevent, JsonObject conditionJson, RPGLContext context, JsonArray originPoint) {
        RPGLObject effectObject = RPGLEffect.GetObject(rpglEffect, subevent, new JsonObject().LoadFromString($$"""
            {
                "from": "effect",
                "object": "{{conditionJson.GetString("effect")}}"
            }
            """));
        RPGLObject subeventObject = RPGLEffect.GetObject(rpglEffect, subevent, new JsonObject().LoadFromString($$"""
            {
                "from": "subevent",
                "object": "{{conditionJson.GetString("subevent")}}"
            }
            """));

        return Equals(effectObject.GetUuid(), subeventObject.GetUuid());
    }

};
