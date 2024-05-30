namespace com.rpglc.core;

public class PersistentContent : DatabaseContent {

    public long GetUuid() {
        return (long) GetInt("uuid");
    }

    public PersistentContent SetUuid(long uuid) {
        PutInt("uuid", uuid);
        return this;
    }

};
