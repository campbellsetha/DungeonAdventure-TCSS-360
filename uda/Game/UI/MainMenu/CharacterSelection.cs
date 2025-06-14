using Godot;
using UDA.Game.Resources;
using System.Collections.Generic;
using UDA.Game.GameManager;

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
			GetNode<CharacterTile>("VBoxContainer/PanelContainer/Warrior"),
			GetNode<CharacterTile>("VBoxContainer/PanelContainer/Thief"),
			GetNode<CharacterTile>("VBoxContainer/PanelContainer/Priest"),
		};
		
		//GD.Print(_characterTiles.Count);
		EventBus.getInstance().Connect(nameof(EventBus.TileSelected), new Callable(this, nameof(OnTileSelected)));
	}
	
	private void OnTileSelected(CharacterTile theCharacterTile)
	{
		GD.Print(theCharacterTile.Name);
		foreach (var tile in _characterTiles)
			tile.SetSelected(tile == theCharacterTile);
		
		_selectedTile = theCharacterTile;
		//GD.Print($"Selected Tile: {_selectedTile.GetName()}");
	}

	public PlayerClassInfo GetSelectedClass()
	{
		//GD.Print($"Selected Class: {_selectedTile.GetClassInfo().MyPlayerClass}");
		return _selectedTile?.GetClassInfo();
	}

	private void OnStartPressed()
	{
		if (_selectedTile.GetClassInfo() == null)
			return;
		
		var saveFile = "res://Game/Resources/PlayerClass.tres";
		//I think we have to load resources at runtime to modify them
		PlayerClassInfo classInfo = ResourceLoader.Load<PlayerClassInfo>("res://Game/Resources/PlayerClass.tres");
		classInfo.MyPlayerClass = _selectedTile.Name;
		ResourceSaver.Save(classInfo, saveFile);
		//We saved the updated class info, which is just the name of the class
		GetTree().ChangeSceneToFile("res://Game/GameManager/dungeonBuilder.tscn");
	}

	private void OnBackPressed()
	{
		Visible = false;
	}

}
