using Godot;
using System;

public partial class DefaultEntity : Node2D
{

	private int HEALTH { get; set; } = 100;
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
