using Godot;
using System;

public partial class PlayerMove : CharacterBody2D
{
	[Export]
	private int _speed = 400;
	private Vector2 _currentVelocity;
	private AnimatedSprite2D _animatedSprite2D;

	public override void _Ready()
	{
		_animatedSprite2D = GetNode<AnimatedSprite2D>("PlayerAnimation");
		_animatedSprite2D.Play("default");
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		_HandleInput();
		
		Velocity = _currentVelocity;
		MoveAndSlide();
	}

	private void _HandleInput()
	{
		_currentVelocity = Input.GetVector(
			"moveLeft", "moveRight",
			"moveUp", "moveDown");
		
		_currentVelocity *= _speed;
	}
	
}
