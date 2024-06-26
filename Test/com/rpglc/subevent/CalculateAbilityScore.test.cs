﻿using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[AssignDatabase]
[Collection("Serial")]
public class CalculateAbilityScoreTest {

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "calculates ability score")]
    public void CalculatesAbilityScore() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");

        CalculateAbilityScore calculateAbilityScore = new CalculateAbilityScore()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "ability": "str"
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .SetTarget(rpglObject)
            .Invoke(new DummyContext(), new());

        Assert.Equal(10, calculateAbilityScore.Get());
    }

};
