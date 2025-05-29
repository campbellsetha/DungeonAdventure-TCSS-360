using System;
using Godot;

namespace UDA.Player;

public partial class Hurtbox : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void _OnHurtBoxEntered(Area2D theHitbox)
	{
		if (theHitbox.Name == "HitBox")
		{
			Console.WriteLine(theHitbox.GetParent().Name);
		}
	}
}