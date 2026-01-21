using com.rpglc.core;
using com.rpglc.subevent;

namespace com.rpglc.runtime;

public class SubeventState(Subevent subevent) {
    
    public struct StateData {
        public SubeventState? dependency;
        public Phase? nextPhase;
        public bool stepCompleted;
    };

    public enum Phase {
        Preparing,
        Targeting,
        Running,
        Completed,
    };

    public readonly Subevent subevent = subevent;
    public Phase phase = Phase.Preparing;
    public int stepIndex = 0;

    private readonly Stack<RPGLObject> targets = [];

    public (SubeventState? subevent, bool canProceed, bool completed) Advance(RPGLContext context) {
        switch(phase) {
            case Phase.Preparing:
                return AdvancePreparing(context);
            case Phase.Targeting:
                return AdvanceTargeting(context);
            case Phase.Running:
                return AdvanceRunning(context);
            default:
                return (null, true, true);
        }
    }

    public void SetTargets(List<RPGLObject> targets) {
        targets.ForEach(this.targets.Push);
    }

    public (SubeventState? subevent, bool canProceed, bool completed) AdvancePreparing(RPGLContext context) {
        StateData response = subevent.subeventSteps[stepIndex](context);
        if (response.stepCompleted) {
            stepIndex++;
        }
        if (response.nextPhase is not null) {
            phase = (Phase) response.nextPhase;
        }
        if (stepIndex == subevent.subeventSteps.Count) {
            phase = Phase.Completed;
        }
        return (response.dependency, response.dependency is null, stepIndex == subevent.subeventSteps.Count);
    }

    public (SubeventState? subevent, bool canProceed, bool completed) AdvanceTargeting(RPGLContext context) {
        if (targets.Count == 0) {
            return (null, false, false);
        } else {
            SubeventState dependencySubevent = new(subevent.Clone());
            dependencySubevent.subevent.SetTarget(targets.Pop());
            dependencySubevent.phase = Phase.Running;
            dependencySubevent.stepIndex = stepIndex;

            return (dependencySubevent, true, targets.Count == 0);
        }
    }

    public (SubeventState? subevent, bool canProceed, bool completed) AdvanceRunning(RPGLContext context) {
        StateData response = subevent.subeventSteps[stepIndex](context);
        if (response.stepCompleted) {
            stepIndex++;
        }
        if (stepIndex == subevent.subeventSteps.Count) {
            phase = Phase.Completed;
        }

        return (response.dependency, response.dependency is null, stepIndex == subevent.subeventSteps.Count);
    }

};
