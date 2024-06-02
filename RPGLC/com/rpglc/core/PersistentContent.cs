namespace com.rpglc.core;

public class PersistentContent : DatabaseContent {

    public string GetUuid() {
        return GetString("uuid");
    }

    public PersistentContent SetUuid(string uuid) {
        PutString("uuid", uuid);
        return this;
    }

};
