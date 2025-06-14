using Godot;
using System;
using UDA.Game.UI.MainMenu;

public partial class WinScene : Node2D
{
    private Button playAgainButton;
    private Button ExitButton;
    public override void _Ready()
    {
        playAgainButton = GetNode<Button>("%PlayAgain");
        playAgainButton.Connect(Button.SignalName.ButtonDown, new Callable(this, nameof(ResetGameState)));
        ExitButton = GetNode<Button>("%Quit");
        ExitButton.Connect(Button.SignalName.ButtonDown, new Callable(this, nameof(ExitGame)));
    }

    public void ResetGameState()
    {
        GetTree().ChangeSceneToFile("res://Game/UI/MainMenu/MainMenu.tscn");
    }
    public void ExitGame()
    {
        GetTree().Quit();
    }
}
