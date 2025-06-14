using Godot;
using System;
using UDA.Game.GameManager;
using UDA.Game.Player;

public partial class ExitPortal : Area2D
{
	private PointLight2D MyPortalLight;

	private bool PillarsCollected = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Connect(Area2D.SignalName.BodyEntered, new Callable(this, MethodName.OnBodyEntered));
		EventBus.getInstance().Connect(nameof(EventBus.PillarsCollected), new Callable(this, nameof(ReadyToWin)));
		MyPortalLight = GetNode<PointLight2D>("PointLight2D");
		MyPortalLight.Visible = false;
	}

	private void OnBodyEntered(PhysicsBody2D theBody)
	{
		if (theBody.IsInGroup("Player") && PillarsCollected)
		{
			GetTree().ChangeSceneToFile("res://Game/UI/EndScenes/win_scene.tscn");
		}
	}

	private void ReadyToWin()
	{
		PillarsCollected = true;
		MyPortalLight.Visible = true;
	}
}
