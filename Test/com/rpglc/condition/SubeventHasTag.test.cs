﻿using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.condition;

[AssignDatabase]
[Collection("Serial")]
public class SubeventHasTagTest {

    [Fact(DisplayName = "condition mismatch")]
    public void ConditionMismatch() {
        bool result = new SubeventHasTag().Evaluate(new(), new DummySubevent(), new JsonObject().LoadFromString("""
            {
                "condition": "not-a-condition"
            }
            """), new DummyContext(), new());

        Assert.False(result);
    }

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "subevent does have tag")]
    public void SubeventDoesHaveTag() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");

        bool result = new SubeventHasTag().Evaluate(
            new(),
            new DummySubevent().AddTag("test_tag"),
            new JsonObject().LoadFromString("""
                {
                    "condition": "subevent_has_tag",
                    "tag": "test_tag"
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.True(result);
    }

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "subevent does not have tag")]
    public void SubeventDoesNotHaveTag() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");

        bool result = new SubeventHasTag().Evaluate(
            new(),
            new DummySubevent(),
            new JsonObject().LoadFromString("""
                {
                    "condition": "subevent_has_tag",
                    "tag": "test_tag"
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.False(result);
    }

};
