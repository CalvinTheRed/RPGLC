using com.rpglc.condition;
using com.rpglc.data;
using com.rpglc.function;
using com.rpglc.subevent;

namespace com.rpglc.core;

public static class RPGL {

    private static readonly List<RPGLEffectTemplate> RPGL_EFFECT_TEMPLATES = [];
    private static readonly List<RPGLEffect> RPGL_EFFECTS = [];
    private static readonly List<RPGLEventTemplate> RPGL_EVENT_TEMPLATES = [];
    private static readonly List<RPGLItemTemplate> RPGL_ITEM_TEMPLATES = [];
    private static readonly List<RPGLItem> RPGL_ITEMS = [];
    private static readonly List<RPGLObjectTemplate> RPGL_OBJECT_TEMPLATES = [];
    private static readonly List<RPGLObject> RPGL_OBJECTS = [];
    private static readonly List<RPGLResourceTemplate> RPGL_RESOURCE_TEMPLATES = [];
    private static readonly List<RPGLResource> RPGL_RESOURCES = [];
    private static readonly List<RPGLClass> RPGL_CLASSES = [];
    private static readonly List<RPGLRace> RPGL_RACES = [];

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

    public static void LoadDatapacks(string path) {
        foreach (string datapackPath in Directory.GetDirectories(path)) {
            _ = new Datapack(datapackPath);
        }
    }

    public static void ClearDatapacks() {
        RPGL_EFFECT_TEMPLATES.Clear();
        RPGL_EVENT_TEMPLATES.Clear();
        RPGL_ITEM_TEMPLATES.Clear();
        RPGL_OBJECT_TEMPLATES.Clear();
        RPGL_RESOURCE_TEMPLATES.Clear();
        RPGL_EFFECTS.Clear();
        RPGL_ITEMS.Clear();
        RPGL_OBJECTS.Clear();
        RPGL_RESOURCES.Clear();
        RPGL_CLASSES.Clear();
        RPGL_RACES.Clear();
    }

    public static void SaveToDatabase(string dbDir, string dbName) {
        DBManager.SetDatabase(dbDir, dbName);


        foreach (var data in RPGL_EFFECT_TEMPLATES) {
            DBManager.InsertRPGLEffectTemplate(data);
        }
        foreach (var data in RPGL_EVENT_TEMPLATES) {
            DBManager.InsertRPGLEventTemplate(data);
        }
        foreach (var data in RPGL_ITEM_TEMPLATES) {
            DBManager.InsertRPGLItemTemplate(data);
        }
        foreach (var data in RPGL_OBJECT_TEMPLATES) {
            DBManager.InsertRPGLObjectTemplate(data);
        }
        foreach (var data in RPGL_RESOURCE_TEMPLATES) {
            DBManager.InsertRPGLResourceTemplate(data);
        }
        foreach (var data in RPGL_EFFECTS) {
            DBManager.InsertRPGLEffect(data);
        }
        foreach (var data in RPGL_ITEMS) {
            DBManager.InsertRPGLItem(data);
        }
        foreach (var data in RPGL_OBJECTS) {
            DBManager.InsertRPGLObject(data);
        }
        foreach (var data in RPGL_RESOURCES) {
            DBManager.InsertRPGLResource(data);
        }
        foreach (var data in RPGL_CLASSES) {
            DBManager.InsertRPGLClass(data);
        }
        foreach (var data in RPGL_RACES) {
            DBManager.InsertRPGLRace(data);
        }
    }

    public static void LoadFromDatabase(string dbDir, string dbName) {
        DBManager.SetDatabase(dbDir, dbName);
        ClearDatapacks();

        foreach (var data in DBManager.QueryRPGLEffectTemplates()) {
            RPGL_EFFECT_TEMPLATES.Add(data);
        }
        foreach (var data in DBManager.QueryRPGLEventTemplates()) {
            RPGL_EVENT_TEMPLATES.Add(data);
        }
        foreach (var data in DBManager.QueryRPGLItemTemplates()) {
            RPGL_ITEM_TEMPLATES.Add(data);
        }
        foreach (var data in DBManager.QueryRPGLObjectTemplates()) {
            RPGL_OBJECT_TEMPLATES.Add(data);
        }
        foreach (var data in DBManager.QueryRPGLResourceTemplates()) {
            RPGL_RESOURCE_TEMPLATES.Add(data);
        }
        foreach (var data in DBManager.QueryRPGLEffects()) {
            RPGL_EFFECTS.Add(data);
        }
        foreach (var data in DBManager.QueryRPGLItems()) {
            RPGL_ITEMS.Add(data);
        }
        foreach (var data in DBManager.QueryRPGLObjects()) {
            RPGL_OBJECTS.Add(data);
        }
        foreach (var data in DBManager.QueryRPGLResources()) {
            RPGL_RESOURCES.Add(data);
        }
        foreach (var data in DBManager.QueryRPGLClasses()) {
            RPGL_CLASSES.Add(data);
        }
        foreach (var data in DBManager.QueryRPGLRaces()) {
            RPGL_RACES.Add(data);
        }
    }

    // =====================================================================
    // Add data
    // =====================================================================

