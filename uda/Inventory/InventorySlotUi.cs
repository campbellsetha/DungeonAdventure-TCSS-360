using Godot;
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