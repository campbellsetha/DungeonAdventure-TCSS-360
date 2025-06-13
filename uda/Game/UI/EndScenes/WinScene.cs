using Godot;
using System;
using UDA.Game.UI.MainMenu;

public partial class WinScene : Node2D
{
    public void ResetGameState()
    {
        // clear and refresh game manager still needed.
        GetTree().ChangeSceneToFile("res://Game/UI/MainMenu/MainMenu.tscn");
    }
    public void ExitGame()
    {
        GetTree().Quit();
    }
}
