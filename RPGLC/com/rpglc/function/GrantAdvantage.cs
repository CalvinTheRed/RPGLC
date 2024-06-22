﻿using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

public class GrantAdvantage : Function {

    public GrantAdvantage() : base("grant_advantage") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is RollSubevent rollSubevent) {
            rollSubevent.GrantAdvantage();
        }
    }

};
