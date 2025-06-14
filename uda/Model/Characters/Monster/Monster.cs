namespace UDA.Model.Characters.Monster;

/// <summary>
/// An implementation of the Monster entity.
/// </summary>
public class Monster : DungeonCharacter
{
    // If the Monster's health drops below this percentage, they are stunned and can no longer heal.
    private readonly double _myStunThreshold;
    // The amount the Monster can heal.
    private (int Min, int Max) MyHealRange { get; }
    // The percent chance that a Monster is able to heal after an attack.
    private double MyHealChance { get; }

    /// <summary>
    /// Constructor for this class.
    /// </summary>
    /// <param name="theName">Monster's name (Skeleton, Ogre, or Gremlin)</param>
    /// <param name="theHitPoints">The max number of hit points this Monster has</param>
    /// <param name="theAttackSpeed">How fast the Monster can attack</param>
    /// <param name="theHitChance">How likely it is the Monster lands an attack</param>
    /// <param name="theDamageRange">The amount of damage the Monster can deal</param>
    /// <param name="theHealChance">How likely it is the Monster heals after an attack</param>
    /// <param name="theHealRange">How many hit points the Monster can recover</param>
    /// <param name="theStunThreshold">The percentage of health the Monster has to lose to be stunned</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when one of the numeric values is less than 0.</exception>
    public Monster(in string theName,
        in int theHitPoints,
        in int theAttackSpeed,
        in double theHitChance,
        in (int, int) theDamageRange,
        in double theHealChance,
        in (int, int) theHealRange,
        in double theStunThreshold) : base(theName, theHitPoints, theAttackSpeed, theHitChance, theDamageRange)
    {
        if (theStunThreshold < 0) 
            throw new ArgumentOutOfRangeException(nameof(theStunThreshold), theStunThreshold, 
                "The stun threshold is negative");
        if (theHealRange.Item1 < 0 || theHealRange.Item2 < 0)
            throw new ArgumentOutOfRangeException(nameof(theHealRange), theHealRange, 
                "The heal range is negative");
        if (theHealChance < 0)
            throw new ArgumentOutOfRangeException(nameof(theHealChance),  theHealChance, 
                "The heal chance is negative");
        _myStunThreshold = theStunThreshold;
        MyHealRange = theHealRange;
        MyHealChance = theHealChance;
    }

    // Called by the TakeDamage method. Determines if the Monster can heal and adds to their HP if they can.
    private void Heal()
    {
        var rand = RandomSingleton.GetInstance();
        /* To demonstrate how this line works, imagine the heal chance is 0.2 as an example. The randomly generated
         value has to be greater than 0.8 for the Monster to heal, and there are more values in the range 0.0 - 0.8 to 
         choose from than in the range 0.8 - 1.0. Most of the chance-based logic in the Model works like this. */
        if (!(rand.NextDouble() > 1 - MyHealChance)) return;
        var healAmount = rand.Next(MyHealRange.Min, MyHealRange.Max + 1);
        // Prevents health from going over max health.
        if (healAmount + MyHitPoints > MyMaxHitPoints) healAmount = MyMaxHitPoints - MyHitPoints;
        MyHitPoints += healAmount;
    }

    /// <summary>
    /// Constructor for this class.
    /// </summary>
    /// <param name="theDamage">The damage dealt to the Monster</param>
    public override void TakeDamage(in int theDamage)
    {
        base.TakeDamage(theDamage);
        MyHitPoints -= theDamage;
        // Clamp health to avoid it going below 0
        MyHitPoints = Math.Max(MyHitPoints, 0);
        // A monster can keep healing as long as a certain percentage of their health hasn't been lost.
        if (MyMaxHitPoints - (MyMaxHitPoints * _myStunThreshold) <= MyHitPoints) Heal();
        else Console.WriteLine("Monster is stunned and cannot heal");
    }

    /// <summary>
    /// String representation of the Monster.
    /// </summary>
    ///
    public override string ToString()
    {
        return base.ToString()
               + $"\nStunThreshold: {_myStunThreshold}\nHealChance: {MyHealChance}\n" +
               $"HealRange: {MyHealRange}";
    }
}