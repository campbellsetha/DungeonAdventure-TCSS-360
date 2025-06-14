using Godot;
using System;

public partial class GameOverScene : Node2D
{
	private Button playAgainButton;
	private Button exitButton;
	public override void _Ready()
	{
		playAgainButton = GetNode<Button>("%Redo");
		playAgainButton.Connect(Button.SignalName.ButtonDown, new Callable(this, nameof(OnRedoButtonDown)));
		exitButton = GetNode<Button>("%Exit");
		exitButton.Connect(Button.SignalName.ButtonDown, new Callable(this, nameof(OnExitButtonDown)));
	}

	private void OnRedoButtonDown()
	{
		GetTree().ChangeSceneToFile("res://Game/UI/MainMenu/MainMenu.tscn");
	}

	private void OnExitButtonDown()
	{
		GetTree().Quit();
	}
}
