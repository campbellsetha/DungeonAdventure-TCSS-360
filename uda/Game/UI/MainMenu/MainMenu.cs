using Godot;

namespace UDA.Game.UI.MainMenu;

public partial class MainMenu : Control
{
    private TextureRect _loadPopup;

    public override void _Ready()
    {
        _loadPopup = GetNode<TextureRect>("Control/TextureRect");
    }

    private void NewGame()
    {
        GetTree().ChangeSceneToFile("res://World/world.tscn");
    }

    private void ExitGame()
    {
        GetTree().Quit();
    }

    private void LoadSavedGame()
    {
        _loadPopup.Visible = true;
    }

    private void BackToMainMenu()
    {
        _loadPopup.Visible = false;
    }
}