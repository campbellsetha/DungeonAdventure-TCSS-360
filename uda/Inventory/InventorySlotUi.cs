using Godot;
using System;
import UDA.inventory.Inventory_item;
public partial class InventorySlotUi : Panel
{
	private Sprite2D item_display_texture = $"item_display";
	void update(Inventory_item theItem)
	{
		if (!theItem)
			item_display_texture
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
