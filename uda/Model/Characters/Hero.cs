using static System.Math;

namespace UDA.Model.Characters;

public abstract class Hero : DungeonCharacter
{
	/// <summary>
	/// Constructor for this class.
	/// </summary>
	/// <param name="theName">Hero's name</param>
	/// <param name="theHitPoints">The max number of hit points this Hero has</param>
	/// <param name="theAttackSpeed">How fast the Hero can attack</param>
	/// <param name="theHitChance">How likely it is the Hero lands an attack</param>
	/// <param name="theDamageRange">The amount of damage the Hero can deal</param>
	/// <param name="theBlockChance">How likely it is the Hero heals after an attack</param>
	/// <param name="theSkill">How many hit points the Hero can recover</param>
	/// <exception cref="ArgumentException">Thrown when one the block chance is less than 0 or the skill's name is null
	/// or empty.</exception>
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

    /// <summary>
    /// Provides definition but not implementation of how the skill is to be performed. Overriden by the child
    /// classes.
    /// </summary>
    /// <param name="theTarget">The entity the character is using their skill on</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when one of the numeric values is less than 0.</exception>
    public virtual int PerformSkill(in DungeonCharacter theTarget)
    {
	    if (theTarget == null) throw new ArgumentNullException(nameof(theTarget), "Target is null");
	    return 0;
    }
    
    /// <summary>
    /// When the Hero is attacked, this method is called to reduce their hit points.
    /// </summary>
    /// <param name="theDamage">The amount of hit points to reduce the Hero's health by</param>
	public override void TakeDamage(in int theDamage)
	{
		base.TakeDamage(theDamage);
		var rand = RandomSingleton.GetInstance().NextDouble();
		// The Hero blocked the attack, so no damage is taken
		if (rand > 1 - MyBlockChance)
			return;
		MyHitPoints -= theDamage;
		// Clamp health to avoid it going below 0
		MyHitPoints = Max(MyHitPoints, 0);
	}

	/// <summary>
	/// Adds hit points to the Hero's health. Called when the Hero consumes a health potion.
	/// </summary>
	/// <param name="theHealAmount">The amount of hit points to add to the Hero's health</param>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when the heal amount is less than 0.</exception>
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

	/// <summary>
	/// String representation of this class
	/// </summary>
    public override string ToString()
    {
	    return base.ToString() + $"\nBlockChance: {MyBlockChance}\nSkill: {MySkill}";
    }
}