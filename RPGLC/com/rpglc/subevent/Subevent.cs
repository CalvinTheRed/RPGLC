using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.subevent;

public abstract class Subevent {

    public static readonly Dictionary<string, Subevent> Subevents = [];

    public JsonObject json = new();

    public List<RPGLEffect> appliedEffects = [];

    readonly string subeventId;

    public static void Initialize(bool includeTestingSubevents = false) {
        Subevents.Clear();

        Initialize([
            new AttackRoll(),
            new DealDamage(),
            new Heal(),
            new SavingThrow(),
        ]);

        if (includeTestingSubevents) {
            Initialize([
                new DummySubevent(),
            ]);
        }
    }

    private static void Initialize(List<Subevent> subevents) {
        foreach (Subevent subevent in subevents) {
            Subevents.Add(subevent.subeventId, subevent);
        }
    }

    public Subevent(string subeventId) {
        this.subeventId = subeventId;
        json.PutString("subevent", subeventId);
        if (!json.AsDict().ContainsKey("tags")) {
            json.PutJsonArray("tags", new());
        }
        AddTag(subeventId);
    }

    public JsonArray GetTags() {
        return json.GetJsonArray("tags");
    }

    public Subevent AddTag(string tag) {
        GetTags().AddString(tag);
        return this;
    }

    public bool HasTag(string tag) {
        return GetTags().Contains(tag);
    }

    public bool VerifySubevent(string expected) {
        return expected == json.GetString("subevent");
    }

    public virtual Subevent JoinSubeventData(JsonObject other) {
        json.Join(other);
        return this;
    }

    public abstract Subevent Clone();

    public abstract Subevent Clone(JsonObject jsonData);

    public abstract Subevent Prepare(RPGLContext context, JsonArray originPoint);

    public virtual Subevent? Invoke(RPGLContext context, JsonArray originPoint) {
        if (VerifySubevent(subeventId)) {
            context.ProcessSubevent(this, originPoint);
            Run(context, originPoint);
            context.ViewCompletedSubevent(this);
            return this;
        }
        return null;
    }

    public abstract Subevent Run(RPGLContext context, JsonArray originPoint);

    public void AddModifyingEffect(RPGLEffect rpglEffect) {
        appliedEffects.Add(rpglEffect);
    }

    public bool IsEffectApplied(RPGLEffect rpglEffect) {
        foreach (RPGLEffect appliedEffect in appliedEffects) {
            if ((rpglEffect.GetDatapackId() == appliedEffect.GetDatapackId() && !rpglEffect.HasTag("allow_duplicates"))
                || rpglEffect.GetUuid() == appliedEffect.GetUuid()
            ) {
                return true;
            }
        }
        return false;
    }

    public RPGLObject GetSource() {
        return RPGLObject.GetRPGLObjects().Find(x => x.GetUuid() == json.GetString("source"));
    }

    public virtual Subevent SetSource(RPGLObject source) {
        json.PutString("source", source.GetUuid());
        return this;
    }

    public RPGLObject GetTarget() {
        return RPGLObject.GetRPGLObjects().Find(x => x.GetUuid() == json.GetString("target"));
    }

    public virtual Subevent SetTarget(RPGLObject target) {
        json.PutString("target", target.GetUuid());
        return this;
    }

    public string GetSubeventId() {
        return subeventId;
    }

    public string? GetOriginItem() {
        return json.GetString("origin_item");
    }

    public virtual Subevent SetOriginItem(string? originItem) {
        json.PutString("origin_item", originItem);
        return this;
    }

    public override string ToString() {
        return json.ToString();
    }

    public string PrettyPrint() {
        return json.PrettyPrint();
    }

};
