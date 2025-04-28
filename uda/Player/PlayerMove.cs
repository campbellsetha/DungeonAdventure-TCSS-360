using Godot;
using System;

public partial class PlayerMove : CharacterBody2D
{
	private int speed = 50;
	private Vector2 currentVelocity;
	

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		handleInput();
		
		Velocity = currentVelocity;
		MoveAndSlide();
	}

	private void handleInput()
	{
		currentVelocity = Input.GetVector(
			"moveLeft", "moveRight",
			"moveUp", "moveUp");
		currentVelocity *= speed;
	}
}
