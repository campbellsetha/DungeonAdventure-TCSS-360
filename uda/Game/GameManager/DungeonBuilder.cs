using Godot;
using Godot.Collections;
using UDA.Game.Resources;
using UDA.inventory;
using UDA.Model.Items;
using UDA.Model.Map;

namespace UDA.Game.GameManager;

public partial class DungeonBuilder : Node2D
{
	private static readonly RoomTypeCollection MyRoomTypes = new ();
	private static readonly RoomConverter MyRoomConverter = new ();
	private static readonly Godot.Collections.Dictionary<RoomTypeCollection.RoomType, PackedScene> MyRoomTypeDict = MyRoomTypes.RoomDictionary;
	private static readonly Godot.Collections.Dictionary<string, RoomTypeCollection.RoomType> MyRoomStringToTypeDict = MyRoomConverter.baseRooms;
	private static PackedScene TheTrapScene = GD.Load<PackedScene>("res://Game/Items/ItemProtos/Trap.tscn");
	
	
	// Characters that can appear in the middle
	private static char[] _middleCharacters = ['A', 'E', 'I', 'O', 'P', 'i', 'H', 'V', 'M', 'X'];
	private char MyRoomItem;

	private const char TrapCharacter = 'X';
	private const int MyRoomCenter = 4;
	private const int MyRoomSideWidth = 224;
	private const int MyRoomHeight = 174;
	private int _myDungeonLength = 10;
	private int _myDungeonWidth = 10;

	
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("QueryPlayerInfo"))
		{
			GD.Print(GetNode<Player.Player>("Player").GlobalPosition);
			//GD.Print(GetNode<Player.Player>("Player").Inventory.ToString());
		}
	}

	// Called when the node enters the scene tree for the first time.
	// do not want this, only want to run if new game is called which should build the map.
	public override void _Ready()
	{
		//TODO: Place the map into a constant, then we can just reload the DungeonMap
		GD.Print(Dungeon.MyInstance.ToString());
		
		//These have to go in the Highest level container or they do not work
		ItemFactory.RegisterItem("heal_potion", "HealPotion","res://2D Pixel Dungeon Asset Pack/items and trap_animation/flasks/flasks_1_1.png");
		ItemFactory.RegisterItem("vision_potion", "VisionPotion","res://2D Pixel Dungeon Asset Pack/items and trap_animation/flasks/flasks_2_1.png");
		ItemFactory.RegisterItem("abstraction_pillar", "AbstractionPillar","res://2D Pixel Dungeon Asset Pack/items and trap_animation/flag/flag_1.png");
		ItemFactory.RegisterItem("encapsulation_pillar", "EncapsulationPillar","res://2D Pixel Dungeon Asset Pack/items and trap_animation/keys/keys_1_1.png");
		ItemFactory.RegisterItem("inheritance_pillar", "InheritancePillar","res://2D Pixel Dungeon Asset Pack/items and trap_animation/coin/coin_1.png");
		ItemFactory.RegisterItem("polymorphism_pillar", "PolymorphismPillar","res://2D Pixel Dungeon Asset Pack/items and trap_animation/torch/candlestick_1_1.png");
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
				LoadRoom(sceneToLoad,j,i);
				//GD.Print(Dungeon.MyInstance._myMap[i,j].GetDetails());
				//GD.Print("My builder Output = " + currentRoom);
			}
		}
	}

	private void LoadRoom(PackedScene theRoomToLoad, int theXCord, int theYCord)
	{
		//Had to flip the Y and X coords, it was building the right rooms but grabbing the wrong details
		var roomModel = Dungeon.MyInstance._myMap[theYCord, theXCord];
		
		var instancedRoom = theRoomToLoad.Instantiate();
		instancedRoom.Set(Node2D.PropertyName.GlobalPosition,
			new Vector2((float)theXCord * MyRoomSideWidth, (float)theYCord * MyRoomHeight));
		AddChild(instancedRoom);
		
		if (MyRoomItem == TrapCharacter)
		{
			PackedScene theTrap = TheTrapScene;
			var trapInstance = theTrap.Instantiate();
			trapInstance.Set(Node2D.PropertyName.GlobalPosition,
				new Vector2((float)theXCord * MyRoomSideWidth + MyRoomSideWidth/2,(float)theYCord * MyRoomHeight + MyRoomHeight/2));
			AddChild(trapInstance);
			
		}
		

		var itemSpawnRoot = instancedRoom.GetNodeOrNull<Node>("ItemSpawnPoints");
		
		if (itemSpawnRoot == null)
			return;
		
		var spawnMarkers = new Array<Node>(itemSpawnRoot.GetChildren());
		spawnMarkers.Shuffle();
			
		var ListOfRoomItems = ItemFactory.GetItemsFromRoom(roomModel);

		for (int i = 0; i < ListOfRoomItems.Count && i < spawnMarkers.Count; i++)
		{
			var itemData = ListOfRoomItems[i];
			var spawnPoint = (Marker2D)spawnMarkers[i];
			var itemScene = GD.Load<PackedScene>("res://Model/Items/Resources/" + itemData.Id.ToLower() + ".tscn");
			var itemNodeBase = itemScene.Instantiate();
			var itemNode = (ItemToPickup)itemNodeBase;
			
			itemNode.ItemData = itemData;
			itemNode.GlobalPosition = spawnPoint.Position;
			instancedRoom.AddChild(itemNode);
		}
		//AddChild(instancedRoom);
	}

	private PackedScene DetermineRoom(string theRoom)
	{
		//TODO: Build A helper method to place objects/enemies/the player at the entrance
		//This should give us the character that contains what the item is
		MyRoomItem = theRoom[MyRoomCenter];
		
		
		//Pulling the character from the string to determine the room type
		string modifiedRoom = theRoom.Remove(MyRoomCenter,1);
		//Padding the string with a "*" to match the dictionary
		modifiedRoom = modifiedRoom.Insert(4, "*");

		RoomTypeCollection.RoomType theRoomType = MyRoomStringToTypeDict[modifiedRoom];
		
		//Getting the enum for the appropriate type that is mapped to the packed scenes
		
		
		//Getting the packed scene from the dictionary based on the room type
		PackedScene theRoomScene = MyRoomTypeDict[theRoomType];
		return theRoomScene;
		
		//TEST PRINT FOR STRING
		//GD.Print(modifiedRoom);
		//GD.Print(theRoomType);

	}
}
