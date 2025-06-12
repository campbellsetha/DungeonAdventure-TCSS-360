using Godot;
using UDA.inventory;
using Range = System.Range;


public partial class InventoryUi : Control
{
	bool is_closed = false;
	//[Export] private PackedScene _itemDisplayScene;
	private Inventory _inventory;
	private GridContainer _generalSlots;
	private GridContainer _idolSlots;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_inventory = GD.Load<Inventory>("res://Game/Player/player_inventory.tres");
		_generalSlots = GetNode<GridContainer>("NinePatchRect/GridContainer_General");
		_idolSlots = GetNode<GridContainer>("NinePatchRect/GridContainer_Idols");
		
		UDA.Game.GameManager.EventBus.getInstance().Connect(nameof(UDA.Game.GameManager.EventBus.ItemAdded), new Callable(this, nameof(ItemAdded)));
			
		close();
		//Refresh();
	}
	
	private void ItemAdded(InventoryItem theItem)
	{
		Refresh();
	}

	private void Refresh()
	{
		//For the general slots
		var generalSlots = _generalSlots.GetChildren();
		var keySlots = _idolSlots.GetChildren();
		var items = _inventory.GetGeneralItems();
		var keyItems = _inventory.GetKeyItems();
		
		//Loop through each general inventory position
		for (int i = 0; i < items.Count ; i++)
		{
			//Check if it is an inventory slot
			//This is mostly so we can call the update method on it
			if (generalSlots[i] is InventorySlotUi slot)
			{
				//Use the InventorySlotUI Update method to display the texture
				//Of the corresponding inventory item
				slot.Update(items[i]);
			}
		}
		
		//Repeat for keySlots, or idols
		for (int i = 0; i < keyItems.Count ; i++)
		{
			if (keySlots[i] is InventorySlotUi keySlot)
			{
				keySlot.Update(keyItems[i]);
			}
		}
	}
	
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("inventoryMenu"))
        {
            GD.Print("t was just pressed\n");
            if (!is_closed)
            {
	            close();
            }
            else
            {
	            open();
            }
                
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