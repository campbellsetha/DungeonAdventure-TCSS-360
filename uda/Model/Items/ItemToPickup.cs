using Godot;
using UDA.Game.GameManager;
using UDA.inventory;

namespace UDA.Model.Items;

[GlobalClass]
public partial class ItemToPickup : Area2D
{
	[Export] public InventoryItem ItemData;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Connect(Area2D.SignalName.AreaEntered, new Callable(this, MethodName.OnBodyEntered));
		var spriteImage = GetNode<Sprite2D>("Sprite2D");
		if (ItemData != null)
			spriteImage.Texture = ItemData.Texture;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	private void OnBodyEntered(Area2D theBody)
	{
		//GD.Print("Found an item");
		if (!theBody.IsInGroup("Monster") && ItemData != null)
		{
			EventBus.getInstance().AddItemToInventory(ItemData);
			QueueFree();
		}
	}
}
