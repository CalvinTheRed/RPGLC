using com.rpglc.subevent;

namespace com.rpglc.core;

public abstract class RPGLConfirmation {

    private static RPGLConfirmation INSTANCE;

    public static void SetInstance(RPGLConfirmation instance) {
        INSTANCE = instance;
    }

    public static RPGLConfirmation GetInstance() {
        return INSTANCE;
    }

    public abstract bool Confirm(RPGLEffect optionalEffect, Subevent triggeringSubevent);

}
