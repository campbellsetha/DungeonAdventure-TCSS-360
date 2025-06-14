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
		if (theBlockChance <= 0)
			throw new ArgumentException("Block chance must be positive");
		MyBlockChance = theBlockChance;
		if (string.IsNullOrEmpty(theSkill))
			throw new ArgumentException("Skill is null or empty");
		MySkill = theSkill;
	}

	private double MyBlockChance { get; }

    protected string MySkill { get; }

    public virtual int PerformSkill(in DungeonCharacter theTarget)
    {
	    if (theTarget == null) throw new ArgumentNullException(nameof(theTarget), "Target is null");
	    return 0;
    }
    
	public override void TakeDamage(in int theDamage)
	{
		base.TakeDamage(theDamage);
		var rand = RandomSingleton.GetInstance().NextDouble();
		if (rand > 1 - MyBlockChance)
			return;
		MyHitPoints -= theDamage;
		// Clamp health to avoid it going below 0
		MyHitPoints = Max(MyHitPoints, 0);
	}

    public void Heal(in int theHealAmount)
    {
	    if (theHealAmount <= 0) 
		    throw new ArgumentOutOfRangeException(nameof(theHealAmount), theHealAmount, 
			    "The heal amount must be positive");
	    if (MyHitPoints == 0)
		    throw new ArgumentException("Cannot heal a dead character");
        MyHitPoints += theHealAmount;
        // Clamp health to avoid it exceeding MaxHitPoints
        MyHitPoints = Min(MyHitPoints, MyMaxHitPoints);
    }

    public override string ToString()
    {
	    return base.ToString() + $"\nBlockChance: {MyBlockChance}\nSkill: {MySkill}";
    }
}