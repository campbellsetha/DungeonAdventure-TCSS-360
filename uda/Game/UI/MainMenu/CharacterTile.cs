using Godot;
using UDA.Game.GameManager;
using UDA.Game.Resources;

namespace UDA.Game.UI;

[GlobalClass]
public partial class CharacterTile : Control
{
	[Export] public PlayerClassInfo ClassInfo = GD.Load<PlayerClassInfo>("res://Game/Resources/PlayerClass.tres");

	private Button _button;
	private bool _isSelected;
	private StyleBoxFlat _borderStyle;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_isSelected = false;
		_button = GetNode<Button>("Button");
		_button.Connect(Button.SignalName.ButtonDown, new Callable(this, nameof(OnButtonDown)));
	}

	private void OnButtonDown()
	{
		EventBus.getInstance().TileClicked(this);
	}

	public void SetSelected(in bool theSelection)
	{
		_isSelected = theSelection;
	}

	private bool IsSelected()
	{
		return _isSelected;
	}

	public PlayerClassInfo GetClassInfo()
	{
		return ClassInfo;
	}
}
