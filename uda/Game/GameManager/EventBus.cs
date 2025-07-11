using Godot;
using UDA.Game.UI;
using UDA.inventory;

namespace UDA.Game.GameManager;

public partial class EventBus : Node
{
	private static EventBus Instance;
	public override void _Ready()
	{
		//GetTree().AutoAcceptQuit = true;
		Connect(Node.SignalName.TreeExited, new Callable(this, nameof(ShutDown)));
	}

	private void ShutDown()
	{
		QueueFree();
	}

	public static EventBus getInstance()
	{
		return Instance ??= new EventBus();
	}
	[Signal]
	public delegate void DealDamageEventHandler(int theDamage);

	[Signal]
	public delegate void PlayerDealsDamageEventHandler(int theDamage);

	[Signal]
	public delegate void AddItemEventHandler(InventoryItem theItem);

	[Signal]
	public delegate void ItemAddedEventHandler(InventoryItem theItem);
	
	[Signal]
	public delegate void UseHealthPotionEventHandler(InventoryItem theItem);

	[Signal]
	public delegate void UseVisionPotionEventHandler(InventoryItem theItem);

	[Signal]
	public delegate void EnemyHitEventHandler();

	[Signal]
	public delegate void SetPlayerPositionEventHandler(Vector2 theVector2);

	[Signal]
	public delegate void PillarsCollectedEventHandler();
	
	[Signal]
	public delegate void TileSelectedEventHandler(CharacterTile theSender);

	public void TileClicked(CharacterTile theCharacterTile)
	{
		EmitSignalTileSelected(theCharacterTile);
	}

	//Keeping this as a separate signal to prevent issues with asynchronous signalling
	public void HoldingAllPillers()
	{
		EmitSignalPillarsCollected();
	}
	public void UseVisionPot(InventoryItem theItem)
	{
		EmitSignalUseVisionPotion(theItem);
	}
	public void UseHealthPot(InventoryItem theItem)
	{
		EmitSignalUseHealthPotion(theItem);
	}
	public void ItemAddedToInventory(InventoryItem theItem)
	{
		EmitSignalItemAdded(theItem);
	}
	public void AddItemToInventory(InventoryItem theItem) 
	{
		EmitSignalAddItem(theItem);
	}

	public void DealDamageToPlayer(int theDamage)
	{
		EmitSignalDealDamage(theDamage);
	}

	public void DealDamageToEnemy(int theDamage)
	{
		EmitSignalPlayerDealsDamage(theDamage);
	}

	public void SetPosition(Vector2 theVector2)
	{
		EmitSignalSetPlayerPosition(theVector2);
	}
}