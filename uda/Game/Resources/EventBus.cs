using Godot;
using System;

public partial class EventBus : Node
{
	private static EventBus Instance;
	public static EventBus getInstance()
	{
		return Instance ??= new EventBus();
	}
	// Called when the node enters the scene tree for the first time.
	[Signal]
	public delegate void DealDamageEventHandler(int theDamage);

	public void DealDamageToPlayer(int theDamage)
	{
		EmitSignalDealDamage(theDamage);
	}

	public void DealDamageToEnemy(int theDamage)
	{
		EmitSignalDealDamage(theDamage);
	}
}
