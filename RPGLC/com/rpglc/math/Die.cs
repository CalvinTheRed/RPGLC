using com.rpglc.json;

namespace com.rpglc.math;

public static class Die {

    private static readonly Random R = new();

    private static bool testing = false;

    public static long Roll(long upperBound, JsonArray? determinedList) {
        if (testing && determinedList is not null && determinedList.Count() > 0) {
            return (long) determinedList.RemoveInt(0);
        } else if (upperBound > 0L) {
            return R.Next((int) upperBound) + 1L;
        } else {
            return 0L;
        }
    }

    public static long Roll(JsonObject die) {
        long roll = Roll((long) die.GetInt("size"), die.GetJsonArray("determined"));
        die.PutInt("roll", roll);
        return roll;
    }

    public static void SetTesting(bool testing) {
        Die.testing = testing;
    }

    public static JsonArray Unpack(JsonArray dice) {
        JsonArray unpackedDice = new();
        for (int i = 0; i < dice.Count(); i++) {
            JsonObject die = dice.GetJsonObject(i);
            JsonObject unpackedDie = die.DeepClone();
            long count = unpackedDie.RemoveInt("count") ?? 1L;
            for (int j = 0; j < count; j++) {
                unpackedDice.AddJsonObject(unpackedDie.DeepClone());
            }
        }
        return unpackedDice;
    }
};
