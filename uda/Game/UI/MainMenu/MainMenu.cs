using Godot;

namespace UDA.Game.UI.MainMenu;

public partial class MainMenu : Control
{
<<<<<<< Updated upstream
    private TextureRect _loadPopup;

    public override void _Ready()
    {
        _loadPopup = GetNode<TextureRect>("Control/TextureRect");
    }

    private void NewGame()
    {
        GetTree().ChangeSceneToFile("res://Game/GameManager/dungeonBuilder.tscn");
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
=======

    [Export] private NodePath _loadMenuPath;
    [Export] private NodePath _characterSelectionPath;
    [Export] private NodePath _helpMenuPath;
    
    private LoadMenu _loadPopup;
    private CharacterSelection _characterSelectionPopup;
    private PopUp _helpPopup;

    public override void _Ready()
    {
        _loadPopup = GetNode<LoadMenu>(_loadMenuPath);
        _characterSelectionPopup = GetNode<CharacterSelection>(_characterSelectionPath);
        _helpPopup = GetNode<PopUp>(_helpMenuPath);
    }

    private void NewGame()
    {
        _characterSelectionPopup.Visible = true;
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
        _characterSelectionPopup.Visible = false;
        _loadPopup.Visible = false;
        _helpPopup.Visible = false;
    }

    private void OnHelpPressed()
    {
        _helpPopup.Visible = true;
>>>>>>> Stashed changes
    }
}