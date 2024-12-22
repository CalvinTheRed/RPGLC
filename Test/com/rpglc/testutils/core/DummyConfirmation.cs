using com.rpglc.subevent;

namespace com.rpglc.core;

public class DummyConfirmation : RPGLConfirmation {

    private static readonly Stack<bool> SCHEDULED_RESPONSES = [];

    public static void ScheduleResponse(bool response) {
        SCHEDULED_RESPONSES.Push(response);
    }

    public static void ClearResponses() {
        SCHEDULED_RESPONSES.Clear();
    }
    
    public override bool Confirm(
        RPGLEffect optionalEffect,
        Subevent triggeringSubevent
    ) {
        if (SCHEDULED_RESPONSES.Count > 0) {
            return SCHEDULED_RESPONSES.Pop();
        }
        return false;
    }

}