using Godot;
using System;

public partial class PlayerMove : CharacterBody2D
{
	[Export] private int _speed = 200;
	private Vector2 _currentVelocity;
	private AnimatedSprite2D _animatedSprite2D;

	public override void _Ready()
	{
		var thisHurtbox = GetNode<Area2D>("Hurtbox");
		thisHurtbox.Connect(Area2D.SignalName.AreaEntered, new Callable(this, MethodName.OnHurtBoxEntered));
		_animatedSprite2D = GetNode<AnimatedSprite2D>("PlayerAnimation");
		_animatedSprite2D.Play("default");
		AddToGroup("player");
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		_ChangeAnimation(_currentVelocity);
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		//Check player input
		_HandleInput();
		//Update velocity
		Velocity = _currentVelocity;
		//Trigger physics process and move the player
		MoveAndSlide();
		_HandleCollision();
	}

	private void _HandleCollision()
	{
		//Currently prints out each execution of a collision.
		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			//KinematicCollision2D collision = GetSlideCollision(i);
			//GD.Print("Collided with: ", (collision.GetCollider() as Node).Name);
			//var collider = GetSlide
		}
	}

	private void _HandleInput()
	{
		_currentVelocity = Input.GetVector(
			"moveLeft", "moveRight",
			"moveUp", "moveDown");
		_currentVelocity *= _speed;
	}

	/// <summary>
	/// Should run at every processing step.
	/// Checks inputVector and updates current animation to reflect that.
	/// E.g. Input vector is -200.X is moving left so we play the left animation.
	/// This is called from _Process because running things in the physics process method can cause slowdown and
	/// runtime issues.
	/// </summary>
	private void _ChangeAnimation(Vector2 theCurrentVector2)
	{
		//If the vector value is greater than zero we are moving
		if (!(theCurrentVector2.Length() > 0))
		{
			_animatedSprite2D.Play("default");
			return;
		}
		//Horizontal movement versus vertical movement
		if (Math.Abs(theCurrentVector2.X) > Math.Abs(theCurrentVector2.Y))
		{
			//Horizontal,
			_animatedSprite2D.Play(theCurrentVector2.X < 0 ? "walkLeft" : "walkRight");
		}
		else
		{
			//Vertical
			_animatedSprite2D.Play(theCurrentVector2.Y < 0 ? "walkUp" : "walkDown");
		}
	}

	private void OnHurtBoxEntered(Area2D theAreaThatEntered)
	{
		GD.Print("OUT MY SPACE G!");
	}
	
}
