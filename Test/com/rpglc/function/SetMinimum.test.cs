﻿using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;
using com.rpglc.testutils.subevent;

namespace com.rpglc.function;

[Collection("Serial")]
public class SetMinimumTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "sets minimum")]
    public void SetsMinimum() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        Subevent subevent = new DummyCalculationSubevent()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        new SetMinimum().Execute(
            new RPGLEffect(),
            subevent,
            new JsonObject().LoadFromString("""
                {
                    "function": "set_minimum",
                    "minimum": {
                        "formula": "number",
                        "number": 5,
                        "scale": {
                            "numerator": 2,
                            "denominator": 1,
                            "round_up": false
                        }
                    }
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.Equal(5 * 2, (subevent as CalculationSubevent).GetMinimum());
    }

};
