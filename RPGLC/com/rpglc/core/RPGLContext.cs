using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.core;

public abstract class RPGLContext {

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