    public static void AddRPGLEffectTemplate(RPGLEffectTemplate rpglEffectTemplate) {
        RPGL_EFFECT_TEMPLATES.Add(rpglEffectTemplate);
    }

    public static void AddRPGLEventTemplate(RPGLEventTemplate rpglEventTemplate) {
        RPGL_EVENT_TEMPLATES.Add(rpglEventTemplate);
    }

    public static void AddRPGLItemTemplate(RPGLItemTemplate rpglItemTemplate) {
        RPGL_ITEM_TEMPLATES.Add(rpglItemTemplate);
    }

    public static void AddRPGLObjectTemplate(RPGLObjectTemplate rpglObjectTemplate) {
        RPGL_OBJECT_TEMPLATES.Add(rpglObjectTemplate);
    }

    public static void AddRPGLResourceTemplate(RPGLResourceTemplate rpglResourceTemplate) {
        RPGL_RESOURCE_TEMPLATES.Add(rpglResourceTemplate);
    }

    public static void AddRPGLEffect(RPGLEffect rpglEffect) {
        RPGL_EFFECTS.Add(rpglEffect);
    }

    public static void AddRPGLItem(RPGLItem rpglItem) {
        RPGL_ITEMS.Add(rpglItem);
    }

    public static void AddRPGLObject(RPGLObject rpglObject) {
        RPGL_OBJECTS.Add(rpglObject);
    }

    public static void AddRPGLResource(RPGLResource rpglResource) {
        RPGL_RESOURCES.Add(rpglResource);
    }

    public static void AddRPGLClass(RPGLClass rpglClass) {
        RPGL_CLASSES.Add(rpglClass);
    }

    public static void AddRPGLRace(RPGLRace rpglRace) {
        RPGL_RACES.Add(rpglRace);
    }

    // =====================================================================
    // Remove data
    // =====================================================================

    public static void RemoveRPGLEffectTemplate(RPGLEffectTemplate rpglEffectTemplate) {
        RPGL_EFFECT_TEMPLATES.Remove(rpglEffectTemplate);
    }

    public static void RemoveRPGLEventTemplate(RPGLEventTemplate rpglEventTemplate) {
        RPGL_EVENT_TEMPLATES.Remove(rpglEventTemplate);
    }

    public static void RemoveRPGLItemTemplate(RPGLItemTemplate rpglItemTemplate) {
        RPGL_ITEM_TEMPLATES.Remove(rpglItemTemplate);
    }

    public static void RemoveRPGLObjectTemplate(RPGLObjectTemplate rpglObjectTemplate) {
        RPGL_OBJECT_TEMPLATES.Remove(rpglObjectTemplate);
    }

    public static void RemoveRPGLResourceTemplate(RPGLResourceTemplate rpglResourceTemplate) {
        RPGL_RESOURCE_TEMPLATES.Remove(rpglResourceTemplate);
    }

    public static void RemoveRPGLEffect(RPGLEffect rpglEffect) {
        RPGL_EFFECTS.Remove(rpglEffect);
    }

    public static void RemoveRPGLItem(RPGLItem rpglItem) {
        RPGL_ITEMS.Remove(rpglItem);
    }

    public static void RemoveRPGLObject(RPGLObject rpglObject) {
        RPGL_OBJECTS.Remove(rpglObject);
    }

    public static void RemoveRPGLResource(RPGLResource rpglResource) {
        RPGL_RESOURCES.Remove(rpglResource);
    }

    public static void RemoveRPGLClass(RPGLClass rpglClass) {
        RPGL_CLASSES.Remove(rpglClass);
    }

    public static void RemoveRPGLRace(RPGLRace rpglRace) {
        RPGL_RACES.Remove(rpglRace);
    }

    // =====================================================================
    // Get data
    // =====================================================================

    public static List<RPGLEffectTemplate> GetRPGLEffectTemplates() {
        return [.. RPGL_EFFECT_TEMPLATES];
    }

    public static List<RPGLEventTemplate> GetRPGLEventTemplates() {
        return [.. RPGL_EVENT_TEMPLATES];
    }

    public static List<RPGLItemTemplate> GetRPGLItemTemplates() {
        return [.. RPGL_ITEM_TEMPLATES];
    }

    public static List<RPGLObjectTemplate> GetRPGLObjectTemplates() {
        return [.. RPGL_OBJECT_TEMPLATES];
    }

    public static List<RPGLResourceTemplate> GetRPGLResourceTemplates() {
        return [.. RPGL_RESOURCE_TEMPLATES];
    }

    public static List<RPGLEffect> GetRPGLEffects() {
        return [.. RPGL_EFFECTS];
    }

    public static List<RPGLItem> GetRPGLItems() {
        return [.. RPGL_ITEMS];
    }

    public static List<RPGLObject> GetRPGLObjects() {
        return [.. RPGL_OBJECTS];
    }

    public static List<RPGLResource> GetRPGLResources() {
        return [.. RPGL_RESOURCES];
    }

    public static List<RPGLClass> GetRPGLClasses() {
        return [.. RPGL_CLASSES];
    }

    public static List<RPGLRace> GetRPGLRaces() {
        return [.. RPGL_RACES];
    }

};
