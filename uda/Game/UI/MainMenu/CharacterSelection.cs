using Godot;
using UDA.Game.Resources;
<<<<<<< Updated upstream
=======
using System.Collections.Generic;
//
>>>>>>> Stashed changes

namespace UDA.Game.UI;
public partial class CharacterSelection : TextureRect
{

<<<<<<< Updated upstream
	private List<CharacterTile> _characterTiles = new();

	private PackedScene _characterTileScene;
	private CharacterTile _selectedTile;
	private PlayerClassInfo _selectedClassInfo;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_characterTileScene = GD.Load<PackedScene>("res://Game/UI/MainMenu/CharacterTile.tscn");
		AddCharacterClasses();
	}

	private void AddCharacterClasses()
	{
		var characters = new[]
		{
			new { Name = "Priest", Frame = GD.Load<SpriteFrames>("res://Game/UI/MainMenu/Priest.tres") },
			new { Name = "Warrior", Frame = GD.Load<SpriteFrames>("res://Game/UI/MainMenu/Warrior.tres") },
			new { Name = "Thief", Frame = GD.Load<SpriteFrames>("res://Game/UI/MainMenu/Thief.tres") }
		};
		
		HBoxContainer tileContainer = GetNode<HBoxContainer>("PanelContainer");
		
		foreach (var character in characters)
		{
			var characterInstance = _characterTileScene.Instantiate<CharacterTile>();
			tileContainer.AddChild(characterInstance);
			
			characterInstance.SetLabel(character.Name);
			characterInstance.SetSpriteFrames(character.Frame);

			_characterTiles.Add(characterInstance);

			characterInstance.GuiInput += input =>
			{
				if (input is InputEventMouseButton mouseEvent &&
					mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
				{
					OnTitleSelected(characterInstance);
				}
			};
		}
	}

	private void OnTitleSelected(CharacterTile theCharacterTile)
	{
		foreach (var tile in _characterTiles)
			tile.Deselect();

		theCharacterTile.Select();
		_selectedTile = theCharacterTile;
		_selectedClassInfo = theCharacterTile.GetClassInfo();
=======
	private List<CharacterTile> _characterTiles;


	private CharacterTile _currentSelectedTile;
	
	public PlayerClassInfo SelectedClassInfo;

	private PlayerClassInfo _warrior = GD.Load<PlayerClassInfo>("res://CharacterSelection/Warrior.tres");
	private PlayerClassInfo _thief = GD.Load<PlayerClassInfo>("res://CharacterSelection/Thief.tres");
	private PlayerClassInfo _priest = GD.Load<PlayerClassInfo>("res://CharacterSelection/Priest.tres");
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_characterTiles = new List<CharacterTile>()
		{
			GetNode<CharacterTile>("VBoxContainer/PanelContainer/WarriorTile"),
			GetNode<CharacterTile>("VBoxContainer/PanelContainer/ThiefTile"),
			GetNode<CharacterTile>("VBoxContainer/PanelContainer/PriestTile")
		};

		foreach (var tile in _characterTiles)
		{
			tile.CharacterTileSelected += OnTileSelected;
		}
	}

	public void OnTileSelected(CharacterTile theSelectedTile)
	{
		foreach (var tile in _characterTiles)
			tile.SetSelected(tile == theSelectedTile);

		_currentSelectedTile = theSelectedTile;
		GD.Print($"Selected Tile: {_currentSelectedTile.GetName()}");
>>>>>>> Stashed changes
	}

	private void OnStartPressed()
	{
<<<<<<< Updated upstream
		GetTree().ChangeSceneToFile("res://Game/UI/world.tscn");
=======
		
		if (_currentSelectedTile == null)
			return;
		
		//var GameManagerInstance = GetTree().Root.GetNode<GameManager>("");
		
		//GameManager.GameManager._selectedClassInfo = _currentSelectedTile.GetClassInfo();

		GetTree().ChangeSceneToFile("res://Game/GameManager/dungeonBuilder.tscn");
>>>>>>> Stashed changes
	}

	private void OnBackPressed()
	{
		Visible = false;
	}

}
