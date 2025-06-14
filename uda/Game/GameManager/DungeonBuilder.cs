using Godot;
using Godot.Collections;
using UDA.Game.Resources;
using UDA.inventory;
using UDA.Model;
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
	private static PackedScene TheExitPortal = GD.Load<PackedScene>("res://Game/Resources/ExitPortal.tscn");
	private static MonsterDictionary _myMonsterDictionary = GD.Load<MonsterDictionary>("res://Game/Resources/MonsterDictionary.tres");
	
	
	// Characters that can appear in the middle
	private static char[] _pillarCharacters = ['A', 'E', 'P', 'I'];
	private char MyRoomItem;
	private char MyEntrance = 'i';
	private char MyExit = 'O';

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
		//These have to go in the Highest level container or they do not work
		ItemFactory.RegisterItem("heal_potion", "HealPotion","res://2D Pixel Dungeon Asset Pack/items and trap_animation/flasks/flasks_1_1.png");
		ItemFactory.RegisterItem("vision_potion", "VisionPotion","res://2D Pixel Dungeon Asset Pack/items and trap_animation/flasks/flasks_2_1.png");
		ItemFactory.RegisterItem("abstraction_pillar", "AbstractionPillar","res://2D Pixel Dungeon Asset Pack/items and trap_animation/flag/flag_1.png");
		ItemFactory.RegisterItem("encapsulation_pillar", "EncapsulationPillar","res://2D Pixel Dungeon Asset Pack/items and trap_animation/keys/keys_1_1.png");
		ItemFactory.RegisterItem("inheritance_pillar", "InheritancePillar","res://2D Pixel Dungeon Asset Pack/items and trap_animation/coin/coin_1.png");
		ItemFactory.RegisterItem("polymorphism_pillar", "PolymorphismPillar","res://2D Pixel Dungeon Asset Pack/items and trap_animation/torch/candlestick_1_1.png");
		BuildDungeon();
		//TODO: Place the map into a constant, then we can just reload the DungeonMap
		GD.Print(Dungeon.MyInstance.ToString());
	}

	//This would read through the values in the 2d array representing the dungeon
	//and instantiate the add the corresponding room in the engine(game)
	public void BuildDungeon()
	{
		for (int i = 0; i < _myDungeonLength; i++)
		{
			for (int j = 0; j < _myDungeonWidth; j++)
			{
				string currentRoom = string.Join("",Dungeon.MyInstance.MyMap[i, j].GetDetails());
				PackedScene sceneToLoad = DetermineRoom(currentRoom);
				LoadRoom(sceneToLoad,j,i);
				//GD.Print(Dungeon.MyInstance._myMap[i,j].GetDetails());
				//GD.Print("My builder Output = " + currentRoom);
			}
		}
	}

	private void LoadRoom(PackedScene theRoomToLoad, int theXCord, int theYCord)
	{
		Random rand = RandomSingleton.GetInstance();
		Vector2 roomsPosition = new Vector2((float)theXCord * MyRoomSideWidth, (float)theYCord * MyRoomHeight);
		Vector2 roomsCenter = new Vector2((float)theXCord * MyRoomSideWidth + MyRoomSideWidth/2,(float)theYCord * MyRoomHeight + MyRoomHeight/2);
		//Had to flip the Y and X coords, it was building the right rooms but grabbing the wrong details
		var roomModel = Dungeon.MyInstance.MyMap[theYCord, theXCord];
		var instancedRoom = theRoomToLoad.Instantiate();
		instancedRoom.Set(Node2D.PropertyName.GlobalPosition,roomsPosition);
		AddChild(instancedRoom);
		
		

		PackedScene bossMonster = _myMonsterDictionary.RoomDictionary["Boss"];
		if (MyRoomItem == MyEntrance)
		{
			EventBus.getInstance().SetPosition(roomsCenter);
		}
		else
		{
			if (MyRoomItem == MyExit)
			{
				LoadRoomObjects(roomsCenter, TheExitPortal);
				LoadRoomObjects(roomsCenter, bossMonster);
			}
			else if (MyRoomItem == TrapCharacter)
			{
				LoadRoomObjects(roomsCenter, TheTrapScene);
			}
			else if (_pillarCharacters.Contains(MyRoomItem))
			{
				//Bosses get placed into pillar rooms
				LoadRoomObjects(roomsCenter, bossMonster);
			}
			//We want to spawn enemies everywhere but the entrance
			else if (rand.Next(2) == 1)
			{
				//Randomly selecting from the dictionary of monster scenes.
				var keys = _myMonsterDictionary.RoomDictionary.Keys.ToList();
				//Fourth entry is the boss, this prevents it from spawning
				int keyPosition = rand.Next(keys.Count);
				string randomKey = keys[keyPosition];
				while (randomKey == "Boss")
				{
					keyPosition = rand.Next(keys.Count);
					randomKey = keys[keyPosition];
				}
				PackedScene monster = _myMonsterDictionary.RoomDictionary[randomKey];
				LoadRoomObjects(roomsCenter, monster);
			}
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
			var itemScene = GD.Load<PackedScene>("res://Game/Items/ItemProtos/" + itemData.Id.ToLower() + ".tscn");
			var itemNodeBase = itemScene.Instantiate();
			var itemNode = (ItemToPickup)itemNodeBase;
			
			itemNode.ItemData = itemData;
			itemNode.GlobalPosition = spawnPoint.Position;
			instancedRoom.AddChild(itemNode);
		}
		//AddChild(instancedRoom);
	}

	private void LoadRoomObjects(Vector2 theObjectPosition, PackedScene theObjectToPlace)
	{
		PackedScene theSceneToPlace = theObjectToPlace;
		var sceneInstance = theSceneToPlace.Instantiate();
		sceneInstance.Set(Node2D.PropertyName.GlobalPosition, theObjectPosition);
		AddChild(sceneInstance);
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
