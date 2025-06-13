using Godot;
using UDA.Game.Resources;

namespace UDA.Game.UI;

[GlobalClass]
public partial class CharacterTile : Control
{
	[Export] public PlayerClassInfo ClassInfo { get; set; }

	[Signal]
	public delegate void TileSelectedEventHandler(CharacterTile sender);

	private Button _button;
	private bool _isSelected;
	private StyleBoxFlat _borderStyle;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_isSelected = false;
		_button = GetNode<Button>("Button");
		_button.Pressed += OnPressed;
	}

	private void OnPressed()
	{
		EmitSignal(SignalName.TileSelected, this);
	}

	public void SetSelected(in bool theSelection)
	{
		_isSelected = theSelection;
	}

	private bool IsSelected()
	{
		return _isSelected;
	}

	public void SetPlayerInfo(in PlayerClassInfo theClassInfo)
	{
		ClassInfo = theClassInfo;
	}

	public PlayerClassInfo GetClassInfo()
	{
		return ClassInfo;
	}
}
