using com.rpglc.condition;
using com.rpglc.database;
using com.rpglc.database.TO;
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

    public static void AddRPGLEffectTemplate(RPGLEffectTemplate rpglEffectTemplate) {
        RPGL_EFFECT_TEMPLATES.Add(rpglEffectTemplate);
    }

    public static void RemoveRPGLEffectTemplate(RPGLEffectTemplate rpglEffectTemplate) {
        RPGL_EFFECT_TEMPLATES.Remove(rpglEffectTemplate);
    }

    public static List<RPGLEffectTemplate> GetRPGLEffectTemplates() {
        return [.. RPGL_EFFECT_TEMPLATES];
    }

    public static void AddRPGLEffect(RPGLEffect rpglEffect) {
        RPGL_EFFECTS.Add(rpglEffect);
    }

    public static void RemoveRPGLEffect(RPGLEffect rpglEffect) {
        RPGL_EFFECTS.Remove(rpglEffect);
    }

    public static List<RPGLEffect> GetRPGLEffects() {
        return [.. RPGL_EFFECTS];
    }

    public static void AddRPGLEventTemplate(RPGLEventTemplate rpglEventTemplate) {
        RPGL_EVENT_TEMPLATES.Add(rpglEventTemplate);
    }

    public static void RemoveRPGLEventTemplate(RPGLEventTemplate rpglEventTemplate) {
        RPGL_EVENT_TEMPLATES.Remove(rpglEventTemplate);
    }

    public static List<RPGLEventTemplate> GetRPGLEventTemplates() {
        return [.. RPGL_EVENT_TEMPLATES];
    }

    public static void AddRPGLItemTemplate(RPGLItemTemplate rpglItemTemplate) {
        RPGL_ITEM_TEMPLATES.Add(rpglItemTemplate);
    }

    public static void RemoveRPGLItemTemplate(RPGLItemTemplate rpglItemTemplate) {
        RPGL_ITEM_TEMPLATES.Remove(rpglItemTemplate);
    }

    public static List<RPGLItemTemplate> GetRPGLItemTemplates() {
        return [.. RPGL_ITEM_TEMPLATES];
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

    public static void AddRPGLObjectTemplate(RPGLObjectTemplate rpglObjectTemplate) {
        RPGL_OBJECT_TEMPLATES.Add(rpglObjectTemplate);
    }

    public static void RemoveRPGLObjectTemplate(RPGLObjectTemplate rpglObjectTemplate) {
        RPGL_OBJECT_TEMPLATES.Remove(rpglObjectTemplate);
    }

    public static List<RPGLObjectTemplate> GetRPGLObjectTemplates() {
        return [.. RPGL_OBJECT_TEMPLATES];
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

    public static void AddRPGLResourceTemplate(RPGLResourceTemplate rpglResourceTemplate) {
        RPGL_RESOURCE_TEMPLATES.Add(rpglResourceTemplate);
    }

    public static void RemoveRPGLResourceTemplate(RPGLResourceTemplate rpglResourceTemplate) {
        RPGL_RESOURCE_TEMPLATES.Remove(rpglResourceTemplate);
    }

    public static List<RPGLResourceTemplate> GetRPGLResourceTemplates() {
        return [.. RPGL_RESOURCE_TEMPLATES];
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

    public static void AddRPGLClass(RPGLClass rpglClass) {
        RPGL_CLASSES.Add(rpglClass);
    }

    public static void RemoveRPGLClass(RPGLClass rpglClass) {
        RPGL_CLASSES.Remove(rpglClass);
    }

    public static List<RPGLClass> GetRPGLClasses() {
        return [.. RPGL_CLASSES];
    }

    public static void AddRPGLRace(RPGLRace rpglRace) {
        RPGL_RACES.Add(rpglRace);
    }

    public static void RemoveRPGLRace(RPGLRace rpglRace) {
        RPGL_RACES.Remove(rpglRace);
    }

    public static List<RPGLRace> GetRPGLRaces() {
        return [.. RPGL_RACES];
    }

    public static void ClearRPGL() {
        RPGL_EFFECT_TEMPLATES.Clear();
        RPGL_EFFECTS.Clear();
        RPGL_EVENT_TEMPLATES.Clear();
        RPGL_ITEM_TEMPLATES.Clear();
        RPGL_ITEMS.Clear();
        RPGL_OBJECT_TEMPLATES.Clear();
        RPGL_OBJECTS.Clear(); 
        RPGL_RESOURCE_TEMPLATES.Clear();
        RPGL_RESOURCES.Clear(); 
        RPGL_CLASSES.Clear();
        RPGL_RACES.Clear();
    }

    public static void SaveToDatabase(string dbDir, string dbName) {
        DBManager.SetDatabase(dbDir, dbName);
        
    }

    public static void LoadFromDatabase(string dbDir, string dbName) {
        ClearRPGL();

        DBManager.SetDatabase(dbDir, dbName);
    }

};
