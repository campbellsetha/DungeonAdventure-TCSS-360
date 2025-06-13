using Godot;

namespace UDA.Game.UI.MainMenu;

public partial class LoadMenu : TextureRect
{
    public override void _Ready()
    {
    }

    public override void _Process(double theDelta)
    {
    }

    public void BackToMainMenu()
    {
        Visible = false;
    }
}