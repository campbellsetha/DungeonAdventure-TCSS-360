using Godot;
using UDA.Game.Resources;

namespace UDA.Game.UI;
public partial class CharacterSelection : TextureRect
{

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
	}

	private void OnStartPressed()
	{
		GetTree().ChangeSceneToFile("res://Game/UI/world.tscn");
	}

	private void OnBackPressed()
	{
		Visible = false;
	}

}
