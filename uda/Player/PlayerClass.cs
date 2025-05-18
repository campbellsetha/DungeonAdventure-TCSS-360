using Godot;
using System;
using UDA.Model;

public partial class PlayerClass : Node2D
{
	//I am going to attach a button on a control node in the startup that initializes this object
	private Hero MY_CLASS { get; set; }
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
	
	
}
