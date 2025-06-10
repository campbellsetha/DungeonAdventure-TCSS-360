using Godot;
<<<<<<< Updated upstream

public partial class InventoryUi : Control
{
    private bool is_closed;
=======
using System;
using System.ComponentModel;
using UDA.inventory;
using Range = System.Range;


public partial class InventoryUi : Control
{
	bool is_closed = false;
	[Export] private PackedScene _itemDisplayScene;
	private Inventory _inventory;
	private GridContainer _generalSlots;
	private GridContainer _idolSlots;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_inventory = GD.Load<Inventory>("res://Game/Player/player_inventory.tres");
		_generalSlots = GetNode<GridContainer>("NinePatchRect/GridContainer_General");
		_idolSlots = GetNode<GridContainer>("NinePatchRect/GridContainer_Idol");
			
		close();
		Refresh();
	}

	private void Refresh()
	{
		foreach (Node node in _generalSlots.GetChildren())
			node.QueueFree();
		
		foreach (Node node in _idolSlots.GetChildren())
			node.QueueFree();

		foreach (var item in _inventory.GetGeneralItems())
		{
			var slot = _itemDisplayScene.Instantiate<InventorySlotUi>();
			slot.Update(item);
			_generalSlots.AddChild(slot);
		}

		foreach (var item in _inventory.GetKeyItems())
		{
			var slot = _itemDisplayScene.Instantiate<InventorySlotUi>();
			slot.Update(item);
			_idolSlots.AddChild(slot);
		}
	}
>>>>>>> Stashed changes

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        close();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("inventoryMenu"))
        {
            GD.Print("t was just pressed\n");
            if (!is_closed)
                close();
            else
                open();
        }
    }

    private void close()
    {
        Visible = false;
        is_closed = true;
    }

    private void open()
    {
        Visible = true;
        is_closed = false;
    }
}