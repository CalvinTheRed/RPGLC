# RPGLC

###### v0.0.0-alpha

RPGLC (Role-Playing Game Library for C#) is a code library that models game mechanics described within the [SRD5 OGL](https://www.dndbeyond.com/srd). The aim of RPGLC is to make it easy to upload and automate user-defined game content that behaves in accordance with these rules.

## Installation

Use the package manager [TBD](google.com) to install foobar.

```bash
# installation instructions TBD
```

## Usage

### Initializing RPGLC
RPGLC must be initialized by calling `RPGL.Init()` before its other features will work as intended.

### Loading Datapacks
RPGLC must load game content from datapacks by calling `RPGL.LoadDatapacks(string path)` before it can model anything. Note that this method must be passed the path to a directory containing any datapacks you wish to load, not the path to a particular datapack.

Also note that if you create a save file, any game content loaded in will be stored in that save file, and loading it in the future will load that game content into RPGLC. You do not need to re-load the same game content from datapacks when you load from a save file.

### Unloading Datapacks
If you wish to un-load loaded datapacks from RPGLC, you must call `RPGL.ClearDatapacks()`. Note that this clears all datapacks currently loaded into RPGLC.

### Creating RPGLC Objects
To create an instance of a core RPGLC data type, you are expected to make use of functions supplied by the `RPGLFactory` class.

```c#
using copm.rpglc.core;

// creating RPGLObjects
RPGLObject goblin = RPGLFactory.NewObject("std:humanoid/goblin/grunt", "user-1", posArray, rotArray);

// create an RPGLEvent
RPGLEvent spearThrow = RPGLFactory.NewEvent("std:weapon/melee/simple/spear/throw");

// create an RPGLItem
RPGLItem spear = RPGLFactory.NewItem("std:weapon/spear");

// create an RPGLEffect (source and target are optional paramaters)
RPGLEffect poisoned = RPGLFactory.NewEffect("std:common/poisoned", source, target);

// create an RPGLResource
RPGLResource action = RPGLFactory.NewResource("std:common/action/01");
```

### Creating a RPGLContext
RPGLC does not provide a concrete implementation of RPGLContext. Client code will need to implement their own derived class to provide to RPGLC functions.

```c#
using com.rpglc.core;
using com.rpglc.subevent;

public class CustomContext : RPGLContext {
    public override bool IsObjectsTurn(RPGLObject rpglObject) {
        // define function
    }

    public override void ViewCompletedSubevent(Subevent subevent) {
        // define function
    }
};
```

### Example Program

```c#
using com.rpglc.core;
using com.rpglc.json;

public void ExampleProgram() {

    // initialize RPGLC

    RPGL.Init();

    // load game content from datapack

    RPGL.LoadDatapacks(@"path\to\datapacks");
    
    // create new RPGLObjects
    
    RPGLObject dragon = RPGLFactory.NewObject(
        "std:dragon/red/adult",
        "dm-1",
        new JsonArray().LoadFromString("[ 0, 20, 0 ]"),
        new JsonArray().LoadFromString("[ 0, 0, 0 ]")
    );
    RPGLObject knight = RPGLFactory.NewObject(
        "std:humanoid/knight",
        "player-1",
        new JsonArray().LoadFromString("[ 0, 20, 0 ]"),
        new JsonArray().LoadFromString("[ 0, 0, 0 ]")
    );

    // populate context

    RPGLContext context = new CustomContext()
        .Add(dragon)
        .Add(knight);

    // take an action

    List<RPGLEvent> dragonEvents = dragon.GetEventObjects(context);
    RPGLEvent? breathAttack = dragonEvents.FirstOrDefault(
        e => e.GetDatapackId() == "std:object/dragon/red/adult/breath"
    );

    List<RPGLResource> dragonResources = dragon.GetResourceObjects(context);
    RPGLResource action = dragonResources.GetFirstOrDefault(
        r => r.GetDatapackId() == "std:common/action/01"
    );
    RPGLResource breathAttackCharge = dragonResources.GetFirstOrDefault(
        r => r.GetDatapackId() == "std:object/common/breath_attack/33"
    );

    dragon.InvokeEvent(
        breathAttack,
        dragon.GetPosition(),
        [ action, breathAttackCharge ],
        [ knight ],
        context
    );
}
```

## Core Data Types

### RPGLObject <span style="font-size: 14px;">(<a href="https://example.com">read more</a>)</span>
The RPGLObject class can be used to model anything that may appear on a RPG battle map. It is primarily designed to model creatures, but it can also be used to model certain objects or structures.

### RPGLEvent <span style="font-size: 14px;">(<a href="https://example.com">read more</a>)</span>
The RPGLEvent class is used to model any verb that takes place in a RPG.

### RPGLItem <span style="font-size: 14px;">(<a href="https://example.com">read more</a>)</span>
The RPGLItem class is used to model any physical item that might be used in an RPG.

### RPGLEffect <span style="font-size: 14px;">(<a href="https://example.com">read more</a>)</span>
The RPGLEffect class is used to model any status effect, condition, or other feature that has the ability to influence the behavior or outcome of an RPGLEvent.

### RPGLResource <span style="font-size: 14px;">(<a href="https://example.com">read more</a>)</span>
The RPGLResource class is used to model anything that can be used to fuel an RPGLEvent.

### RPGLClass <span style="font-size: 14px;">(<a href="https://example.com">read more</a>)</span>
The RPGLClass class is used to model a progression for gaining or losing RPGLEffects, RPGLEvents, and RPGLResources as an RPGLObject gains levels in a particular trade or profession. This is primarily intended to model RPG character classes, but it is flexible enough to be easily applied to any RPGLObject, not just player characters.

### RPGLRace <span style="font-size: 14px;">(<a href="https://example.com">read more</a>)</span>
The RPGLResource class behaves identically to the RPGLClass class, though its features are gained or lost any time the RPGLObject gains a level in any class. It is designed to model a creature's race, or species, or ancestry, rather than a trade or profession.

### RPGLContext <span style="font-size: 14px;">(<a href="https://example.com">read more</a>)</span>
The RPGLContext class is used to model the context in which an RPGLEvent takes place. Only RPGLEffects of RPGLObjects included in the relevant context can affect an RPGLEvent.