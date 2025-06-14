using Godot;
using System;
using UDA.Game.GameManager;
using UDA.Model;

public partial class Trap : Area2D
{
	private AnimatedSprite2D mySprite;
	public override void _Ready()
	{
		Connect(Area2D.SignalName.BodyEntered, new Callable(this, MethodName.OnBodyEntered));
		mySprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		mySprite.Play("default");
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	private void OnBodyEntered(PhysicsBody2D theBody)
	{
		GD.Print("Hit the trap");
		Random rand = RandomSingleton.GetInstance();
		EventBus.getInstance().DealDamageToPlayer(rand.Next(1,20));
		QueueFree();
	}
}
