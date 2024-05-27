namespace com.rpglc.core;

public class PersistentContent : DatabaseContent {

    public string GetUuid() {
        return base.GetString("uuid");
    }

    public void SetUuid(string uuid) {
        base.PutString("uuid", uuid);
    }

};
