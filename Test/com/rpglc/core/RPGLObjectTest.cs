using com.rpglc.database;
using com.rpglc.testutils;
using com.rpglc.testutils.mocks;

namespace com.rpglc.core;

[AssignDatabase]
[Collection("Serial")]
public class RPGLObjectTest {

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "manipulates item in inventory")]
    public void GivesAndTakesItems() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        RPGLItem rpglItem = RPGLFactory.NewItem("test:dummy");

        // give item
        rpglObject.GiveItem(rpglItem.GetUuid());
        rpglObject = DBManager.QueryRPGLObject(x => x.UserId == "Player 1");
        Assert.Equal(1, rpglObject.GetInventory().Count());
        Assert.Equal(rpglItem.GetUuid(), rpglObject.GetInventory().GetString(0));

        // give item again
        rpglObject.GiveItem(rpglItem.GetUuid());
        rpglObject = DBManager.QueryRPGLObject(x => x.UserId == "Player 1");
        Assert.Equal(1, rpglObject.GetInventory().Count());
        Assert.Equal(rpglItem.GetUuid(), rpglObject.GetInventory().GetString(0));

        // equip item
        rpglObject.EquipItem(rpglItem.GetUuid(), "mainhand");
        rpglObject = DBManager.QueryRPGLObject(x => x.UserId == "Player 1");
        Assert.Equal(1, rpglObject.GetEquippedItems().AsDict().Keys.Count());
        Assert.Equal(
            rpglItem.GetUuid(),
            rpglObject.GetEquippedItems().GetString("mainhand")
        );

        // unequip item
        rpglObject.UnequipItem("mainhand");
        rpglObject = DBManager.QueryRPGLObject(x => x.UserId == "Player 1");
        Assert.Equal(0, rpglObject.GetEquippedItems().AsDict().Keys.Count());

        // take item
        rpglObject.TakeItem(rpglItem.GetUuid());
        rpglObject = DBManager.QueryRPGLObject(x => x.UserId == "Player 1");
        Assert.Equal(0, rpglObject.GetInventory().Count());

        // give, equip, and take item
        rpglObject.GiveItem(rpglItem.GetUuid());
        rpglObject.EquipItem(rpglItem.GetUuid(), "mainhand");
        rpglObject.TakeItem(rpglItem.GetUuid());
        rpglObject = DBManager.QueryRPGLObject(x => x.UserId == "Player 1");
        Assert.Equal(0, rpglObject.GetInventory().Count());
        Assert.Equal(0, rpglObject.GetEquippedItems().AsDict().Keys.Count());
    }

    [ExtraClassesMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "levels up")]
    public void LevelsUp() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        
        rpglObject.LevelUp("test:class_with_nested_class", new());
        rpglObject = DBManager.QueryRPGLObject(x => x.UserId == "Player 1");
        Assert.Equal(1, rpglObject.GetLevel());
        Assert.Equal(1, rpglObject.GetLevel("test:class_with_nested_class"));
        Assert.Equal(1, rpglObject.GetLevel("test:nested_class"));

        rpglObject.AddAdditionalNestedClass(
            "test:class_with_nested_class",
            "test:additional_nested_class",
            2L,
            false
        );
        // ^ this method is meant to be called immediately before a level-up,
        // and will not retro-fit any features or update the database on its own.
        Assert.Equal(0, rpglObject.GetLevel("test:additional_nested_class"));

        rpglObject.LevelUp("test:class_with_nested_class", new());
        rpglObject = DBManager.QueryRPGLObject(x => x.UserId == "Player 1");
        Assert.Equal(2, rpglObject.GetLevel());
        Assert.Equal(2, rpglObject.GetLevel("test:class_with_nested_class"));
        Assert.Equal(2, rpglObject.GetLevel("test:nested_class"));
        Assert.Equal(1, rpglObject.GetLevel("test:additional_nested_class"));
    }

};
