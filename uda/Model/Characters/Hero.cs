//using Godot;

using Godot;
using static System.Math;

namespace UDA.Model.Characters;

public abstract class Hero : DungeonCharacter
{
	protected Hero(string theName,
		int theHitPoints,
		int theAttackSpeed,
		double theHitChance,
		(int, int) theDamageRange,
		double theBlockChance,
		string theSkill) : base(theName, theHitPoints, theAttackSpeed, theHitChance, theDamageRange)
	{
		if (theBlockChance < 0)
			throw new ArgumentOutOfRangeException(nameof(theBlockChance),  theBlockChance, "Block chance must be positive");
		MyBlockChance = theBlockChance;
		MySkill = theSkill ?? throw new ArgumentNullException(nameof(theSkill), "The skill is null");
	}

	private double MyBlockChance { get; }

    protected string MySkill { get; }

    protected virtual int PerformSkill(in DungeonCharacter theTarget)
    {
	    if (theTarget == null) throw new ArgumentNullException(nameof(theTarget), "Target is null");
	    return 0;
    }
    
	public override void TakeDamage(in int theDamage)
	{
		base.TakeDamage(theDamage);
		if (!(RandomSingleton.GetInstance().NextDouble() > 1 - MyBlockChance)) MyHitPoints -= theDamage;
		{
			MyHitPoints -= theDamage;
			// Clamp health to avoid it going below 0
			MyHitPoints = Max(MyHitPoints, 0);
        }
    }

    public void Heal(in int theHealAmount)
    {
	    if (theHealAmount < 0) 
		    throw new ArgumentOutOfRangeException(nameof(theHealAmount), theHealAmount, "The heal amount is negative");
        MyHitPoints += theHealAmount;
        // Clamp health to avoid it exceeding MaxHitPoints
        MyHitPoints = Min(MyHitPoints, MyMaxHitPoints);
    }

    public override string ToString()
    {
	    return base.ToString() + $"\nBlockChance: {MyBlockChance}\nSkill: {MySkill}";
    }
}