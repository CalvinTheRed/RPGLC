using com.rpglc.condition;
using com.rpglc.function;
using com.rpglc.subevent;

namespace com.rpglc.core;

public static class RPGL {

    public static void Init() {
        Condition.Initialize();
        Function.Initialize();
        Subevent.Initialize();
    }

    public static void InitTesting() {
        Condition.Initialize(true);
        Function.Initialize(true);
        Subevent.Initialize(true);
    }

    private static readonly List<RPGLEffect> RPGL_EFFECTS = [];
    private static readonly List<RPGLItem> RPGL_ITEMS = [];
    private static readonly List<RPGLObject> RPGL_OBJECTS = [];
    private static readonly List<RPGLResource> RPGL_RESOURCES = [];

    public static void AddRPGLEffect(RPGLEffect rpglEffect) {
        RPGL_EFFECTS.Add(rpglEffect);
    }

    public static void RemoveRPGLEffect(RPGLEffect rpglEffect) {
        RPGL_EFFECTS.Remove(rpglEffect);
    }

    public static List<RPGLEffect> GetRPGLEffects() {
        return [.. RPGL_EFFECTS];
    }

    public static void AddRPGLItem(RPGLItem rpglItem) {
        RPGL_ITEMS.Add(rpglItem);
    }

    public static void RemoveRPGLItem(RPGLItem rpglItem) {
        RPGL_ITEMS.Remove(rpglItem);
    }

    public static List<RPGLItem> GetRPGLItems() {
        return [.. RPGL_ITEMS];
    }

    public static void AddRPGLObject(RPGLObject rpglObject) {
        RPGL_OBJECTS.Add(rpglObject);
    }

    public static void RemoveRPGLObject(RPGLObject rpglObject) {
        RPGL_OBJECTS.Remove(rpglObject);
    }

    public static List<RPGLObject> GetRPGLObjects() {
        return [.. RPGL_OBJECTS];
    }

    public static void AddRPGLResource(RPGLResource rpglResource) {
        RPGL_RESOURCES.Add(rpglResource);
    }

    public static void RemoveRPGLResource(RPGLResource rpglResource) {
        RPGL_RESOURCES.Remove(rpglResource);
    }

    public static List<RPGLResource> GetRPGLResources() {
        return [.. RPGL_RESOURCES];
    }

    public static void ClearRPGL() {
        RPGL_EFFECTS.Clear();
        RPGL_ITEMS.Clear();
        RPGL_OBJECTS.Clear();
        RPGL_RESOURCES.Clear();
    }

};
