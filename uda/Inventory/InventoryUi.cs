using Godot;
using System;

public partial class InventoryUi : Control
{
	bool is_closed = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		close();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("inventoryMenu"))
		{
			GD.Print("t was just pressed\n");
			if (!is_closed)
				close();
			else 
				open();
		}
	}

	void close()
	{
		Visible = false;
		is_closed = true;
	}

	void open()
	{
		Visible = true;
		is_closed = false;
	}
}
