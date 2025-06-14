using Godot;
using UDA.Game.GameManager;

namespace UDA.inventory;
public partial class InventorySlotUi : Panel
{
	private Sprite2D _itemDisplayTexture;
	private Label _itemCountLabel;
	// private Texture2D healthTexture = 
	// 	GD.Load<Texture2D>("res://2D Pixel Dungeon Asset Pack/items and trap_animation/flasks/flasks_1_1.png");
	// private Texture2D visionTexture =
	// 	GD.Load<Texture2D>("res://2D Pixel Dungeon Asset Pack/items and trap_animation/flasks/flasks_2_1.png");
	private InventoryItem _myHeldItem;
	private int _myHeldAmountOfItem;
	
	public override void _Ready()
	{
		Connect(Panel.SignalName.GuiInput, new Callable(this, nameof(ItemClicked)));
		_itemDisplayTexture = GetNode<Sprite2D>("Sprite2D");
		_itemCountLabel = GetNode<Label>("ItemCountLabel");
		_itemDisplayTexture.Visible = true;
	}
	
	public void Update(InventoryItem theItem)
	{
		_myHeldItem = theItem;
		_myHeldAmountOfItem = theItem.ItemCount;
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
			_itemCountLabel.Text = theItem.ItemCount.ToString();
		}
	}
	
	void ItemClicked(InputEvent theEvent)
	{
		if (theEvent is not InputEventMouseButton { Pressed: true }) return;
		if (_myHeldItem == null || _myHeldItem.ItemCount == 0) return;
		
		//These signals should never emit unless the item exits in the players inventory
		switch (_myHeldItem.Name)
		{
			case "healpotion":
				GD.Print("Used a health potion");
				//Send the signal so it gets updated in the inventory
				EventBus.getInstance().UseHealthPot(_myHeldItem);
				//Then call update
				Update(_myHeldItem);
				break;
			case "visionpotion":
				GD.Print("Used a vision potion");
				EventBus.getInstance().UseVisionPot(_myHeldItem);
				Update(_myHeldItem);
				break;
		}

		//Emit the use item if the item exists?
	}
}

