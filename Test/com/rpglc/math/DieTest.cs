using com.rpglc.json;
using com.rpglc.math;

namespace Test.com.rpglc.math;

public class DieTest {

    [Fact(DisplayName = "unpacks")]
    public void Unpacks() {
        JsonArray unpackedDice = Die.Unpack(new JsonArray().LoadFromString("""
            [
                {
                    "count": 2,
                    "size": 6,
                    "determined": [ 3 ]
                }
            ]
            """));

        Assert.Equal("""
            [
              {
                "determined": [
                  3
                ],
                "size": 6
              },
              {
                "determined": [
                  3
                ],
                "size": 6
              }
            ]
            """, unpackedDice.PrettyPrint());
    }

}
