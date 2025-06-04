using Godot;

namespace UDA.Game.GameManager;

public partial class DungeonBuilder : Node2D
{
    private const int MyRoomSideWidth = 224;
    private const int MyRoomHeight = 174;
    private static readonly RoomTypeCollection _myRoomTypes = new();

    private static readonly Godot.Collections.Dictionary<RoomTypeCollection.RoomType, PackedScene> _myRoomTypeDict =
        _myRoomTypes.RoomDictionary;

    private int _myDungeonLength = 5;
    private int _myDungeonWidth = 5;

    // Called when the node enters the scene tree for the first time.
    // do not want this, only want to run if new game is called which should build the map.
    public override void _Ready()
    {
        BuildDungeon();
    }

    //This would read through the values in the 2d array representing the dunegon
    //and instantiate the add the corresponding room in the engine(game)
    public void BuildDungeon()
    {
        for (var i = 0; i < _myDungeonLength; i++)
        for (var j = 0; j < _myDungeonWidth; j++)
        {
            //Access the room type and figure out what relates to the appropriate key in the dict
            //Probably a bunch of conditionals
            //This is only going to load in a bunch of four door rooms
            //TODO: Update the logic here to select the correct room type 
            //This should almost certainly be placed into a helper method as it is essentially going to be a huge
            //switch statement
            var roomToLoad = _myRoomTypeDict[RoomTypeCollection.RoomType.FourDoor];
            //Instantiate
            //TODO: This should also probably be placed into helper method VVV
            var instancedRoom = roomToLoad.Instantiate();
            instancedRoom.Set(Node2D.PropertyName.GlobalPosition,
                new Vector2((float)i * MyRoomSideWidth, (float)j * MyRoomHeight));
            AddChild(instancedRoom);
        }
    }
}