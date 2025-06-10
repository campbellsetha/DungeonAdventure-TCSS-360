using Godot;
using UDA.Game.Player;
using UDA.inventory;

namespace UDA.Model.Items;

[GlobalClass]
public partial class ItemToPickup : Area2D
{
	[Export] public InventoryItem ItemData;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var spriteImage = GetNode<Sprite2D>("Sprite2D");
		if (ItemData != null)
			spriteImage.Texture = ItemData.Texture;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	private void OnBodyEntered(Node2D theBody)
	{
		if (theBody is Player player && ItemData != null)
		{
			player.Inventory.AddToInventory(ItemData);
		}
	}
}
