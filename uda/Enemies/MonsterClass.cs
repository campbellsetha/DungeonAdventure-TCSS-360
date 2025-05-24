using Godot;
using System;
using UDA.Model;

public partial class MonsterClass : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public Monster myClass;
	public override void _Ready()
	{
		myClass = MonsterFactory.CreateGremlin();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
