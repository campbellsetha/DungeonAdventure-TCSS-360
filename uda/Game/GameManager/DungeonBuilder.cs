using Godot;
using UDA.Game.Resources;
using UDA.Model.Map;

namespace UDA.Game.GameManager;

public partial class DungeonBuilder : Node2D
{
	private static readonly RoomTypeCollection MyRoomTypes = new ();
	private static readonly RoomConverter MyRoomConverter = new ();
	private static readonly Godot.Collections.Dictionary<RoomTypeCollection.RoomType, PackedScene> MyRoomTypeDict = MyRoomTypes.RoomDictionary;
	private static readonly Godot.Collections.Dictionary<string, RoomTypeCollection.RoomType> MyRoomStringToTypeDict = MyRoomConverter.baseRooms;
	
	
	// Characters that can appear in the middle
	private static char[] _middleCharacters = ['A', 'E', 'I', 'O', 'P', 'i', 'H', 'V', 'M'];

	private const int MyRoomCenter = 4;
	private const int MyRoomSideWidth = 224;
	private const int MyRoomHeight = 174;
	private int _myDungeonLength = 10;
	private int _myDungeonWidth = 10;
	
	// Called when the node enters the scene tree for the first time.
	// do not want this, only want to run if new game is called which should build the map.
	public override void _Ready()
	{
		//TODO: Place the map into a constant, then we can just reload the DungeonMap
		GD.Print(Dungeon.MyInstance.ToString());
		//DetermineRoom("****A|***");
		BuildDungeon();
	}

	//This would read through the values in the 2d array representing the dungeon
	//and instantiate the add the corresponding room in the engine(game)
	public void BuildDungeon()
	{
		for (int i = 0; i < _myDungeonLength; i++)
		{
			for (int j = 0; j < _myDungeonWidth; j++)
			{
				string currentRoom = string.Join("",Dungeon.MyInstance._myMap[i, j].GetDetails());
				PackedScene sceneToLoad = DetermineRoom(currentRoom);
				//Not sure why these coords need to get flipped, but they do
				LoadRoom(sceneToLoad,j,i);
				//GD.Print(Dungeon.MyInstance._myMap[i,j].GetDetails());
				//GD.Print("My builder Output = " + currentRoom);
			}
		}
	}

	private void LoadRoom(PackedScene theRoomToLoad, int theXCord, int theYCord)
	{
		var instancedRoom = theRoomToLoad.Instantiate();
		instancedRoom.Set(Node2D.PropertyName.GlobalPosition,
			new Vector2((float)theXCord * MyRoomSideWidth, (float)theYCord * MyRoomHeight));
		AddChild(instancedRoom);
	}

	private PackedScene DetermineRoom(string theRoom)
	{
		//TODO: Build A helper method to place objects/enemies/the player at the entrance
		//This should give us the character that contains what the item is
		char roomItem = theRoom[MyRoomCenter];
		
		//Pulling the character from the string to determine the room type
		string modifiedRoom = theRoom.Remove(MyRoomCenter,1);
		//Padding the string with a "*" to match the dictionary
		modifiedRoom = modifiedRoom.Insert(4, "*");

		RoomTypeCollection.RoomType theRoomType = MyRoomStringToTypeDict[modifiedRoom];
		
		//Getting the enum for the appropriate type that is mapped to the packed scenes
		
		
		//Getting the packed scene from the dictionary based on the room type
		PackedScene theRoomScene = MyRoomTypeDict[theRoomType];
		
		//TEST PRINT FOR STRING
		GD.Print(modifiedRoom);
		GD.Print(theRoomType);
		
		return theRoomScene;

	}
	

}