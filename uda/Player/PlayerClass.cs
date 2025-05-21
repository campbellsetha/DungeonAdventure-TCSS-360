using Godot;
using System;
using UDA.Model;

[GlobalClass]
public partial class PlayerClass : Node2D
{
	//I am going to attach a button on a control node in the startup that initializes this object
	public Hero MY_CLASS { get; set; }
	private string MY_NAME;

	private PlayerClass()
	{
	}

	public void SetClass(string thePlayerName, string thePlayerClass)
	{
		if (thePlayerName == null)
		{
			throw new ArgumentException("The name cannot be null");
		}

		if (thePlayerClass == null)
		{
			throw new ArgumentException("The class cannot be null");
		}

		//TODO: Find an appropriate exception
		if (MY_CLASS != null)
		{
			throw new Exception("The class has already been defined");
		}
		switch (thePlayerClass)
		{
			case "Warrior":
				MY_CLASS = HeroFactory.CreateWarrior(MY_NAME);
				break;
			case "Thief":
				MY_CLASS = HeroFactory.CreateThief(MY_NAME);
				break;
			case "Priest":
				MY_CLASS = HeroFactory.CreatePriest(MY_NAME);
				break;
		}
	}


	//I guess since this is going to be called from other places we can pass it a reference of itself?
	public void TestSkill(DungeonCharacter theCharacter)
	{
		MY_CLASS.PerformSkill(theCharacter);
	}
	
	
	//Hopefully this is in the right spot for proper serialization with a constructed dictionary
	//Should allow us to modify class parameters and have them be restored on load.
	//Might also allow for instantiating the player into scenes as needed instead of having to run through the scene tree.
	public Godot.Collections.Dictionary<string, Variant> Save()
	{
		return new Godot.Collections.Dictionary<string, Variant>()
		{
			{"Name", MY_CLASS.EntityName },
			{"MaxHitPoints", MY_CLASS.MaxHitPoints},
			{"CurrentHP", MY_CLASS.HitPoints},
			{"MinDamage", MY_CLASS.DamageRange.Min},
			{"MaxDamage", MY_CLASS.DamageRange.Max},
			{"AttackSpeed", MY_CLASS.AttackSpeed},
			{"HitChance", MY_CLASS.HitChance},
			{"Skill", MY_CLASS.Skill} ,
			{"BlockChance", MY_CLASS.BlockChance}
		};
	}
	
}
	
	

