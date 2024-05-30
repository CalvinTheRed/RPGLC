namespace com.rpglc.math;

public static class RPGLRandom {

    private static Random? r = null;

    public static Random GetRandom() {
        if (r is null) {
            r = new Random(DateTime.Now.Millisecond);
        }
        return r;
    }

};

public static class Die {

};
