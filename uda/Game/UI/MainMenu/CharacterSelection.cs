using Godot;
using UDA.Game.Resources;
using System.Collections.Generic;
namespace UDA.Game.UI;
public partial class CharacterSelection : Control
{

	private List<CharacterTile> _characterTiles = new();
	
	private CharacterTile _selectedTile;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_characterTiles = new List<CharacterTile>()
		{
			GetNode<CharacterTile>("VBoxContainer/PanelContainer/WarriorTile"),
			GetNode<CharacterTile>("VBoxContainer/PanelContainer/ThiefTile"),
			GetNode<CharacterTile>("VBoxContainer/PanelContainer/PriestTile"),
		};
		
		GD.Print(_characterTiles.Count);

		foreach (var tile in _characterTiles)
		{
			GD.Print(tile.Name);
			tile.Connect(CharacterTile.SignalName.TileSelected, new Callable(this, nameof(OnTileSelected)));
		}
	}
	
	private void OnTileSelected(in CharacterTile theCharacterTile)
	{
		GD.Print(theCharacterTile.Name);
		foreach (var tile in _characterTiles)
			tile.SetSelected(tile == theCharacterTile);
		
		_selectedTile = theCharacterTile;
		GD.Print($"Selected Tile: {_selectedTile.GetName()}");
	}

	public PlayerClassInfo GetSelectedClass()
	{
		GD.Print($"Selected Class: {_selectedTile.GetClassInfo().MyPlayerClass}");
		return _selectedTile?.GetClassInfo();
	}

	private void OnStartPressed()
	{
		if (_selectedTile.GetClassInfo() == null)
			return;
		//Link to game manager and pass PlayerClassInfo and run game instance.
		GetTree().ChangeSceneToFile("res://Game/UI/world.tscn");
	}

	private void OnBackPressed()
	{
		Visible = false;
	}

}
