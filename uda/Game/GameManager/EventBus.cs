using Godot;
using UDA.inventory;

namespace UDA.Game.GameManager;

public partial class EventBus : Node
{
	private static EventBus Instance;
	public static EventBus getInstance()
	{
		return Instance ??= new EventBus();
	}
	// Called when the node enters the scene tree for the first time.
	[Signal]
	public delegate void DealDamageEventHandler(int theDamage);

	[Signal]
	public delegate void AddItemEventHandler(InventoryItem theItem);

	[Signal]
	public delegate void ItemAddedEventHandler(InventoryItem theItem);

	//Keeping this as a separate signal to prevent issues with asynchronous signalling
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
		EmitSignalDealDamage(theDamage);
	}
}