using Godot;
using System;
namespace UDA.World;
public partial class PauseMenu : Control
{
	[Signal] 
	public delegate void PauseToggledEventHandler(bool thePausedState);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("PauseGame"))
		{
			if (GetTree().Paused)
			{
				GetTree().Paused = false;
				Hide();
			}
			else
			{
				GetTree().Paused = true;
				Show();
			}
		}
	}

	private void _OnPausedPressed()
	{
		GetTree().Paused = !GetTree().Paused;
		EmitSignal(SignalName.PauseToggled, GetTree().Paused);
	}

	private void ResumeGame()
	{
		GetTree().Paused = false;
		Hide();
	}

	private void ExitGame()
	{
		GetTree().ChangeSceneToFile("res://Game/UI/MainMenu.tscn");
	}
}
