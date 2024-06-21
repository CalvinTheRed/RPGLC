using com.rpglc.core;
using com.rpglc.subevent;

namespace com.rpglc.testutils.core;

public class DummyContext : RPGLContext
{

    public override bool IsObjectsTurn(RPGLObject rpglObject)
    {
        return true;
    }

    public override void ViewCompletedSubevent(Subevent subevent) { }

};
