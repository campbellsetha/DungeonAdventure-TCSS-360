using System;
using Godot;
using static System.Math;

namespace UDA.Model.Characters;

public abstract class Hero
{

	// Should probably have a countdown timer for how often the special skill can be used 
	
	[Signal]
	public delegate void HealthChanged(int theCurrentHealth, int theMaxHealth);

	
	protected Hero(
		string theName, 
		int theHitPoints, 
		int theAttackSpeed,
		double theHitChance, 
		(int, int) theDamageRange, 
		double theBlockChance, 
		string theSkill) 
		: base(theName, theHitPoints, theAttackSpeed, theHitChance, theDamageRange)
	{
		BlockChance = theBlockChance;
		Skill = theSkill;
		MaxHitPoints = theHitPoints;
	}

	public int MaxHitPoints { get; set; }

	public double BlockChance { get; }
	
	public string Skill { get; }

	public virtual void PerformSkill(DungeonCharacter theTarget)
	{
		
	}
	public void TakeDamage(int theDamage)
	{
		if (!(DungeonCharacter.RandomNumberGenerator.NextDouble() > 1 - BlockChance)) HitPoints -= theDamage;
		{
			HitPoints -= theDamage;

			// Clamp health to avoid it going below 0
			HitPoints = Max((int)HitPoints, 0);

			// Emit the signal
			EmitSignal(nameof(HealthChanged), HitPoints, MaxHitPoints);

			void EmitSignal(string theHealthChangedName, int theHitPoints, int theMaxHitPoints)
			{
				throw new NotImplementedException();
			}
		}
	}
	
	public void Heal(int theHealAmount)
	{
		HitPoints += theHealAmount;

		// Clamp health to avoid it exceeding MaxHitPoints
		HitPoints = Min((int)HitPoints, (int)MaxHitPoints);

		// Emit the signal
		EmitSignal(nameof(HealthChanged), HitPoints, MaxHitPoints);

		void EmitSignal(string theHealthChangedName, object theHitPoints, int theMaxHitPoints)
		{
			throw new NotImplementedException();
		}
	}

	public int HitPoints { get; set; }


	//TODO: SERIALIZE THIS

	public void Connect(string theHealthchanged, Callable theCallable)
	{
		throw new NotImplementedException();
	}
}
