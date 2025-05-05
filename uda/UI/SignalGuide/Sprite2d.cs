using Godot;
using System;

/// <summary>
/// This script includes the different ways to handle signal connections in Godot.
/// NOTE: If any 'Signal" has a parameter specified in its method name, YOU need to include
/// that parameter within the method that handles the signal.
///
/// </summary>
public partial class Sprite2d : Sprite2D
{
	private int _speed = 400;
	private float _angularSpeed = Mathf.Pi;
	
	public override void _Ready()
	{
		//Connects the timer to the node that the script is attached to
		//We want to create a variable that represents the object in the script
		var timer = GetNode<Timer>("Timer");
		//When the timer emits the signal "Timeout", we call a method to handle it.
		//Method #1, easy to do can fail
		//timer.Timeout += OnTimerTimeout;
		//Method #2, a little more complex supposedly safer to run
		timer.Connect(Timer.SignalName.Timeout, new Callable(this, MethodName.OnTimerTimeout));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var velocity = Vector2.Up.Rotated(Rotation) * _speed;
		Position += velocity * (float)delta;
		Rotation += _angularSpeed * (float)delta;
	}

	/// <summary>
	/// A signal is emitted from the button and connected to the 2d sprite node.
	/// This is done through Godot and is easier to do if you need to wire signals
	/// that share the same space together.
	/// </summary>
	private void OnButtonPressed()
	{
		GD.Print("DID IT WORK, WELL");
		SetProcess(!IsProcessing());
	}

	/// <summary>
	/// This is wired directly to the signal that the timer emits.
	/// 
	/// It needs to be connected to it first. We do this by representing the obj
	/// in the class that is listening to it as a variable.
	/// 
	/// Then, we connect to the signal we want to look out for.
	/// 
	/// Note: The list of signals emitted by certain nodes can be found in the nodes tab
	/// next to the inspector, at the top right of the screen, while inside Godot.
	///
	/// After we have connected to a signal, then we need to create a method that will handle
	/// what happens when this node receives a specific signal.
	/// </summary>
	private void OnTimerTimeout()
	{
		Visible = !Visible;
	}
}
