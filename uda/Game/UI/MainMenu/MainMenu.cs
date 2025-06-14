using Godot;

namespace UDA.Game.UI.MainMenu;

public partial class MainMenu : Control
{

    [Export] private NodePath _loadMenuPath;
    [Export] private NodePath _characterSelectionPath;
    [Export] private NodePath _helpMenuPath;
    
    private LoadMenu _loadPopup;
    private CharacterSelection _characterSelectionPopup;
    private PopUp _helpMenuPopup;

    public override void _Ready()
    {
        _loadPopup = GetNode<LoadMenu>(_loadMenuPath);
        _characterSelectionPopup = GetNode<CharacterSelection>(_characterSelectionPath);
        _helpMenuPopup = GetNode<PopUp>(_helpMenuPath);
    }

    private void NewGame()
    {
        _characterSelectionPopup.Visible = true;
    }

    private void ExitGame()
    {
        Free();
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

    private void OnHelpPressed()
    {
        _helpMenuPopup.Visible = true;
    }
}