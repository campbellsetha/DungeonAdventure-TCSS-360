using Godot;
<<<<<<< Updated upstream
using UDA.inventory;

public partial class InventorySlotUi : Panel
{
    // Commenting this out right now because the errors are preventing me from testing code
    //private Sprite2D item_display_texture = $"item_display";
    private void Update(Inventory_item theItem)
    {
        //if (!theItem)
        //item_display_texture;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double theDelta)
    {
    }
}
=======
using System;

namespace UDA.inventory;
public partial class InventorySlotUi : Panel
{
	private Sprite2D _itemDisplayTexture;
	private Label _itemCountLabel;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_itemDisplayTexture = GetNode<Sprite2D>("Sprite2D");
		_itemCountLabel = GetNode<Label>("ItemCountLabel");
	}
	
	public void Update(InventoryItem theItem)
	{
		if (theItem == null)
		{
			_itemDisplayTexture.Visible = false;
			_itemDisplayTexture.Texture = null;
			_itemCountLabel.Text = "";
		}
		else
		{
			_itemDisplayTexture.Visible = true;
			_itemDisplayTexture.Texture = theItem.Texture;
			_itemCountLabel.Text = theItem.ItemCount > 1 ?
				theItem.ItemCount.ToString() : "";
		}
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
>>>>>>> Stashed changes
