using Godot;
using System;

public partial class SelectType : HBoxContainer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var characterTileScenes = GD.Load<PackedScene[]>("res://Game/UI/CharacterTile.tscn");

		var classes = new[]
		{
			new {}
		};
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
