using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.core;

public abstract class RPGLContext {

    // =====================================================================
    // Static code.
    // =====================================================================

    private static readonly List<RPGLContext> RPGL_CONTEXTS = [];
    
    public static void AddContext(RPGLContext context) {
        RPGL_CONTEXTS.Add(context);
    }

    public static void RemoveContext(RPGLContext context) {
        RPGL_CONTEXTS.Remove(context);
    }

    public static List<RPGLContext> GetContexts() {
        return [.. RPGL_CONTEXTS];
    }

    // =====================================================================
    // Instance code.
    // =====================================================================

    private readonly List<RPGLObject> contextObjects;

    public RPGLContext() {
        contextObjects = [];
    }

    public abstract bool IsObjectsTurn(RPGLObject rpglObject);

    public void ProcessSubevent(Subevent subevent, JsonArray originPoint) {
        bool wasProcessed;
        do {
            wasProcessed = false;
            foreach (RPGLObject rpglObject in contextObjects) {
                wasProcessed |= rpglObject.ProcessSubevent(subevent, this, originPoint);
            }
        } while (wasProcessed);
    }

    public RPGLContext Add(RPGLObject rpglObject) {
        contextObjects.Add(rpglObject);
        return this;
    }

    public RPGLContext Remove(RPGLObject rpglObject) {
        contextObjects.Remove(rpglObject);
        return this;
    }

    public RPGLContext Clear() {
        contextObjects.Clear();
        return this;
    }

    public RPGLContext Merge(RPGLContext other) {
        contextObjects.AddRange(other.contextObjects);
        return this;
    }

    public List<RPGLObject> GetContextObjects() {
        return new List<RPGLObject>(contextObjects);
    }

    public abstract void ViewCompletedSubevent(Subevent subevent);

};
